using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitaOrbitApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiastolicPressure",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "SystolicPressure",
                table: "HealthRecords");

            migrationBuilder.AlterColumn<decimal>(
                name: "Intensity",
                table: "SymptomRecords",
                type: "DECIMAL(18, 2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AddColumn<string>(
                name: "BloodPressure",
                table: "HealthRecords",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mood",
                table: "HealthRecords",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodPressure",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "Mood",
                table: "HealthRecords");

            migrationBuilder.AlterColumn<int>(
                name: "Intensity",
                table: "SymptomRecords",
                type: "NUMBER(10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18, 2)");

            migrationBuilder.AddColumn<int>(
                name: "DiastolicPressure",
                table: "HealthRecords",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SystolicPressure",
                table: "HealthRecords",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);
        }
    }
}
