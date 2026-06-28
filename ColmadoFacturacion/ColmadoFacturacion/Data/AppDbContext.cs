using ColmadoFacturacion.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ColmadoFacturacion.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Rol> Roles { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Proveedor> Proveedores { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetalleVentas { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<CierreDiario> CierresDiarios { get; set; }
    public DbSet<MovimientoStock> MovimientosStock { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relación CierreDiario → Usuario
        modelBuilder.Entity<CierreDiario>()
            .HasOne(c => c.Usuario)
            .WithMany()
            .HasForeignKey(c => c.RealizadoPor);

        // Configurar precisión para todos los decimal
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) ||
                        p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(10);
            property.SetScale(2);
        }

        // Factura -> Venta (1 a 1)
        modelBuilder.Entity<Factura>()
            .HasOne(f => f.Venta)
            .WithOne(v => v.Factura)
            .HasForeignKey<Factura>(f => f.VentaID);

        // Número de factura único
        modelBuilder.Entity<Factura>()
            .HasIndex(f => f.NumeroFactura)
            .IsUnique();
     
        // Configuración explícita de CierreDiario
        modelBuilder.Entity<CierreDiario>(entity =>
        {
            entity.HasKey(e => e.CierreID);

            entity.HasIndex(e => e.Fecha)
                .IsUnique();

            entity.Property(e => e.TotalVentas)
                .HasPrecision(10, 2);

            entity.Property(e => e.TotalFacturado)
                .HasPrecision(10, 2);
        });
    }
}