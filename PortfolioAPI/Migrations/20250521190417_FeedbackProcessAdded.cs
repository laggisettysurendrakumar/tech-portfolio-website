using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioAPI.Migrations
{
    /// <inheritdoc />
    public partial class FeedbackProcessAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommunicationRating = table.Column<int>(type: "int", nullable: false),
                    CollaborationRating = table.Column<int>(type: "int", nullable: false),
                    TechnicalSkillRating = table.Column<int>(type: "int", nullable: false),
                    CodeQualityRating = table.Column<int>(type: "int", nullable: false),
                    HelpfulnessRating = table.Column<int>(type: "int", nullable: false),
                    WhatWentWell = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatCouldBeImproved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FarewellNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YourName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");
        }
    }
}
