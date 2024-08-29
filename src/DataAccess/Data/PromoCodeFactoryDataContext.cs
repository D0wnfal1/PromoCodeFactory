using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administation;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Data
{
    public class PromoCodeFactoryDataContext : DbContext
    {
        public PromoCodeFactoryDataContext(DbContextOptions<PromoCodeFactoryDataContext> promoCodeFactoryDataContext) : base(promoCodeFactoryDataContext)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<CustomerPreference> CustomerPreferences { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee - Role (Many-to-Many -> EmployeeRole)
            modelBuilder.Entity<EmployeeRole>()
                .HasKey(er => new { er.EmployeeId, er.RoleId });

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.EmployeeId);

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Role)
                .WithMany(r => r.EmployeeRoles)
                .HasForeignKey(er => er.RoleId);

            modelBuilder.Entity<Employee>()
                .Property(e => e.LastName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Description)
                .HasMaxLength(200);

            // Customer - Preference (Many-to-Many -> CustomerPreference)
            modelBuilder.Entity<CustomerPreference>()
                .HasKey(cp => new { cp.CustomerId, cp.PreferenceId });

            modelBuilder.Entity<CustomerPreference>()
                .HasOne(cp => cp.Customer)
                .WithMany(c => c.CustomerPreferences)
                .HasForeignKey(cp => cp.CustomerId);

            modelBuilder.Entity<CustomerPreference>()
                .HasOne(cp => cp.Preference)
                .WithMany(p => p.CustomerPreferences)
                .HasForeignKey(cp => cp.PreferenceId);

            modelBuilder.Entity<Customer>()
                .Property(c => c.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.LastName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Preference>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            // PromoCode - Preference & Customer (One-to-Many)
            modelBuilder.Entity<PromoCode>()
                .HasOne(p => p.Preference)
                .WithMany()
                .HasForeignKey(pc => pc.PreferenceId);

            modelBuilder.Entity<PromoCode>()
                .HasOne(pc => pc.Customer)
                .WithMany(c => c.PromoCodes)
                .HasForeignKey(pc => pc.CustomerId);

            modelBuilder.Entity<PromoCode>()
                .HasOne(pc => pc.PartnerManager)
                .WithMany()
                .HasForeignKey(pc => pc.PartnerManagerId);

            modelBuilder.Entity<PromoCode>()
                .Property(pc => pc.Code)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<PromoCode>()
                .Property(pc => pc.ServiceName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<PromoCode>()
                .Property(pc => pc.PartnerName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
