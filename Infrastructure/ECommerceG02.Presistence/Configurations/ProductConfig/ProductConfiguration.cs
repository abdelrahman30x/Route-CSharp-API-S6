using ECommerceG02.Domian.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Presistence.Configurations.ProductConfig
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(e => e.Brand).WithMany().HasForeignKey(e => e.BrandId).OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.Type).WithMany().HasForeignKey(e => e.TypeId).OnDelete(DeleteBehavior.SetNull);
            builder.Property(e => e.Price).HasColumnType("decimal(10,3)");
        }
    }
}
