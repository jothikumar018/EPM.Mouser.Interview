using EPM.Mouser.Interview.Web.Interfaces.Services;
using EPM.Mouser.Interview.Web.Services;

namespace EPM.Mouser.Interview.Web.Common
{
    public static class ServiceExtension
    {
        public static IServiceCollection SetupServiceForWarehouse(this IServiceCollection services)
        {
            services.AddTransient<IWarehouseService, WarehouseService>();
            return services;
        }
    }
}
