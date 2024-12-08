using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Models;

public partial class ProjectContext : DbContext
{
    public ProjectContext()
    {
    }

    public ProjectContext(DbContextOptions<ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admlookup> Admlookups { get; set; }

    public virtual DbSet<Cartdetail> Cartdetails { get; set; }

    public virtual DbSet<Cartheater> Cartheaters { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Orderheader> Orderheaders { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductEn> ProductEns { get; set; }

    public virtual DbSet<ProductJp> ProductJps { get; set; }

    public virtual DbSet<Stockheader> Stockheaders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VCartAll> VCartAlls { get; set; }

    public virtual DbSet<VCartProduct> VCartProducts { get; set; }

    public virtual DbSet<VOrderHistory> VOrderHistories { get; set; }

    public virtual DbSet<VOrderheaderOrderdetail> VOrderheaderOrderdetails { get; set; }

    public virtual DbSet<VProductStock> VProductStocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\sqlexpress; Database=PROJECT; User ID=sa; Password=12345; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admlookup>(entity =>
        {
            entity.HasKey(e => e.Lookupid).HasName("PK__ADMLOOKU__7F261053169B8C16");

            entity.ToTable("ADMLOOKUP");

            entity.Property(e => e.Lookupid)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("LOOKUPID");
            entity.Property(e => e.Classno)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CLASSNO");
            entity.Property(e => e.Createdate)
                .HasColumnType("datetime")
                .HasColumnName("CREATEDATE");
            entity.Property(e => e.Createuser)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CREATEUSER");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Updatedate)
                .HasColumnType("datetime")
                .HasColumnName("UPDATEDATE");
            entity.Property(e => e.Updateuser)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("UPDATEUSER");
        });

        modelBuilder.Entity<Cartdetail>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.CartItemId }).HasName("PK__CARTDETA__51BCD7B7586C69E1");

            entity.ToTable("CARTDETAIL");

            entity.Property(e => e.CartId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ProductId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Qty).HasColumnName("QTY");
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cartheater>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__CARTHEAT__51BCD7B75865C4F4");

            entity.ToTable("CARTHEATER");

            entity.Property(e => e.CartId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__CUSTOMER__A4AE64B824E3E012");

            entity.ToTable("CUSTOMER");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CustomerID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.ImgSrc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Language)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ReceiverAddress).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateUser)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasKey(e => new { e.HistoryId, e.OrderId }).HasName("PK__OrderSta__4D7B4ADDDE804D63");

            entity.ToTable("OrderStatusHistory");

            entity.Property(e => e.HistoryId)
                .ValueGeneratedOnAdd()
                .HasColumnName("HistoryID");
            entity.Property(e => e.OrderId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Updatedate)
                .HasColumnType("datetime")
                .HasColumnName("UPDATEDATE");
            entity.Property(e => e.Updateuser)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("UPDATEUSER");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.OrderItem }).HasName("PK__ORDERDET__C3905BCF9EC2CA22");

            entity.ToTable("ORDERDETAIL");

            entity.Property(e => e.OrderId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.ProductId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Uom).HasMaxLength(6);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateUser)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Orderheader>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__ORDERHEA__C3905BCF70681B5F");

            entity.ToTable("ORDERHEADER", tb =>
                {
                    tb.HasTrigger("trg_Insert_OrderStatusHistory");
                    tb.HasTrigger("trg_OrderStatusHistory");
                });

            entity.Property(e => e.OrderId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CustomerID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Payment)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ShipStatus)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.ShippingMethod)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateUser)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PayId).HasName("PK__PAYMENT__EE8FCE2F5B4376E1");

            entity.ToTable("PAYMENT");

            entity.Property(e => e.PayId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("PayID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Currency)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.OrderId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateUser)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PRODUCT");

            entity.Property(e => e.Baking)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreateUser).HasMaxLength(12);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Flavor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Img)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Method)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.Strong).HasColumnName("STRONG");
            entity.Property(e => e.Timelimit).HasMaxLength(4);
            entity.Property(e => e.Uom)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateUser).HasMaxLength(12);
            entity.Property(e => e.Weight)
                .HasMaxLength(4)
                .IsFixedLength();
        });

        modelBuilder.Entity<ProductEn>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PRODUCT_EN");

            entity.Property(e => e.Baking)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreateUser).HasMaxLength(12);
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Flavor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Img)
                .HasMaxLength(46)
                .IsUnicode(false);
            entity.Property(e => e.Method)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.Strong).HasColumnName("STRONG");
            entity.Property(e => e.Timelimit).HasMaxLength(10);
            entity.Property(e => e.Uom)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateUser).HasMaxLength(12);
            entity.Property(e => e.Weight)
                .HasMaxLength(4)
                .IsFixedLength();
        });

        modelBuilder.Entity<ProductJp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PRODUCT_JP");

            entity.Property(e => e.Baking).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreateUser).HasMaxLength(12);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Flavor).HasMaxLength(100);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Img)
                .HasMaxLength(46)
                .IsUnicode(false);
            entity.Property(e => e.Method).HasMaxLength(100);
            entity.Property(e => e.ProductId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.Strong).HasColumnName("STRONG");
            entity.Property(e => e.Timelimit).HasMaxLength(4);
            entity.Property(e => e.Uom)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateUser).HasMaxLength(12);
            entity.Property(e => e.Weight)
                .HasMaxLength(4)
                .IsFixedLength();
        });

        modelBuilder.Entity<Stockheader>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__STOCKHEA__2C83A9E220AE656A");

            entity.ToTable("STOCKHEADER");

            entity.Property(e => e.StockId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("StockID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreateUser)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.ProductId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Uom)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateUser)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACE99B2E24");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E48E142D64").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<VCartAll>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_cartALL");

            entity.Property(e => e.CartId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ProductId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Qty).HasColumnName("QTY");
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VCartProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_cart_product");

            entity.Property(e => e.CartId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DStatus)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("d_Status");
            entity.Property(e => e.Img)
                .HasMaxLength(28)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.Qty).HasColumnName("QTY");
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VOrderHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_OrderHistory");

            entity.Property(e => e.Classno)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CLASSNO");
            entity.Property(e => e.Name)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.OrderId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Updatedate)
                .HasColumnType("datetime")
                .HasColumnName("UPDATEDATE");
            entity.Property(e => e.Updateuser)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("UPDATEUSER");
        });

        modelBuilder.Entity<VOrderheaderOrderdetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_orderheader_orderdetail");

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CustomerId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ImgSrc)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.PayMathod)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Payment)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.ShipMethod)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ShipStatu)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ShipStatus)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.ShippingMethod)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Uom).HasMaxLength(6);
        });

        modelBuilder.Entity<VProductStock>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_product_stock");

            entity.Property(e => e.Baking)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Flavor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImgA)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImgB)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImgC)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImgD)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Method)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ProductID");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.StockId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("StockID");
            entity.Property(e => e.Strong).HasColumnName("STRONG");
            entity.Property(e => e.Timelimit).HasColumnType("datetime");
            entity.Property(e => e.Uom)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
