﻿using Microsoft.EntityFrameworkCore;
using API.Models;
using API.DTOs.Roles;
using System.Xml.Linq;

namespace API.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<University> Universities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
                 new NewRoleDefaultDto
        {
            Guid = Guid.Parse("f96a6e22-f9b7-46a4-1890-08db922b6b7e"),
            Name = "Employee"
        },
                 new NewRoleDefaultDto
        {
            Guid = Guid.Parse("f96a6e22-f9b7-46a4-1890-08db922b6b8e"),
            Name = "Manager"
        },     
                 new NewRoleDefaultDto
        {
            Guid = Guid.Parse("f96a6e22-f9b7-46a4-1890-08db922b6b9e"),
            Name = "Admin"
        });

        modelBuilder.Entity<Employee>()
                    .HasIndex(e => new
                    {
                        e.NIK,
                        e.Email,
                        e.PhoneNumber
                    }).IsUnique();

        // Many Educations with One University (N:1)
        modelBuilder.Entity<Education>()
                    .HasOne(e => e.University)
                    .WithMany(u => u.Educations)
                    .HasForeignKey(u => u.UniversityGuid)
                    .OnDelete(DeleteBehavior.Restrict);

        // Many Bookings with One Room (N:1)
        modelBuilder.Entity<Booking>()
                    .HasOne(b => b.Room)
                    .WithMany(r => r.Bookings)
                    .HasForeignKey(b => b.RoomGuid)
                    .OnDelete(DeleteBehavior.Restrict);

        // Many Account Roles with One Role (N:1)
        modelBuilder.Entity<AccountRole>()
                    .HasOne(ar => ar.Role)
                    .WithMany(r => r.AccountRoles)
                    .HasForeignKey(ar => ar.RoleGuid)
                    .OnDelete(DeleteBehavior.Restrict);

        // Many Account Roles with One Account (1:N)
        modelBuilder.Entity<Account>()
                    .HasMany(a => a.AccountRoles)
                    .WithOne(ar => ar.Account)
                    .HasForeignKey(ar => ar.AccountGuid)
                    .OnDelete(DeleteBehavior.Restrict);

        // One Employee with One Account (1:1)
        modelBuilder.Entity<Employee>()
                    .HasOne(e => e.Account)
                    .WithOne(a => a.Employee)
                    .HasForeignKey<Account>(a => a.Guid)
                    .OnDelete(DeleteBehavior.Restrict);

        // One Employee with One Education (1:1)
        modelBuilder.Entity<Employee>()
                    .HasOne(e => e.Education)
                    .WithOne(ed => ed.Employee)
                    .HasForeignKey<Education>(ed => ed.Guid)
                    .OnDelete(DeleteBehavior.Restrict);

        // Many Bookings with One Employee (1:N)
        modelBuilder.Entity<Employee>()
                    .HasMany(e => e.Bookings)
                    .WithOne(b => b.Employee)
                    .HasForeignKey(b => b.EmployeeGuid)
                    .OnDelete(DeleteBehavior.Restrict);
    }
}


