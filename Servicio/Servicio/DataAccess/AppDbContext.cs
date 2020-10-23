using Servicio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servicio.Shared.Models;

namespace Servicio.Server.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Celdas> Celdas { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Marca> Marca { get; set; }
        public DbSet<Medida> Medida { get; set; }
        public DbSet<Modelo> Modelo { get; set; }
        public DbSet<Operario> Operario { get; set; }
        public DbSet<Orificio> Orificio { get; set; }
        public DbSet<Serie> Serie { get; set; }
        public DbSet<Service> Servicios { get; set; }
        public DbSet<Sobrepresion> Sobrepresion { get; set; }
        public DbSet<Tipo> Tipo { get; set; }
        public DbSet<Trabajosefec> Trabajosefec { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

    }
}
