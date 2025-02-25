using Autofac;
using Business.Abstracts;
using Business.Concretes;
using Business.Profiles;
using AutoMapper;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Core.Business.Rules;

namespace Business
{
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICustomerService, CustomerManager>();
            services.AddScoped<ICampaignService, CampaignManager>();
            services.AddScoped<IOrderDetailService, OrderDetailManager>();
            services.AddScoped<IOrderService, OrderManager>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

            return services;
        }
        public static IServiceCollection AddSubClassesOfType(this IServiceCollection services,
         Assembly assembly, Type type, Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
            foreach (var item in types)
                if (addWithLifeCycle == null)
                    services.AddScoped(item);

                else
                    addWithLifeCycle(services, type);
            return services;
        }
    }
}
