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

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICustomerService, CustomerManager>();
            services.AddScoped<ICampaignService, CampaignManager>();
            services.AddScoped<IOrderDetailService, OrderDetailManager>();
            services.AddScoped<IOrderService, OrderManager>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());



            return services;
        }
    }
}
