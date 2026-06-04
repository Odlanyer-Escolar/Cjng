using Cjng.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cjng.Contexto
{
    public class CJNG_ChecadorDB : DbContext
    {
        public CJNG_ChecadorDB()
        {
            Database.EnsureCreated();
        }
        public DbSet<Trabajador> Trabajadores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CJNG_ChecadorDB.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configure the relations
            modelBuilder.Entity<Trabajador>(e => { e.HasKey(p => p.Id); e.Property(p => p.Id).UseAutoincrement(); });


            //Configure additional columns
            modelBuilder
                .Entity<Usuario>(e => { e.HasKey(p => p.Id); e.Property(p => p.Id).UseAutoincrement(); });
        }
    }
}
