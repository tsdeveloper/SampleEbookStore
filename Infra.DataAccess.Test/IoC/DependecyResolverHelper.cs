using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DataAccess.Test.IoC;

public class DependecyResolverHelper
{
  private readonly IWebHost _webHost;
  public DependecyResolverHelper(IWebHost webHost) => _webHost = webHost;

  public T GetService<T>()
  {
    var serviceScope = _webHost.Services.CreateScope();
    var services = serviceScope.ServiceProvider;

    try
    {
      var scopedService = services.GetRequiredService<T>();
      return scopedService;
    }
    catch (Exception ex)
    {
       // TODO
       Console.WriteLine(ex);
       throw;
    }
  }
}
