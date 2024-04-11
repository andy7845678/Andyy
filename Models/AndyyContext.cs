using System;
using System.Collections.Generic;
using Andyy.Models;
using Microsoft.EntityFrameworkCore;

namespace Andyy.Models;

public partial class AndyyContext : DbContext
{
    public AndyyContext(DbContextOptions<AndyyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<WorkSheet> WorkSheet { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkSheet>(entity =>
        {
            entity.Property(e => e.CreateDateTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.UpDateTime).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}