using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
namespace Infra.Data;

public class SampleEbookStoreContext : DbContext
{
  public SampleEbookStoreContext(DbContextOptions<SampleEbookStoreContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder m)
  {
    base.OnModelCreating(m);
    m.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}