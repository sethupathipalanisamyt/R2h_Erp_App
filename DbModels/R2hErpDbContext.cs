using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace R2h_Erp_App.DbModels;

public partial class R2hErpDbContext : DbContext
{
    public R2hErpDbContext()
    {
    }

    public R2hErpDbContext(DbContextOptions<R2hErpDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-D90893D\\SQLEXPRESS;Initial Catalog=R2h_Erp_Db;User ID=sa;Password=sethu903;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomersId).HasName("PK__Customer__EB5B58FE113775F3");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.UpdateedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF290029D7");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customers).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Customer__3D5E1FD2");

            entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__ProductI__3E52440B");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductsId).HasName("PK__Products__BB48EDE5247B773C");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateedOn).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
