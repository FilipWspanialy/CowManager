using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CowManagerApp.MVC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disease",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false),
                    Comment = table.Column<string>(type: "nchar(200)", fixedLength: true, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disease", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Herd",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nchar(200)", fixedLength: true, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Herd", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Medicine",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false),
                    Comment = table.Column<string>(type: "nchar(200)", fixedLength: true, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicine", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cow",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: true),
                    IDHerd = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nchar(200)", fixedLength: true, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cow", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cow_Herd",
                        column: x => x.IDHerd,
                        principalTable: "Herd",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Diagnosis",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDCow = table.Column<int>(type: "int", nullable: false),
                    IDDisease = table.Column<int>(type: "int", nullable: false),
                    NameOfDisease = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false),
                    Comment = table.Column<string>(type: "nchar(200)", fixedLength: true, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosis", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Diagnosis_Cow",
                        column: x => x.IDCow,
                        principalTable: "Cow",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Diagnosis_Disease",
                        column: x => x.IDDisease,
                        principalTable: "Disease",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Treatment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDCow = table.Column<int>(type: "int", nullable: false),
                    IDMedicine = table.Column<int>(type: "int", nullable: false),
                    NameOfMedicine = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false),
                    IDDiagnosis = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nchar(200)", fixedLength: true, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Treatment_Cow",
                        column: x => x.IDCow,
                        principalTable: "Cow",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Treatment_Diagnosis",
                        column: x => x.IDDiagnosis,
                        principalTable: "Diagnosis",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Treatment_Medicine",
                        column: x => x.IDMedicine,
                        principalTable: "Medicine",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cow_IDHerd",
                table: "Cow",
                column: "IDHerd");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosis_IDCow",
                table: "Diagnosis",
                column: "IDCow");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosis_IDDisease",
                table: "Diagnosis",
                column: "IDDisease");

            migrationBuilder.CreateIndex(
                name: "IX_Treatment_IDCow",
                table: "Treatment",
                column: "IDCow");

            migrationBuilder.CreateIndex(
                name: "IX_Treatment_IDDiagnosis",
                table: "Treatment",
                column: "IDDiagnosis");

            migrationBuilder.CreateIndex(
                name: "IX_Treatment_IDMedicine",
                table: "Treatment",
                column: "IDMedicine");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Treatment");

            migrationBuilder.DropTable(
                name: "Diagnosis");

            migrationBuilder.DropTable(
                name: "Medicine");

            migrationBuilder.DropTable(
                name: "Cow");

            migrationBuilder.DropTable(
                name: "Disease");

            migrationBuilder.DropTable(
                name: "Herd");
        }
    }
}
