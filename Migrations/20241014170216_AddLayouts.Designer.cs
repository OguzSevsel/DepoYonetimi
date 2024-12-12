﻿// <auto-generated />
using System;
using Balya_Yerleştirme.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Balya_Yerleştirme.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20241014170216_AddLayouts")]
    partial class AddLayouts
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Balya_Yerleştirme.Models.Ambar", b =>
                {
                    b.Property<int>("AmbarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ambar_id");

                    b.Property<float>("AmbarBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("ambar_Boyu_Metre");

                    b.Property<string>("AmbarDescription")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("ambar_description");

                    b.Property<float>("AmbarEni")
                        .HasColumnType("REAL")
                        .HasColumnName("ambar_Eni_Metre");

                    b.Property<string>("AmbarName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("ambar_name");

                    b.Property<float>("KareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Boyu");

                    b.Property<float>("KareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Eni");

                    b.Property<float>("KareX")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_X");

                    b.Property<float>("KareY")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Y");

                    b.Property<int>("LayoutId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("layout_id");

                    b.Property<float>("OriginalKareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Boyu");

                    b.Property<float>("OriginalKareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Eni");

                    b.Property<float>("OriginalKareX")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_X");

                    b.Property<float>("OriginalKareY")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Y");

                    b.Property<float>("Zoomlevel")
                        .HasColumnType("REAL")
                        .HasColumnName("zoomlevel");

                    b.HasKey("AmbarId");

                    b.ToTable("Ambar", (string)null);
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Cell", b =>
                {
                    b.Property<int>("CellId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("cell_id");

                    b.Property<float>("CellBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("cell_Boyu");

                    b.Property<float>("CellEni")
                        .HasColumnType("REAL")
                        .HasColumnName("cell_Eni");

                    b.Property<string>("CellEtiketi")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("cell_Etiketi");

                    b.Property<int>("CellMalSayisi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("cell_Mal_Sayisi");

                    b.Property<int>("CellYuksekligi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("cell_Yuksekligi");

                    b.Property<int>("Column")
                        .HasColumnType("INTEGER")
                        .HasColumnName("cell_Kolon");

                    b.Property<int>("DepoId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("depo_id");

                    b.Property<float>("DikeyKenarBoslugu")
                        .HasColumnType("REAL")
                        .HasColumnName("cell_Dikey_Kenar_Boslugu");

                    b.Property<int>("ItemSayisi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("item_Sayisi");

                    b.Property<float>("KareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Boyu");

                    b.Property<float>("KareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Eni");

                    b.Property<float>("KareX")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_X");

                    b.Property<float>("KareY")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Y");

                    b.Property<float>("NesneBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("cell_Nesne_Boyu");

                    b.Property<float>("NesneEni")
                        .HasColumnType("REAL")
                        .HasColumnName("cell_Nesne_Eni");

                    b.Property<int>("NesneYuksekligi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("cell_Nesne_Yuksekligi");

                    b.Property<float>("OriginalKareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Boyu");

                    b.Property<float>("OriginalKareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Eni");

                    b.Property<float>("OriginalKareX")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_X");

                    b.Property<float>("OriginalKareY")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Y");

                    b.Property<int>("Row")
                        .HasColumnType("INTEGER")
                        .HasColumnName("cell_Satir");

                    b.Property<float>("YatayKenarBoslugu")
                        .HasColumnType("REAL")
                        .HasColumnName("cell_Yatay_Kenar_Boslugu");

                    b.Property<float>("Zoomlevel")
                        .HasColumnType("REAL")
                        .HasColumnName("zoomlevel");

                    b.Property<float>("cell_Cm_X")
                        .HasColumnType("REAL")
                        .HasColumnName("cell_CM_X");

                    b.Property<float>("cell_Cm_Y")
                        .HasColumnType("REAL")
                        .HasColumnName("cell_CM_Y");

                    b.Property<int>("toplam_Nesne_Yuksekligi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("cell_Toplam_Nesne_Yuksekligi");

                    b.HasKey("CellId");

                    b.HasIndex("DepoId");

                    b.ToTable("Cell", (string)null);
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.PLC", b =>
                {
                    b.Property<int>("ConveyorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("conveyor_id");

                    b.Property<int>("AmbarId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ambar_id");

                    b.Property<float?>("ConveyorAraligi")
                        .HasColumnType("REAL")
                        .HasColumnName("conveyor_Araligi");

                    b.Property<float>("ConveyorBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("conveyor_Boyu");

                    b.Property<float>("ConveyorEni")
                        .HasColumnType("REAL")
                        .HasColumnName("conveyor_Eni");

                    b.Property<float>("KareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Boyu");

                    b.Property<float>("KareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Eni");

                    b.Property<float>("KareX")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_X");

                    b.Property<float>("KareY")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Y");

                    b.Property<float>("OriginalKareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Boyu");

                    b.Property<float>("OriginalKareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Eni");

                    b.Property<float>("OriginalKareX")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_X");

                    b.Property<float>("OriginalKareY")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Y");

                    b.Property<float>("Zoomlevel")
                        .HasColumnType("REAL")
                        .HasColumnName("zoomlevel");

                    b.HasKey("ConveyorId");

                    b.HasIndex("AmbarId");

                    b.ToTable("PLC", (string)null);
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.ConveyorReferencePoint", b =>
                {
                    b.Property<int>("ReferenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("reference_id");

                    b.Property<int>("AmbarId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ambar_id");

                    b.Property<int>("ConveyorId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("conveyor_id");

                    b.Property<float>("KareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Boyu");

                    b.Property<float>("KareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Eni");

                    b.Property<float>("KareX")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_X");

                    b.Property<float>("KareY")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Y");

                    b.Property<float>("OriginalKareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Boyu");

                    b.Property<float>("OriginalKareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Eni");

                    b.Property<float>("OriginalKareX")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_X");

                    b.Property<float>("OriginalKareY")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Y");

                    b.Property<float>("Pointsize")
                        .HasColumnType("REAL")
                        .HasColumnName("pointsize");

                    b.Property<float>("Zoomlevel")
                        .HasColumnType("REAL")
                        .HasColumnName("zoomlevel");

                    b.HasKey("ReferenceId");

                    b.HasIndex("ConveyorId");

                    b.ToTable("ConveyorReferencePoint", (string)null);
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Depo", b =>
                {
                    b.Property<int>("DepoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("depo_id");

                    b.Property<int>("AmbarId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ambar_id");

                    b.Property<int>("ColumnCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("kolon_Sayisi");

                    b.Property<float>("DepoAlaniBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("depo_Alani_Boyu");

                    b.Property<float>("DepoAlaniEni")
                        .HasColumnType("REAL")
                        .HasColumnName("depo_Alani_Eni");

                    b.Property<int>("DepoAlaniYuksekligi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("depo_Alani_Yuksekligi");

                    b.Property<string>("DepoDescription")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("depo_description");

                    b.Property<string>("DepoName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("depo_name");

                    b.Property<float>("Depo_Alani_Boyu_Cm")
                        .HasColumnType("REAL")
                        .HasColumnName("depo_Alani_Boyu_Cm");

                    b.Property<float>("Depo_Alani_Eni_Cm")
                        .HasColumnType("REAL")
                        .HasColumnName("depo_Alani_Eni_Cm");

                    b.Property<string>("ItemTuru")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("item_Turu");

                    b.Property<float>("KareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Boyu");

                    b.Property<float>("KareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Eni");

                    b.Property<float>("KareX")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_X");

                    b.Property<float>("KareY")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Y");

                    b.Property<float>("OriginalDepoSizeHeight")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Depo_Size_Height");

                    b.Property<float>("OriginalDepoSizeWidth")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Depo_Size_Width");

                    b.Property<float>("OriginalKareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Boyu");

                    b.Property<float>("OriginalKareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Eni");

                    b.Property<float>("OriginalKareX")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_X");

                    b.Property<float>("OriginalKareY")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Y");

                    b.Property<int>("RowCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("satir_Sayisi");

                    b.Property<int>("Yerlestirilme_Sirasi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("yerlestirilme_Sirasi");

                    b.Property<float>("Zoomlevel")
                        .HasColumnType("REAL")
                        .HasColumnName("zoomlevel");

                    b.Property<int>("asama1_ItemSayisi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("asama1_ItemSayisi");

                    b.Property<int>("asama1_Yuksekligi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("itemDrop_Stage_1");

                    b.Property<int>("asama2_ToplamItemSayisi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("asama2_ToplamItemSayisi");

                    b.Property<int>("asama2_Yuksekligi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("itemDrop_Stage_2");

                    b.Property<int>("currentColumn")
                        .HasColumnType("INTEGER")
                        .HasColumnName("current_Column");

                    b.Property<int>("currentRow")
                        .HasColumnType("INTEGER")
                        .HasColumnName("current_Row");

                    b.Property<int>("currentStage")
                        .HasColumnType("INTEGER")
                        .HasColumnName("current_Stage");

                    b.Property<string>("itemDrop_LeftRight")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("itemDrop_LeftRight");

                    b.Property<string>("itemDrop_StartLocation")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("itemDrop_StartLocation");

                    b.Property<string>("itemDrop_UpDown")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("itemDrop_UpDown");

                    b.HasKey("DepoId");

                    b.HasIndex("AmbarId");

                    b.ToTable("Depo", (string)null);
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("item_id");

                    b.Property<int>("CellId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("cell_id");

                    b.Property<float>("Cm_X_Axis")
                        .HasColumnType("REAL")
                        .HasColumnName("item_cm_X_Axis");

                    b.Property<float>("Cm_Y_Axis")
                        .HasColumnType("REAL")
                        .HasColumnName("item_cm_Y_Axis");

                    b.Property<float>("Cm_Z_Axis")
                        .HasColumnType("REAL")
                        .HasColumnName("item_cm_Z_Axis");

                    b.Property<string>("ItemAciklamasi")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("item_Aciklamasi");

                    b.Property<float>("ItemAgirligi")
                        .HasColumnType("REAL")
                        .HasColumnName("item_Agirligi");

                    b.Property<float>("ItemBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("item_Boyu");

                    b.Property<float>("ItemEni")
                        .HasColumnType("REAL")
                        .HasColumnName("item_Eni");

                    b.Property<string>("ItemEtiketi")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("item_Etiketi");

                    b.Property<string>("ItemTuru")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("item_turu");

                    b.Property<int>("ItemYuksekligi")
                        .HasColumnType("INTEGER")
                        .HasColumnName("item_Yuksekligi");

                    b.Property<float>("KareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Boyu");

                    b.Property<float>("KareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Eni");

                    b.Property<float>("KareX")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_X");

                    b.Property<float>("KareY")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Y");

                    b.Property<float>("OriginalKareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Boyu");

                    b.Property<float>("OriginalKareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Eni");

                    b.Property<float>("OriginalKareX")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_X");

                    b.Property<float>("OriginalKareY")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Y");

                    b.Property<float>("Zoomlevel")
                        .HasColumnType("REAL")
                        .HasColumnName("zoomlevel");

                    b.HasKey("ItemId");

                    b.HasIndex("CellId");

                    b.ToTable("Item", (string)null);
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.ItemReferencePoint", b =>
                {
                    b.Property<int>("ReferenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("reference_id");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("item_id");

                    b.Property<float>("KareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Boyu");

                    b.Property<float>("KareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Eni");

                    b.Property<float>("KareX")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_X");

                    b.Property<float>("KareY")
                        .HasColumnType("REAL")
                        .HasColumnName("Kare_Y");

                    b.Property<float>("OriginalKareBoyu")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Boyu");

                    b.Property<float>("OriginalKareEni")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Eni");

                    b.Property<float>("OriginalKareX")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_X");

                    b.Property<float>("OriginalKareY")
                        .HasColumnType("REAL")
                        .HasColumnName("original_Kare_Y");

                    b.Property<float>("Pointsize")
                        .HasColumnType("REAL")
                        .HasColumnName("pointsize");

                    b.Property<float>("Zoomlevel")
                        .HasColumnType("REAL")
                        .HasColumnName("zoomlevel");

                    b.HasKey("ReferenceId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemReferencePoint", (string)null);
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Layout", b =>
                {
                    b.Property<int>("LayoutId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Layout_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Layout_description");

                    b.Property<int>("LastClosedLayout")
                        .HasColumnType("INTEGER")
                        .HasColumnName("LastClosedLayout");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .HasColumnName("Layout_name");

                    b.HasKey("LayoutId");

                    b.ToTable("Layout", "Layout");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Cell", b =>
                {
                    b.HasOne("Balya_Yerleştirme.Models.Depo", "Depo")
                        .WithMany("Cells")
                        .HasForeignKey("DepoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Cell_Depo");

                    b.Navigation("Depo");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.PLC", b =>
                {
                    b.HasOne("Balya_Yerleştirme.Models.Ambar", "Ambar")
                        .WithMany("Conveyors")
                        .HasForeignKey("AmbarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Conveyor_Ambar");

                    b.Navigation("Ambar");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.ConveyorReferencePoint", b =>
                {
                    b.HasOne("Balya_Yerleştirme.Models.PLC", "PLC")
                        .WithMany("ConveyorReferencePoints")
                        .HasForeignKey("ConveyorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ConveyorReferencePoint_Conveyor");

                    b.Navigation("PLC");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Depo", b =>
                {
                    b.HasOne("Balya_Yerleştirme.Models.Ambar", "Ambar")
                        .WithMany("Depos")
                        .HasForeignKey("AmbarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Depo_Ambar");

                    b.Navigation("Ambar");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Item", b =>
                {
                    b.HasOne("Balya_Yerleştirme.Models.Cell", "Cell")
                        .WithMany("Items")
                        .HasForeignKey("CellId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Item_Cell");

                    b.Navigation("Cell");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.ItemReferencePoint", b =>
                {
                    b.HasOne("Balya_Yerleştirme.Models.Item", "Item")
                        .WithMany("ItemReferencePoints")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ItemReferencePoint_Item");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Ambar", b =>
                {
                    b.Navigation("Conveyors");

                    b.Navigation("Depos");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Cell", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.PLC", b =>
                {
                    b.Navigation("ConveyorReferencePoints");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Depo", b =>
                {
                    b.Navigation("Cells");
                });

            modelBuilder.Entity("Balya_Yerleştirme.Models.Item", b =>
                {
                    b.Navigation("ItemReferencePoints");
                });
#pragma warning restore 612, 618
        }
    }
}
