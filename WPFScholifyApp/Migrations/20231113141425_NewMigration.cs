// <copyright file="20231113141425_NewMigration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace WPFScholifyApp.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Schedules_ScheduleId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_DayOfWeeks_Schedules_ScheduleId",
                table: "DayOfWeeks");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonTimes_Schedules_ScheduleId",
                table: "LessonTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Schedules_ScheduleId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_ScheduleId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_LessonTimes_ScheduleId",
                table: "LessonTimes");

            migrationBuilder.DropIndex(
                name: "IX_DayOfWeeks_ScheduleId",
                table: "DayOfWeeks");

            migrationBuilder.DropIndex(
                name: "IX_Classes_ScheduleId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "LessonTimes");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "DayOfWeeks");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Schedules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeekId",
                table: "Schedules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonTimeId",
                table: "Schedules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Schedules",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ClassId",
                table: "Schedules",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DayOfWeekId",
                table: "Schedules",
                column: "DayOfWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_LessonTimeId",
                table: "Schedules",
                column: "LessonTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_SubjectId",
                table: "Schedules",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Classes_ClassId",
                table: "Schedules",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_DayOfWeeks_DayOfWeekId",
                table: "Schedules",
                column: "DayOfWeekId",
                principalTable: "DayOfWeeks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_LessonTimes_LessonTimeId",
                table: "Schedules",
                column: "LessonTimeId",
                principalTable: "LessonTimes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Subjects_SubjectId",
                table: "Schedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Classes_ClassId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_DayOfWeeks_DayOfWeekId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_LessonTimes_LessonTimeId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Subjects_SubjectId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_ClassId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_DayOfWeekId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_LessonTimeId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_SubjectId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "DayOfWeekId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "LessonTimeId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Schedules");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Subjects",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "LessonTimes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "DayOfWeeks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Classes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ScheduleId",
                table: "Subjects",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTimes_ScheduleId",
                table: "LessonTimes",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_DayOfWeeks_ScheduleId",
                table: "DayOfWeeks",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ScheduleId",
                table: "Classes",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Schedules_ScheduleId",
                table: "Classes",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DayOfWeeks_Schedules_ScheduleId",
                table: "DayOfWeeks",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonTimes_Schedules_ScheduleId",
                table: "LessonTimes",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Schedules_ScheduleId",
                table: "Subjects",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");
        }
    }
}
