﻿// <auto-generated />
using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Songbird.Web.Models;

#pragma warning disable 219, 612, 618
#nullable disable

namespace Songbird.Web.CompiledModels
{
    partial class LunchGamingDateEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "Songbird.Web.Models.LunchGamingDate",
                typeof(LunchGamingDate),
                baseEntityType);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(Guid),
                propertyInfo: typeof(ModelBase).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ModelBase).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd,
                afterSaveBehavior: PropertySaveBehavior.Throw);
            id.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var createdAt = runtimeEntityType.AddProperty(
                "CreatedAt",
                typeof(DateTime),
                propertyInfo: typeof(ModelBase).GetProperty("CreatedAt", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ModelBase).GetField("<CreatedAt>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            createdAt.AddAnnotation("Relational:DefaultValueSql", "getdate()");
            createdAt.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var date = runtimeEntityType.AddProperty(
                "Date",
                typeof(DateTime),
                propertyInfo: typeof(LunchGamingDate).GetProperty("Date", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LunchGamingDate).GetField("<Date>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            date.AddAnnotation("Relational:ColumnType", "date");
            date.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var deletedAt = runtimeEntityType.AddProperty(
                "DeletedAt",
                typeof(DateTime?),
                propertyInfo: typeof(ModelBase).GetProperty("DeletedAt", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ModelBase).GetField("<DeletedAt>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);
            deletedAt.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var gameId = runtimeEntityType.AddProperty(
                "GameId",
                typeof(Guid),
                propertyInfo: typeof(LunchGamingDate).GetProperty("GameId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LunchGamingDate).GetField("<GameId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            gameId.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var isDeleted = runtimeEntityType.AddProperty(
                "IsDeleted",
                typeof(bool),
                propertyInfo: typeof(ModelBase).GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ModelBase).GetField("<IsDeleted>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            isDeleted.AddAnnotation("Relational:DefaultValueSql", "0");
            isDeleted.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var updatedAt = runtimeEntityType.AddProperty(
                "UpdatedAt",
                typeof(DateTime),
                propertyInfo: typeof(ModelBase).GetProperty("UpdatedAt", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ModelBase).GetField("<UpdatedAt>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd);
            updatedAt.AddAnnotation("Relational:DefaultValueSql", "getdate()");
            updatedAt.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { gameId });

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("GameId") },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id") }),
                principalEntityType,
                deleteBehavior: DeleteBehavior.Cascade,
                required: true);

            var game = declaringEntityType.AddNavigation("Game",
                runtimeForeignKey,
                onDependent: true,
                typeof(LunchGame),
                propertyInfo: typeof(LunchGamingDate).GetProperty("Game", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(LunchGamingDate).GetField("<Game>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "LunchGamingDates");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
