using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Audit;
using OrderFlow.Domain.Entities;
using OrderFlow.Infrastructure.Interceptors;

namespace OrderFlow.Infrastructure.Persistence;

public class OrderFlowDbContext(
    DbContextOptions<OrderFlowDbContext> options,
    AuditInterceptor auditInterceptor)
    : DbContext(options)
{
    private readonly AuditInterceptor auditInterceptor = auditInterceptor;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderFlowDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        =>  optionsBuilder.AddInterceptors(auditInterceptor);

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
}
