using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p=>p.Id).IsRequired();
            builder.Property(p=>p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Description).IsRequired().HasMaxLength(180);
            builder.Property(p=>p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p=>p.PictureUrl).IsRequired();
            //HasOne pq cada Product tem uma Brand e WithMany() pq cada Brand pode ter muitos Product
            builder.HasOne(b => b.ProductBrand).WithMany()
                .HasForeignKey(b => b.ProductBrandId);
            //HasOne pq cada Product tem um Type e WithMany() pq cada Type pode ter muitos Product
            builder.HasOne(t => t.ProductType).WithMany()
                .HasForeignKey(t => t.ProductTypeId);
        }
    }
}