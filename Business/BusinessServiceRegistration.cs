using Autofac;
using Business.Abstracts;
using Business.Concretes;
using Business.Profiles;
using AutoMapper;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Autofac ile DI container'ı entegre etmek için
            //var builder = new ContainerBuilder();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICustomerService, CustomerManager>();
            services.AddScoped<ICampaignService, CampaignManager>();
            services.AddScoped<IOrderDetailService, OrderDetailManager>();
            services.AddScoped<IOrderService, OrderManager>();

            //services.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //       .Where(t => t.Name.EndsWith("Manager"))
            //       .AsImplementedInterfaces()
            //       .InstancePerLifetimeScope();

            // Autofac'in servisleri kullanabilmesi için IServiceCollection'a ekle
            //builder.Populate(services);
            //services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());



            return services;
        }
    }
}
