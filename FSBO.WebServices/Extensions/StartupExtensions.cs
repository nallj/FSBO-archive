using System;
using Microsoft.Extensions.DependencyInjection;

using FSBO.DAL;

namespace FSBO.WebServices.Extensions
{
    public static partial class StartupExtensions
    {

        /// <summary>
        /// Adds a scoped FsboContext, connecting with the connection string passed when instantiating.
        /// </summary>
        public static IServiceCollection AddFsboContext(this IServiceCollection services, string connectionString)
        {
            return services
                .AddScoped<IFsboContext>(x =>
                {
                    var dbContext = new FsboContext(connectionString);
                    return dbContext;
                });
                // This is needed for the repository base (can't use interface).
                //.AddScoped(x =>
                //{
                //    var dbContext = new FsboContext(connectionString);
                //    return dbContext;
                //});
        }

        /// <summary>
        /// Adds a FsboContext factory method as a singleton, connecting with the connection string passed when instantiating.  "Creates a function that creates a new FsboContent each time.  This is necessary for the claims transformer when it syncs with the User table."
        /// </summary>
        public static IServiceCollection AddFsboContextFactoryMethod(this IServiceCollection services, string connectionString)
        {
            return services.AddSingleton<Func<IFsboContext>>(x =>
            {
                return () =>
                {
                    var dbContext = new FsboContext(connectionString);
                    return dbContext;
                };
            });
        }

    }
}