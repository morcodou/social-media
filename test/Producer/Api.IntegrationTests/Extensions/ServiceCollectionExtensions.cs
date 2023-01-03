namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Removes all registered <see cref="ServiceLifetime.Singleton"/> registrations of <see cref="TService"/> and adds a new registration which uses the <see cref="Func{IServiceProvider, TService}"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service interface which needs to be placed.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="implementationFactory">The implementation factory for the specified type.</param>
        public static IServiceCollection SwapSingleton<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory) =>
                services.SwapService(implementationFactory, ServiceLifetime.Singleton);

        /// <summary>
        /// Removes all registered <see cref="ServiceLifetime.Transient"/> registrations of <see cref="TService"/> and adds a new registration which uses the <see cref="Func{IServiceProvider, TService}"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service interface which needs to be placed.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="implementationFactory">The implementation factory for the specified type.</param>
        public static IServiceCollection SwapTransient<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory) =>
                services.SwapService(implementationFactory, ServiceLifetime.Transient);

        /// <summary>
        /// Removes all registered <see cref="ServiceLifetime.Scoped"/> registrations of <see cref="TService"/> and adds a new registration which uses the <see cref="Func{IServiceProvider, TService}"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service interface which needs to be placed.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="implementationFactory">The implementation factory for the specified type.</param>
        public static IServiceCollection SwapScoped<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory) => 
                services.SwapService(implementationFactory, ServiceLifetime.Scoped);


        /// <summary>
        /// Removes all registered <see cref="ServiceLifetime.Scoped"/> registrations of <see cref="TService"/> and adds a new registration which uses the <see cref="Func{IServiceProvider, TService}"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service interface which needs to be placed.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="implementationFactory">The implementation factory for the specified type.</param>
        /// <param name="lifetime">The lifetime of a service in an <see cref="IServiceCollection"/>.</param>
        public static IServiceCollection SwapService<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ServiceLifetime lifetime)
        {
            if (services.Any(x => x.ServiceType == typeof(TService) && x.Lifetime == lifetime))
            {
                var serviceDescriptors = services.Where(x => x.ServiceType == typeof(TService) && x.Lifetime == lifetime)
                                                 .ToList();
                foreach (var serviceDescriptor in serviceDescriptors)
                {
                    services.Remove(serviceDescriptor);
                }
            }

            return lifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton(typeof(TService), (sp) => implementationFactory(sp)!),
                ServiceLifetime.Scoped => services.AddScoped(typeof(TService), (sp) => implementationFactory(sp)!),
                ServiceLifetime.Transient => services.AddTransient(typeof(TService), (sp) => implementationFactory(sp)!),
                _ => services
            };
        }
    }
}