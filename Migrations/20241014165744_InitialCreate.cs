using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balya_Yerleştirme.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Layout");

            migrationBuilder.CreateTable(
                name: "Ambar",
                columns: table => new
                {
                    ambar_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    layout_id = table.Column<int>(type: "INTEGER", nullable: false),
                    ambar_name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ambar_description = table.Column<string>(type: "TEXT", nullable: false),
                    Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    zoomlevel = table.Column<float>(type: "REAL", nullable: false),
                    ambar_Eni_Metre = table.Column<float>(type: "REAL", nullable: false),
                    ambar_Boyu_Metre = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambar", x => x.ambar_id);
                });

            migrationBuilder.CreateTable(
                name: "Layout",
                schema: "Layout",
                columns: table => new
                {
                    Layout_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Layout_name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Layout_description = table.Column<string>(type: "TEXT", nullable: false),
                    LastClosedLayout = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layout", x => x.Layout_id);
                });

            migrationBuilder.CreateTable(
                name: "Conveyor",
                columns: table => new
                {
                    conveyor_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ambar_id = table.Column<int>(type: "INTEGER", nullable: false),
                    conveyor_Araligi = table.Column<float>(type: "REAL", nullable: true),
                    Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    zoomlevel = table.Column<float>(type: "REAL", nullable: false),
                    conveyor_Eni = table.Column<float>(type: "REAL", nullable: false),
                    conveyor_Boyu = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conveyor", x => x.conveyor_id);
                    table.ForeignKey(
                        name: "FK_Conveyor_Ambar",
                        column: x => x.ambar_id,
                        principalTable: "Ambar",
                        principalColumn: "ambar_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Depo",
                columns: table => new
                {
                    depo_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ambar_id = table.Column<int>(type: "INTEGER", nullable: false),
                    depo_name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    depo_description = table.Column<string>(type: "TEXT", nullable: false),
                    depo_Alani_Eni = table.Column<float>(type: "REAL", nullable: false),
                    depo_Alani_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    depo_Alani_Yuksekligi = table.Column<int>(type: "INTEGER", nullable: false),
                    original_Depo_Size_Width = table.Column<float>(type: "REAL", nullable: false),
                    original_Depo_Size_Height = table.Column<float>(type: "REAL", nullable: false),
                    Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    zoomlevel = table.Column<float>(type: "REAL", nullable: false),
                    itemDrop_StartLocation = table.Column<string>(type: "TEXT", nullable: false),
                    itemDrop_UpDown = table.Column<string>(type: "TEXT", nullable: false),
                    itemDrop_LeftRight = table.Column<string>(type: "TEXT", nullable: false),
                    itemDrop_Stage_1 = table.Column<int>(type: "INTEGER", nullable: false),
                    itemDrop_Stage_2 = table.Column<int>(type: "INTEGER", nullable: false),
                    yerlestirilme_Sirasi = table.Column<int>(type: "INTEGER", nullable: false),
                    depo_Alani_Eni_Cm = table.Column<float>(type: "REAL", nullable: false),
                    depo_Alani_Boyu_Cm = table.Column<float>(type: "REAL", nullable: false),
                    satir_Sayisi = table.Column<int>(type: "INTEGER", nullable: false),
                    kolon_Sayisi = table.Column<int>(type: "INTEGER", nullable: false),
                    current_Column = table.Column<int>(type: "INTEGER", nullable: false),
                    current_Row = table.Column<int>(type: "INTEGER", nullable: false),
                    item_Turu = table.Column<string>(type: "TEXT", nullable: false),
                    asama1_ItemSayisi = table.Column<int>(type: "INTEGER", nullable: false),
                    asama2_ToplamItemSayisi = table.Column<int>(type: "INTEGER", nullable: false),
                    current_Stage = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depo", x => x.depo_id);
                    table.ForeignKey(
                        name: "FK_Depo_Ambar",
                        column: x => x.ambar_id,
                        principalTable: "Ambar",
                        principalColumn: "ambar_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConveyorReferencePoint",
                columns: table => new
                {
                    reference_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    conveyor_id = table.Column<int>(type: "INTEGER", nullable: false),
                    ambar_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    zoomlevel = table.Column<float>(type: "REAL", nullable: false),
                    pointsize = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConveyorReferencePoint", x => x.reference_id);
                    table.ForeignKey(
                        name: "FK_ConveyorReferencePoint_Conveyor",
                        column: x => x.conveyor_id,
                        principalTable: "Conveyor",
                        principalColumn: "conveyor_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cell",
                columns: table => new
                {
                    cell_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    depo_id = table.Column<int>(type: "INTEGER", nullable: false),
                    cell_Etiketi = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    cell_Eni = table.Column<float>(type: "REAL", nullable: false),
                    cell_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    cell_Yuksekligi = table.Column<int>(type: "INTEGER", nullable: false),
                    cell_Mal_Sayisi = table.Column<int>(type: "INTEGER", nullable: false),
                    Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    zoomlevel = table.Column<float>(type: "REAL", nullable: false),
                    item_Sayisi = table.Column<int>(type: "INTEGER", nullable: false),
                    cell_Dikey_Kenar_Boslugu = table.Column<float>(type: "REAL", nullable: false),
                    cell_Yatay_Kenar_Boslugu = table.Column<float>(type: "REAL", nullable: false),
                    cell_Nesne_Eni = table.Column<float>(type: "REAL", nullable: false),
                    cell_Nesne_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    cell_Nesne_Yuksekligi = table.Column<int>(type: "INTEGER", nullable: false),
                    cell_Kolon = table.Column<int>(type: "INTEGER", nullable: false),
                    cell_Satir = table.Column<int>(type: "INTEGER", nullable: false),
                    cell_Toplam_Nesne_Yuksekligi = table.Column<int>(type: "INTEGER", nullable: false),
                    cell_CM_X = table.Column<float>(type: "REAL", nullable: false),
                    cell_CM_Y = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cell", x => x.cell_id);
                    table.ForeignKey(
                        name: "FK_Cell_Depo",
                        column: x => x.depo_id,
                        principalTable: "Depo",
                        principalColumn: "depo_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    item_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    cell_id = table.Column<int>(type: "INTEGER", nullable: false),
                    item_Etiketi = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    item_turu = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    item_Eni = table.Column<float>(type: "REAL", nullable: false),
                    item_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    item_Yuksekligi = table.Column<int>(type: "INTEGER", nullable: false),
                    Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    zoomlevel = table.Column<float>(type: "REAL", nullable: false),
                    item_Agirligi = table.Column<float>(type: "REAL", nullable: false),
                    item_Aciklamasi = table.Column<string>(type: "TEXT", nullable: false),
                    item_cm_X_Axis = table.Column<float>(type: "REAL", nullable: false),
                    item_cm_Y_Axis = table.Column<float>(type: "REAL", nullable: false),
                    item_cm_Z_Axis = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.item_id);
                    table.ForeignKey(
                        name: "FK_Item_Cell",
                        column: x => x.cell_id,
                        principalTable: "Cell",
                        principalColumn: "cell_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemReferencePoint",
                columns: table => new
                {
                    reference_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    item_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_X = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Y = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Eni = table.Column<float>(type: "REAL", nullable: false),
                    original_Kare_Boyu = table.Column<float>(type: "REAL", nullable: false),
                    zoomlevel = table.Column<float>(type: "REAL", nullable: false),
                    pointsize = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemReferencePoint", x => x.reference_id);
                    table.ForeignKey(
                        name: "FK_ItemReferencePoint_Item",
                        column: x => x.item_id,
                        principalTable: "Item",
                        principalColumn: "item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cell_depo_id",
                table: "Cell",
                column: "depo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Conveyor_ambar_id",
                table: "Conveyor",
                column: "ambar_id");

            migrationBuilder.CreateIndex(
                name: "IX_ConveyorReferencePoint_conveyor_id",
                table: "ConveyorReferencePoint",
                column: "conveyor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Depo_ambar_id",
                table: "Depo",
                column: "ambar_id");

            migrationBuilder.CreateIndex(
                name: "IX_Item_cell_id",
                table: "Item",
                column: "cell_id");

            migrationBuilder.CreateIndex(
                name: "IX_ItemReferencePoint_item_id",
                table: "ItemReferencePoint",
                column: "item_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConveyorReferencePoint");

            migrationBuilder.DropTable(
                name: "ItemReferencePoint");

            migrationBuilder.DropTable(
                name: "Layout",
                schema: "Layout");

            migrationBuilder.DropTable(
                name: "Conveyor");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Cell");

            migrationBuilder.DropTable(
                name: "Depo");

            migrationBuilder.DropTable(
                name: "Ambar");
        }
    }
}
