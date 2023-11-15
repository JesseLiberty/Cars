using Cars.Data.Repositories;

namespace Cars.Data.Interfaces
{
    public static class Extensions
    {
        public static IServiceCollection RegisterDataAccessDependencies(this IServiceCollection services)
        {
            services.AddTransient<ICarRepository, CarRepository>();

            return services;
        }
    }

}
