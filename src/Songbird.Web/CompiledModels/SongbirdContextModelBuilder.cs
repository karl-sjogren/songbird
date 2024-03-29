﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable disable

namespace Songbird.Web.CompiledModels
{
    public partial class SongbirdContextModel
    {
        partial void Initialize()
        {
            var fikaMatchUser = FikaMatchUserEntityType.Create(this);
            var application = ApplicationEntityType.Create(this);
            var applicationLogFilter = ApplicationLogFilterEntityType.Create(this);
            var binaryFile = BinaryFileEntityType.Create(this);
            var customer = CustomerEntityType.Create(this);
            var fikaMatch = FikaMatchEntityType.Create(this);
            var fikaSchedule = FikaScheduleEntityType.Create(this);
            var plannedProjectTime = PlannedProjectTimeEntityType.Create(this);
            var planningBoard = PlanningBoardEntityType.Create(this);
            var project = ProjectEntityType.Create(this);
            var scheduledOfficeRole = ScheduledOfficeRoleEntityType.Create(this);
            var scheduledStatus = ScheduledStatusEntityType.Create(this);
            var user = UserEntityType.Create(this);
            var userPhoto = UserPhotoEntityType.Create(this);
            var userSchedule = UserScheduleEntityType.Create(this);

            FikaMatchUserEntityType.CreateForeignKey1(fikaMatchUser, fikaMatch);
            FikaMatchUserEntityType.CreateForeignKey2(fikaMatchUser, user);
            ApplicationEntityType.CreateForeignKey1(application, project);
            ApplicationLogFilterEntityType.CreateForeignKey1(applicationLogFilter, application);
            CustomerEntityType.CreateForeignKey1(customer, binaryFile);
            FikaMatchEntityType.CreateForeignKey1(fikaMatch, fikaSchedule);
            PlannedProjectTimeEntityType.CreateForeignKey1(plannedProjectTime, project);
            PlannedProjectTimeEntityType.CreateForeignKey2(plannedProjectTime, userSchedule);
            ProjectEntityType.CreateForeignKey1(project, customer);
            ProjectEntityType.CreateForeignKey2(project, binaryFile);
            ScheduledOfficeRoleEntityType.CreateForeignKey1(scheduledOfficeRole, userSchedule);
            ScheduledStatusEntityType.CreateForeignKey1(scheduledStatus, userSchedule);
            UserScheduleEntityType.CreateForeignKey1(userSchedule, planningBoard);
            UserScheduleEntityType.CreateForeignKey2(userSchedule, user);

            FikaMatchEntityType.CreateSkipNavigation1(fikaMatch, user, fikaMatchUser);
            UserEntityType.CreateSkipNavigation1(user, fikaMatch, fikaMatchUser);

            FikaMatchUserEntityType.CreateAnnotations(fikaMatchUser);
            ApplicationEntityType.CreateAnnotations(application);
            ApplicationLogFilterEntityType.CreateAnnotations(applicationLogFilter);
            BinaryFileEntityType.CreateAnnotations(binaryFile);
            CustomerEntityType.CreateAnnotations(customer);
            FikaMatchEntityType.CreateAnnotations(fikaMatch);
            FikaScheduleEntityType.CreateAnnotations(fikaSchedule);
            PlannedProjectTimeEntityType.CreateAnnotations(plannedProjectTime);
            PlanningBoardEntityType.CreateAnnotations(planningBoard);
            ProjectEntityType.CreateAnnotations(project);
            ScheduledOfficeRoleEntityType.CreateAnnotations(scheduledOfficeRole);
            ScheduledStatusEntityType.CreateAnnotations(scheduledStatus);
            UserEntityType.CreateAnnotations(user);
            UserPhotoEntityType.CreateAnnotations(userPhoto);
            UserScheduleEntityType.CreateAnnotations(userSchedule);

            AddAnnotation("ProductVersion", "7.0.0-preview.4.22229.2");
            AddAnnotation("Relational:MaxIdentifierLength", 128);
            AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}
