namespace BookShopSystem.DataAccess.Context
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using BookShopSystem.DataAccess.Entity;

    public partial class BookShopContext : DbContext
    {
        public BookShopContext()
            : base("name=BookShopContext")
        {
        }

        public virtual DbSet<BaseUser> BaseUser { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<FileUpload> FileUpload { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrdersDetail> OrdersDetail { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductComment> ProductComment { get; set; }
        public virtual DbSet<Profession> Profession { get; set; }
        public virtual DbSet<ShopingCart> ShopingCart { get; set; }
        public virtual DbSet<UserAddress> UserAddress { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Publishers> Publishers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseUser>()
                .Property(e => e.Account)
                .IsUnicode(false);

            modelBuilder.Entity<BaseUser>()
                .Property(e => e.Pwd)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<FileUpload>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<FileUpload>()
                .Property(e => e.FilePath)
                .IsUnicode(false);

            modelBuilder.Entity<FileUpload>()
                .Property(e => e.MediaType)
                .IsUnicode(false);

            modelBuilder.Entity<FileUpload>()
                .Property(e => e.SourceId)
                .IsUnicode(false);

            modelBuilder.Entity<Orders>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<Orders>()
                .Property(e => e.Contacts)
                .IsUnicode(false);

            modelBuilder.Entity<Orders>()
                .Property(e => e.Tel)
                .IsUnicode(false);

            modelBuilder.Entity<Orders>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Orders>()
                .Property(e => e.TotalPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrdersDetail>()
                .Property(e => e.UnitPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Author)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.UnitPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ProductComment>()
                .Property(e => e.OrderNum)
                .IsUnicode(false);

            modelBuilder.Entity<ProductComment>()
                .Property(e => e.Contents)
                .IsUnicode(false);

            modelBuilder.Entity<Profession>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<UserAddress>()
                .Property(e => e.ProvinceCode)
                .IsUnicode(false);

            modelBuilder.Entity<UserAddress>()
                .Property(e => e.CityCode)
                .IsUnicode(false);

            modelBuilder.Entity<UserAddress>()
                .Property(e => e.DistrictCode)
                .IsUnicode(false);

            modelBuilder.Entity<UserAddress>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<UserAddress>()
                .Property(e => e.Contacts)
                .IsUnicode(false);

            modelBuilder.Entity<UserAddress>()
                .Property(e => e.Tel)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.RegionName)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.RegionCode)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.ParentCode)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.Latitude)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.Levels)
                .IsUnicode(false);
        }
    }
}
