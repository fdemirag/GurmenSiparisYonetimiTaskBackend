using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace DataAccess
{
    public static class DataAccessServiceRegistration
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {


            services.AddScoped<DapperContext>();  // DapperContext servisini ekliyoruz

            //// Autofac ile DI container'ı entegre etmek için
            //var builder = new ContainerBuilder();

            //// "Repository" ile biten tüm sınıfları ekle
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //       .Where(x => x.Name.EndsWith("Repository"))
            //       .AsImplementedInterfaces()
            //       .InstancePerLifetimeScope();

            //// Autofac'in servisleri kullanabilmesi için IServiceCollection'a ekle
            //builder.Populate(services);

            return services;
        }
    }
}
