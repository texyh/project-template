using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTemplate.Api
{
    public static class ServiceCollectionExtension
    {
        public static void Decorate(this IServiceCollection services, Type serviceType, Type implementaionType)
        {
            var wrappedDescriptor = services.FirstOrDefault(
              s => s.ServiceType == serviceType);

            if (wrappedDescriptor == null)
                throw new InvalidOperationException($"{serviceType.Name} is not registered");

            var objectFactory = ActivatorUtilities.CreateFactory(
              implementaionType,
              new[] { serviceType });

            
            services.Replace(ServiceDescriptor.Describe(
              serviceType,
              s => objectFactory(s, new[] { s.CreateInstance(wrappedDescriptor) }),
              wrappedDescriptor.Lifetime)
            );
        }

        private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
                return descriptor.ImplementationInstance;

            if (descriptor.ImplementationFactory != null)
                return descriptor.ImplementationFactory(services);

            return ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
        }
    }
}
