using ECommerce.Domain;
using ECommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DAL.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.OrderStatus)
                .HasConversion<string>() // Store the enum as a string in the database
                .HasMaxLength(20);

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.ApplicationUser)
                   .WithMany(u=>u.Orders)
                   .HasForeignKey(o => o.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}






