using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitaOrbitApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FullName = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Gender = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    UserDescription = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    CurrentLocation = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    EmergencyContact = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Emergencies",
                columns: table => new
                {
                    EmergencyId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Location = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emergencies", x => x.EmergencyId);
                    table.ForeignKey(
                        name: "FK_Emergencies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentalConditions",
                columns: table => new
                {
                    EnvironmentalConditionId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ExternalTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Humidity = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Altitude = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    AtmosphericPressure = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    AirQuality = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    RadiationLevel = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    EnvironmentType = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentalConditions", x => x.EnvironmentalConditionId);
                    table.ForeignKey(
                        name: "FK_EnvironmentalConditions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthRecords",
                columns: table => new
                {
                    HealthRecordId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    HeartRate = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SystolicPressure = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DiastolicPressure = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    BodyTemperature = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OxygenSaturation = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    HydrationLevel = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    SleepHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Notes = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    RiskClassification = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthRecords", x => x.HealthRecordId);
                    table.ForeignKey(
                        name: "FK_HealthRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SymptomRecords",
                columns: table => new
                {
                    SymptomRecordId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SymptomName = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Intensity = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Frequency = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    RiskClassification = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomRecords", x => x.SymptomRecordId);
                    table.ForeignKey(
                        name: "FK_SymptomRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    AlertId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    HealthRecordId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SymptomRecordId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TypeAlert = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    RiskLevel = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_Alerts_HealthRecords_HealthRecordId",
                        column: x => x.HealthRecordId,
                        principalTable: "HealthRecords",
                        principalColumn: "HealthRecordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alerts_SymptomRecords_SymptomRecordId",
                        column: x => x.SymptomRecordId,
                        principalTable: "SymptomRecords",
                        principalColumn: "SymptomRecordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alerts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_HealthRecordId",
                table: "Alerts",
                column: "HealthRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_SymptomRecordId",
                table: "Alerts",
                column: "SymptomRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_UserId",
                table: "Alerts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Emergencies_UserId",
                table: "Emergencies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentalConditions_UserId",
                table: "EnvironmentalConditions",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_UserId",
                table: "HealthRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomRecords_UserId",
                table: "SymptomRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "Emergencies");

            migrationBuilder.DropTable(
                name: "EnvironmentalConditions");

            migrationBuilder.DropTable(
                name: "HealthRecords");

            migrationBuilder.DropTable(
                name: "SymptomRecords");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
