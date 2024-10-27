using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Models;

public partial class PRN221_Warehouse : DbContext
{
    public PRN221_Warehouse()
    {
    }

    public PRN221_Warehouse(DbContextOptions<PRN221_Warehouse> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Lot> Lots { get; set; }

    public virtual DbSet<LotDetail> LotDetails { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<StockOut> StockOuts { get; set; }

    public virtual DbSet<StockOutDetail> StockOutDetails { get; set; }

    public virtual DbSet<StorageArea> StorageAreas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=LAPTOP-RL07OIIS\\ASUS;Initial Catalog=WarehousePRN221;User ID=qe170139;Password=dtmghsk29903;Trusted_Connection=True;Trust Server Certificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            modelBuilder.Entity<Product>()
            .Property(p => p.Quantity)
            .HasDefaultValue(0);
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A6A5CE4174");

            entity.ToTable("Account");

            entity.Property(e => e.AccountCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B9EBCB217");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Lot>(entity =>
        {
            entity.HasKey(e => e.LotId).HasName("PK__Lot__4160EFADD583275B");

            entity.ToTable("Lot");

            entity.Property(e => e.DateIn).HasColumnType("date");
            entity.Property(e => e.LotCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasColumnType("text");

            entity.HasOne(d => d.Account).WithMany(p => p.Lots)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lot__AccountId__3F466844");

            entity.HasOne(d => d.Partner).WithMany(p => p.Lots)
                .HasForeignKey(d => d.PartnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lot__PartnerId__403A8C7D");
        });

        modelBuilder.Entity<LotDetail>(entity =>
        {
            entity.HasKey(e => e.LotDetailId).HasName("PK__LotDetai__80020F86396A33B0");

            entity.ToTable("LotDetail");

            entity.HasOne(d => d.Lot).WithMany(p => p.LotDetails)
                .HasForeignKey(d => d.LotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LotDetail__LotId__4E88ABD4");

            entity.HasOne(d => d.Partner).WithMany(p => p.LotDetails)
                .HasForeignKey(d => d.PartnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LotDetail__Partn__5070F446");

            entity.HasOne(d => d.Product).WithMany(p => p.LotDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LotDetail__Produ__4F7CD00D");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.PartnerId).HasName("PK__Partner__39FD63120E99DAFC");

            entity.ToTable("Partner");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PartnerCode)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6CDAD1BA680");

            entity.ToTable("Product");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ProductCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.Products)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__AreaId__47DBAE45");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__46E78A0C");
        });

        modelBuilder.Entity<StockOut>(entity =>
        {
            entity.HasKey(e => e.StockOutId).HasName("PK__StockOut__C5308D7A9C6EF5F8");

            entity.ToTable("StockOut");

            entity.Property(e => e.DateOut).HasColumnType("date");
            entity.Property(e => e.Note).HasColumnType("text");

            entity.HasOne(d => d.Account).WithMany(p => p.StockOuts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockOut__Accoun__4316F928");

            entity.HasOne(d => d.Partner).WithMany(p => p.StockOuts)
                .HasForeignKey(d => d.PartnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockOut__Partne__440B1D61");
        });

        modelBuilder.Entity<StockOutDetail>(entity =>
        {
            entity.HasKey(e => e.StockOutDetailId).HasName("PK__StockOut__EB248E9F2DF3BED5");

            entity.ToTable("StockOutDetail");

            entity.HasOne(d => d.Product).WithMany(p => p.StockOutDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockOutD__Produ__4AB81AF0");

            entity.HasOne(d => d.StockOut).WithMany(p => p.StockOutDetails)
                .HasForeignKey(d => d.StockOutId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockOutD__Stock__4BAC3F29");
        });

        modelBuilder.Entity<StorageArea>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__StorageA__70B820480A5581BF");

            entity.ToTable("StorageArea");

            entity.Property(e => e.AreaCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AreaName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
