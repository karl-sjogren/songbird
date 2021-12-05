using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using AutoMapper.EquivalencyExpression;
using LetterAvatars.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using RobotsTxt;
using Serilog;
using Songbird.Web.Contracts;
using Songbird.Web.Extensions;
using Songbird.Web.HostedServices;
using Songbird.Web.Options;
using Songbird.Web.Services;

namespace Songbird.Web {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private bool IsLocalEnvironment => Configuration["IsLocal"] == bool.TrueString;
        private bool DisableHostedServices => Configuration["DisableHostedServices"] == bool.TrueString;

        public void ConfigureServices(IServiceCollection services) {
            // Refactored extension methods
            services.AddAuthentication(Configuration);
            services.AddAvatars();
            services.AddControllersWithSerialization();
            services.AddCompression();
            services.AddElasticsearch();
            services.AddEntityFramework(Configuration);
            services.AddRobotsTxt();
            services.AddSinglePageApplication();

            // Misc
            services.AddSingleton(Configuration); // Refactor into options
            services.AddAutoMapper(configuration => configuration.AddCollectionMappers(), typeof(Startup).Assembly);
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>();
            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

            // Options
            services.Configure<AzureAdOptions>(Configuration.GetSection("AzureAd"));
            services.Configure<ElasticsearchOptions>(Configuration.GetSection("Elasticsearch"));
            services.Configure<GraphApiOptions>(Configuration.GetSection("GraphApi"));
            services.Configure<FikaBuddiesOptions>(Configuration.GetSection("FikaBuddies"));

            // HttpClients
            services.AddHttpClient();
            services.AddHttpClient<ISlackMessagingService, SlackMessagingService>();

            // Services
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<IBinaryFileService, BinaryFileService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IFikaScheduleService, FikaScheduleService>();
            services.AddScoped<ILogGraphService, LogGraphService>();
            services.AddScoped<ILogSearchService, LogSearchService>();
            services.AddScoped<ILunchGameService, LunchGameService>();
            services.AddScoped<ILunchGamingDateService, LunchGamingDateService>();
            services.AddScoped<IUserService, UserService>();

            // Hosted services
            if(!DisableHostedServices) {
                services.AddHostedService<CalculateFikaBuddiesHostedService>();
                services.AddHostedService<NotifyFikaBuddiesOnSlackHostedService>();
            }
        }

        public void Configure(IApplicationBuilder app) {
            if(IsLocalEnvironment) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseResponseCompression();

            app.UseRobotsTxt();

            var cultureInfo = new CultureInfo("sv-SE");
            app.UseRequestLocalization(new RequestLocalizationOptions {
                DefaultRequestCulture = new RequestCulture(cultureInfo),
                SupportedCultures = new List<CultureInfo> {
                    cultureInfo
                },
                SupportedUICultures = new List<CultureInfo> {
                    cultureInfo
                }
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.Use(async (context, next) => {
                if(!context.User.Identity.IsAuthenticated) {
                    if(context.Request.Path.StartsWithSegments("/api")) {
                        var problemDetailsFactory = context.RequestServices?.GetRequiredService<ProblemDetailsFactory>();
                        var problemDetails = problemDetailsFactory.CreateProblemDetails(context, statusCode: 401, detail: "API access requires authentication by OpenID or API key.");

                        var json = JsonSerializer.Serialize(problemDetails);
                        var buffer = Encoding.UTF8.GetBytes(json);

                        context.Response.StatusCode = 401;
                        await context.Response.BodyWriter.WriteAsync(buffer);
                        return;
                    }

                    await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme);
                } else {
                    await next();
                }
            });

            app.UseAvatars("/avatars");

            static void onPrepareResponse(StaticFileResponseContext ctx) {
                var cacheDuration = TimeSpan.FromDays(365).TotalSeconds;
                ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + cacheDuration + ",immutable";
            }

            app.UseStaticFiles(new StaticFileOptions { OnPrepareResponse = onPrepareResponse });
            app.UseSpaStaticFiles(new StaticFileOptions { OnPrepareResponse = onPrepareResponse });

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseWhen(
                context => !context.Request.Path.StartsWithSegments("/api"),
                appBuilder => appBuilder.UseSpa(spa => {
                    spa.Options.SourcePath = "../Songbird.Frontend";

                    if(IsLocalEnvironment) {
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
                    }
                })
            );
        }
    }
}
