using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DATN.Models;

public partial class DeviceContext : DbContext
{
    public DeviceContext()
    {
    }

    public DeviceContext(DbContextOptions<DeviceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<DetailRegist> DetailRegists { get; set; }

    public virtual DbSet<DetailsPenaltyTicket> DetailsPenaltyTickets { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<DeviceRegistration> DeviceRegistrations { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ListDeviceRegist> ListDeviceRegists { get; set; }

    public virtual DbSet<PenaltyTicket> PenaltyTickets { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-UDBF063;Database=Device;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_PhanLoai");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Descr).HasMaxLength(20);
        });

        modelBuilder.Entity<DetailRegist>(entity =>
        {
            entity.HasKey(e => new { e.RegistId, e.DeviceId, e.ItemId });

            entity.ToTable("DetailRegist");

            entity.Property(e => e.RegistId).HasColumnName("RegistID");
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.AfterStatus).HasMaxLength(50);
            entity.Property(e => e.BeforeStatus).HasMaxLength(50);

            entity.HasOne(d => d.Item).WithMany(p => p.DetailRegists)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailRegist_Item");

            entity.HasOne(d => d.ListDeviceRegist).WithMany(p => p.DetailRegists)
                .HasForeignKey(d => new { d.RegistId, d.DeviceId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailRegist_ListDeviceRegist");
        });

        modelBuilder.Entity<DetailsPenaltyTicket>(entity =>
        {
            entity.HasKey(e => new { e.PenaltyId, e.RegistId, e.LineRef });

            entity.ToTable("DetailsPenaltyTicket");

            entity.Property(e => e.PenaltyId).HasColumnName("PenaltyID");
            entity.Property(e => e.RegistId).HasColumnName("RegistID");

            entity.HasOne(d => d.Penalty).WithMany(p => p.DetailsPenaltyTickets)
                .HasForeignKey(d => d.PenaltyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailsPenaltyTicket_PenaltyTicket1");

            entity.HasOne(d => d.Regist).WithMany(p => p.DetailsPenaltyTickets)
                .HasForeignKey(d => d.RegistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailsPenaltyTicket_DeviceRegistration");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Device");

            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Descr).HasMaxLength(255);
            entity.Property(e => e.Pdf).HasColumnName("PDF");
            entity.Property(e => e.ShortDescr).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Devices)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Device_Category");
        });

        modelBuilder.Entity<DeviceRegistration>(entity =>
        {
            entity.HasKey(e => e.RegistId);

            entity.ToTable("DeviceRegistration");

            entity.Property(e => e.RegistId).HasColumnName("RegistID");
            entity.Property(e => e.ActualBorrowDate).HasColumnType("datetime");
            entity.Property(e => e.ActualReturnDate).HasColumnType("datetime");
            entity.Property(e => e.IsFine)
                .HasDefaultValue(false)
                .HasColumnName("isFine");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.RegistDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");

            entity.HasOne(d => d.User).WithMany(p => p.DeviceRegistrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeviceRegistration_User");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.ImportDate).HasColumnType("datetime");
            entity.Property(e => e.ImporterId).HasColumnName("ImporterID");
            entity.Property(e => e.PosId).HasColumnName("PosID");
            entity.Property(e => e.Qr).HasColumnName("QR");
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.Device).WithMany(p => p.Items)
                .HasForeignKey(d => d.DeviceId)
                .HasConstraintName("FK_Item_Device");

            entity.HasOne(d => d.Importer).WithMany(p => p.Items)
                .HasForeignKey(d => d.ImporterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Item_User");

            entity.HasOne(d => d.Pos).WithMany(p => p.Items)
                .HasForeignKey(d => d.PosId)
                .HasConstraintName("FK_Item_Position");
        });

        modelBuilder.Entity<ListDeviceRegist>(entity =>
        {
            entity.HasKey(e => new { e.RegistId, e.DeviceId }).HasName("PK_DetailsRegistration");

            entity.ToTable("ListDeviceRegist");

            entity.Property(e => e.RegistId).HasColumnName("RegistID");
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

            entity.HasOne(d => d.Device).WithMany(p => p.ListDeviceRegists)
                .HasForeignKey(d => d.DeviceId)
                .HasConstraintName("FK_ListDeviceRegist_Device");

            entity.HasOne(d => d.Regist).WithMany(p => p.ListDeviceRegists)
                .HasForeignKey(d => d.RegistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetailsRegistration_DeviceRegistration");
        });

        modelBuilder.Entity<PenaltyTicket>(entity =>
        {
            entity.HasKey(e => e.PenaltyId);

            entity.ToTable("PenaltyTicket");

            entity.Property(e => e.PenaltyId).HasColumnName("PenaltyID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PosId).HasName("PK_Position_1");

            entity.ToTable("Position");

            entity.Property(e => e.PosId).HasColumnName("PosID");
            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Positions)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Position_Warehouse");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Descr).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CitizenId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CitizenID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Surname).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Roles");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK_NhaKho");

            entity.ToTable("Warehouse");

            entity.Property(e => e.WarehouseId).HasColumnName("WarehouseID");
            entity.Property(e => e.WarehouseDescr).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
