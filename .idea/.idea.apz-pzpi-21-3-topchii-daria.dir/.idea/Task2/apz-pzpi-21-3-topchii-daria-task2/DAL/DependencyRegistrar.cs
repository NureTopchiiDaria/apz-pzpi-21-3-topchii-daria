using DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class DependencyRegistrar
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbConnectionString")));
            services.AddScoped(typeof(IGenericStorageWorker<>), typeof(GenericDbWorker<>));
            services.AddScoped<ITransactionsWorker, TransactionsWorker>();
        }
    }
}
