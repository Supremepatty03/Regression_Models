using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColorRegressionApp.Migrations
{
    /// <inheritdoc />
    public partial class MakePredictionModelIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExperimentPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    PointNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    X1 = table.Column<double>(type: "REAL", nullable: false),
                    X2 = table.Column<double>(type: "REAL", nullable: false),
                    X3 = table.Column<double>(type: "REAL", nullable: false),
                    L = table.Column<double>(type: "REAL", nullable: false),
                    A = table.Column<double>(type: "REAL", nullable: false),
                    B = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetName = table.Column<string>(type: "TEXT", nullable: false),
                    B1 = table.Column<double>(type: "REAL", nullable: false),
                    B2 = table.Column<double>(type: "REAL", nullable: false),
                    B3 = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelCoefficients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Predictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelId = table.Column<int>(type: "INTEGER", nullable: true),
                    TargetL = table.Column<double>(type: "REAL", nullable: false),
                    TargetA = table.Column<double>(type: "REAL", nullable: false),
                    TargetB = table.Column<double>(type: "REAL", nullable: false),
                    ResultX1 = table.Column<double>(type: "REAL", nullable: false),
                    ResultX2 = table.Column<double>(type: "REAL", nullable: false),
                    ResultX3 = table.Column<double>(type: "REAL", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predictions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegressionModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegressionModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperimentPoints");

            migrationBuilder.DropTable(
                name: "ModelCoefficients");

            migrationBuilder.DropTable(
                name: "Predictions");

            migrationBuilder.DropTable(
                name: "RegressionModels");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
