using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Balya_Yerleştirme.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Layout> Layout { get; set; }

    public virtual DbSet<Isletme> Isletme { get; set; }

    public virtual DbSet<Ambar> Ambars { get; set; }

    public virtual DbSet<Cell> Cells { get; set; }

    public virtual DbSet<Conveyor> Conveyors { get; set; }

    public virtual DbSet<ConveyorReferencePoint> ConveyorReferencePoints { get; set; }

    public virtual DbSet<Depo> Depos { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemReferencePoint> ItemReferencePoints { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    => optionsBuilder.UseSqlite("Data Source = depo.db");
    //=> optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Mal_Yerleştirme;TrustServerCertificate=True;Trusted_Connection=True;");
    //=> optionsBuilder.UseSqlServer("Server=192.168.1.103,1433;Database=Mal_Yerleştirme;TrustServerCertificate=True;User id=oguz Desktop;Password=20dollars*");
    //=> optionsBuilder.UseSqlServer("Server=192.168.1.253,1433;Database=Mal_Yerleştirme;TrustServerCertificate=True;User id=oguz;Password=20dollars*");
    //=> optionsBuilder.UseSqlServer("Server=192.168.0.102,1433;Database=Mal_Yerleştirme;TrustServerCertificate=True;User id=oguz;Password=20dollars*");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Layout>(entity =>
        {
            entity.ToTable("Layout");

            entity.Property(e => e.LayoutId).HasColumnName("Layout_id");
            entity.Property(e => e.IsletmeID).HasColumnName("Isletme_id");
            entity.Property(e => e.Description).HasColumnName("Layout_description");
            entity.Property(e => e.LastClosedLayout).HasColumnName("LastClosedLayout");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("Layout_name");
        });

        modelBuilder.Entity<Isletme>(entity =>
        {
            entity.ToTable("Isletme");

            entity.Property(e => e.IsletmeID).HasColumnName("Isletme_id");
            entity.Property(e => e.Description).HasColumnName("Isletme_description");
            entity.Property(e => e.LastClosedIsletme).HasColumnName("LastClosedIsletme");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("Isletme_name");
        });

        modelBuilder.Entity<Ambar>(entity =>
        {
            entity.ToTable("Ambar");

            entity.Property(e => e.AmbarId).HasColumnName("ambar_id");
            entity.Property(e => e.LayoutId).HasColumnName("layout_id");
            entity.Property(e => e.AmbarDescription).HasColumnName("ambar_description");
            entity.Property(e => e.AmbarName)
                .HasMaxLength(50)
                .HasColumnName("ambar_name");
            entity.Property(e => e.KareBoyu).HasColumnName("Kare_Boyu");
            entity.Property(e => e.KareEni).HasColumnName("Kare_Eni");
            entity.Property(e => e.KareX).HasColumnName("Kare_X");
            entity.Property(e => e.KareY).HasColumnName("Kare_Y");
            entity.Property(e => e.OriginalKareBoyu).HasColumnName("original_Kare_Boyu");
            entity.Property(e => e.OriginalKareEni).HasColumnName("original_Kare_Eni");
            entity.Property(e => e.OriginalKareX).HasColumnName("original_Kare_X");
            entity.Property(e => e.OriginalKareY).HasColumnName("original_Kare_Y");
            entity.Property(e => e.Zoomlevel).HasColumnName("zoomlevel");
            entity.Property(e => e.AmbarEni).HasColumnName("ambar_Eni_Metre");
            entity.Property(e => e.AmbarBoyu).HasColumnName("ambar_Boyu_Metre");
        });
       

        modelBuilder.Entity<Cell>(entity =>
        {
            entity.ToTable("Cell");

            entity.Property(e => e.CellId).HasColumnName("cell_id");
            entity.Property(e => e.CellBoyu).HasColumnName("cell_Boyu");
            entity.Property(e => e.CellEni).HasColumnName("cell_Eni");
            entity.Property(e => e.CellEtiketi)
                .HasMaxLength(50)
                .HasColumnName("cell_Etiketi");
            entity.Property(e => e.CellMalSayisi).HasColumnName("cell_Mal_Sayisi");
            entity.Property(e => e.CellYuksekligi).HasColumnName("cell_Yuksekligi");
            entity.Property(e => e.DepoId).HasColumnName("depo_id");
            entity.Property(e => e.KareBoyu).HasColumnName("Kare_Boyu");
            entity.Property(e => e.KareEni).HasColumnName("Kare_Eni");
            entity.Property(e => e.KareX).HasColumnName("Kare_X");
            entity.Property(e => e.KareY).HasColumnName("Kare_Y");
            entity.Property(e => e.OriginalKareBoyu).HasColumnName("original_Kare_Boyu");
            entity.Property(e => e.OriginalKareEni).HasColumnName("original_Kare_Eni");
            entity.Property(e => e.OriginalKareX).HasColumnName("original_Kare_X");
            entity.Property(e => e.OriginalKareY).HasColumnName("original_Kare_Y");
            entity.Property(e => e.Zoomlevel).HasColumnName("zoomlevel");
            entity.Property(e => e.ItemSayisi).HasColumnName("item_Sayisi");
            entity.Property(e => e.DikeyKenarBoslugu).HasColumnName("cell_Dikey_Kenar_Boslugu");
            entity.Property(e => e.YatayKenarBoslugu).HasColumnName("cell_Yatay_Kenar_Boslugu");
            entity.Property(e => e.NesneEni).HasColumnName("cell_Nesne_Eni");
            entity.Property(e => e.NesneBoyu).HasColumnName("cell_Nesne_Boyu");
            entity.Property(e => e.NesneYuksekligi).HasColumnName("cell_Nesne_Yuksekligi");
            entity.Property(e => e.Column).HasColumnName("cell_Kolon");
            entity.Property(e => e.Row).HasColumnName("cell_Satir");
            entity.Property(e => e.toplam_Nesne_Yuksekligi).HasColumnName("cell_Toplam_Nesne_Yuksekligi");
            entity.Property(e => e.cell_Cm_X).HasColumnName("cell_CM_X");
            entity.Property(e => e.cell_Cm_Y).HasColumnName("cell_CM_Y");




            entity.HasOne(d => d.Depo).WithMany(p => p.Cells)
                .HasForeignKey(d => d.DepoId)
                .HasConstraintName("FK_Cell_Depo");
        });

        modelBuilder.Entity<Conveyor>(entity =>
        {
            entity.ToTable("Conveyor");

            entity.Property(e => e.ConveyorId).HasColumnName("conveyor_id");
            entity.Property(e => e.AmbarId).HasColumnName("ambar_id");
            entity.Property(e => e.ConveyorAraligi).HasColumnName("conveyor_Araligi");
            entity.Property(e => e.KareBoyu).HasColumnName("Kare_Boyu");
            entity.Property(e => e.KareEni).HasColumnName("Kare_Eni");
            entity.Property(e => e.KareX).HasColumnName("Kare_X");
            entity.Property(e => e.KareY).HasColumnName("Kare_Y");
            entity.Property(e => e.OriginalKareBoyu).HasColumnName("original_Kare_Boyu");
            entity.Property(e => e.OriginalKareEni).HasColumnName("original_Kare_Eni");
            entity.Property(e => e.OriginalKareX).HasColumnName("original_Kare_X");
            entity.Property(e => e.OriginalKareY).HasColumnName("original_Kare_Y");
            entity.Property(e => e.Zoomlevel).HasColumnName("zoomlevel");
            entity.Property(e => e.ConveyorEni).HasColumnName("conveyor_Eni");
            entity.Property(e => e.ConveyorBoyu).HasColumnName("conveyor_Boyu");

            entity.HasOne(d => d.Ambar).WithMany(p => p.Conveyors)
                .HasForeignKey(d => d.AmbarId)
                .HasConstraintName("FK_Conveyor_Ambar");
        });

        modelBuilder.Entity<ConveyorReferencePoint>(entity =>
        {
            entity.HasKey(e => e.ReferenceId);

            entity.ToTable("ConveyorReferencePoint");

            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.AmbarId).HasColumnName("ambar_id");
            entity.Property(e => e.ConveyorId).HasColumnName("conveyor_id");
            entity.Property(e => e.KareBoyu).HasColumnName("Kare_Boyu");
            entity.Property(e => e.KareEni).HasColumnName("Kare_Eni");
            entity.Property(e => e.KareX).HasColumnName("Kare_X");
            entity.Property(e => e.KareY).HasColumnName("Kare_Y");
            entity.Property(e => e.OriginalKareBoyu).HasColumnName("original_Kare_Boyu");
            entity.Property(e => e.OriginalKareEni).HasColumnName("original_Kare_Eni");
            entity.Property(e => e.OriginalKareX).HasColumnName("original_Kare_X");
            entity.Property(e => e.OriginalKareY).HasColumnName("original_Kare_Y");
            entity.Property(e => e.Pointsize).HasColumnName("pointsize");
            entity.Property(e => e.Zoomlevel).HasColumnName("zoomlevel");
            entity.Property(e => e.OriginalLocationInsideParentX).HasColumnName("originalLocationInsideParentX");
            entity.Property(e => e.OriginalLocationInsideParentY).HasColumnName("originalLocationInsideParentY");



            entity.HasOne(d => d.Conveyor).WithMany(p => p.ConveyorReferencePoints)
                .HasForeignKey(d => d.ConveyorId)
                .HasConstraintName("FK_ConveyorReferencePoint_Conveyor");
        });

        modelBuilder.Entity<Depo>(entity =>
        {
            entity.ToTable("Depo");

            entity.Property(e => e.DepoId).HasColumnName("depo_id");
            entity.Property(e => e.AmbarId).HasColumnName("ambar_id");
            entity.Property(e => e.DepoAlaniBoyu).HasColumnName("depo_Alani_Boyu");
            entity.Property(e => e.DepoAlaniEni).HasColumnName("depo_Alani_Eni");
            entity.Property(e => e.DepoAlaniYuksekligi).HasColumnName("depo_Alani_Yuksekligi");
            entity.Property(e => e.DepoDescription).HasColumnName("depo_description");
            entity.Property(e => e.DepoName)
                .HasMaxLength(50)
                .HasColumnName("depo_name");
            entity.Property(e => e.KareBoyu).HasColumnName("Kare_Boyu");
            entity.Property(e => e.KareEni).HasColumnName("Kare_Eni");
            entity.Property(e => e.KareX).HasColumnName("Kare_X");
            entity.Property(e => e.KareY).HasColumnName("Kare_Y");
            entity.Property(e => e.OriginalDepoSizeHeight).HasColumnName("original_Depo_Size_Height");
            entity.Property(e => e.OriginalDepoSizeWidth).HasColumnName("original_Depo_Size_Width");
            entity.Property(e => e.OriginalKareBoyu).HasColumnName("original_Kare_Boyu");
            entity.Property(e => e.OriginalKareEni).HasColumnName("original_Kare_Eni");
            entity.Property(e => e.OriginalKareX).HasColumnName("original_Kare_X");
            entity.Property(e => e.OriginalKareY).HasColumnName("original_Kare_Y");
            entity.Property(e => e.Zoomlevel).HasColumnName("zoomlevel");
            entity.Property(e => e.itemDrop_StartLocation).HasColumnName("itemDrop_StartLocation");
            entity.Property(e => e.itemDrop_UpDown).HasColumnName("itemDrop_UpDown");
            entity.Property(e => e.itemDrop_LeftRight).HasColumnName("itemDrop_LeftRight");
            entity.Property(e => e.asama1_Yuksekligi).HasColumnName("itemDrop_Stage_1");
            entity.Property(e => e.asama2_Yuksekligi).HasColumnName("itemDrop_Stage_2");
            entity.Property(e => e.Yerlestirilme_Sirasi).HasColumnName("yerlestirilme_Sirasi");
            entity.Property(e => e.Depo_Alani_Eni_Cm).HasColumnName("depo_Alani_Eni_Cm");
            entity.Property(e => e.Depo_Alani_Boyu_Cm).HasColumnName("depo_Alani_Boyu_Cm");
            entity.Property(e => e.ColumnCount).HasColumnName("kolon_Sayisi");
            entity.Property(e => e.RowCount).HasColumnName("satir_Sayisi");
            entity.Property(e => e.currentColumn).HasColumnName("current_Column");
            entity.Property(e => e.currentRow).HasColumnName("current_Row");
            entity.Property(e => e.ItemTuru).HasColumnName("item_Turu");
            entity.Property(e => e.asama1_ItemSayisi).HasColumnName("asama1_ItemSayisi");
            entity.Property(e => e.asama2_ToplamItemSayisi).HasColumnName("asama2_ToplamItemSayisi");
            entity.Property(e => e.currentStage).HasColumnName("current_Stage");
            entity.Property(e => e.ItemTuruSecondary).HasColumnName("item_Turu_Secondary");

            entity.HasOne(d => d.Ambar).WithMany(p => p.Depos)
                .HasForeignKey(d => d.AmbarId)
                .HasConstraintName("FK_Depo_Ambar");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.CellId).HasColumnName("cell_id");
            entity.Property(e => e.ItemBoyu).HasColumnName("item_Boyu");
            entity.Property(e => e.ItemEni).HasColumnName("item_Eni");
            entity.Property(e => e.ItemAgirligi).HasColumnName("item_Agirligi");
            entity.Property(e => e.ItemEtiketi)
                .HasMaxLength(50)
                .HasColumnName("item_Etiketi");
            entity.Property(e => e.ItemTuru)
                .HasMaxLength(50)
                .HasColumnName("item_turu");
            entity.Property(e => e.ItemYuksekligi).HasColumnName("item_Yuksekligi");
            entity.Property(e => e.KareBoyu).HasColumnName("Kare_Boyu");
            entity.Property(e => e.KareEni).HasColumnName("Kare_Eni");
            entity.Property(e => e.KareX).HasColumnName("Kare_X");
            entity.Property(e => e.KareY).HasColumnName("Kare_Y");
            entity.Property(e => e.OriginalKareBoyu).HasColumnName("original_Kare_Boyu");
            entity.Property(e => e.OriginalKareEni).HasColumnName("original_Kare_Eni");
            entity.Property(e => e.OriginalKareX).HasColumnName("original_Kare_X");
            entity.Property(e => e.OriginalKareY).HasColumnName("original_Kare_Y");
            entity.Property(e => e.Zoomlevel).HasColumnName("zoomlevel");
            entity.Property(e => e.ItemAciklamasi).HasColumnName("item_Aciklamasi");
            entity.Property(e => e.Cm_X_Axis).HasColumnName("item_cm_X_Axis");
            entity.Property(e => e.Cm_Y_Axis).HasColumnName("item_cm_Y_Axis");
            entity.Property(e => e.Cm_Z_Axis).HasColumnName("item_cm_Z_Axis");

            entity.HasOne(d => d.Cell).WithMany(p => p.Items)
                .HasForeignKey(d => d.CellId)
                .HasConstraintName("FK_Item_Cell");
        });

        modelBuilder.Entity<ItemReferencePoint>(entity =>
        {
            entity.HasKey(e => e.ReferenceId);

            entity.ToTable("ItemReferencePoint");

            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.KareBoyu).HasColumnName("Kare_Boyu");
            entity.Property(e => e.KareEni).HasColumnName("Kare_Eni");
            entity.Property(e => e.KareX).HasColumnName("Kare_X");
            entity.Property(e => e.KareY).HasColumnName("Kare_Y");
            entity.Property(e => e.OriginalKareBoyu).HasColumnName("original_Kare_Boyu");
            entity.Property(e => e.OriginalKareEni).HasColumnName("original_Kare_Eni");
            entity.Property(e => e.OriginalKareX).HasColumnName("original_Kare_X");
            entity.Property(e => e.OriginalKareY).HasColumnName("original_Kare_Y");
            entity.Property(e => e.Pointsize).HasColumnName("pointsize");
            entity.Property(e => e.Zoomlevel).HasColumnName("zoomlevel");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemReferencePoints)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ItemReferencePoint_Item");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
