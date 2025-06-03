using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace LiveLib.Application.Common.Mapping
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile(Assembly assembly)
            : this()
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            ApplyMappingsFromAssembly(assembly);
        }

        public AssemblyMappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            try
            {
                const string mappingMethodName = "Mapping";
                var mapInterfaceType = typeof(IMapWith<>);

                var types = assembly.GetExportedTypes()
                    .Where(t => t.GetInterfaces()
                        .Any(i => i.IsGenericType &&
                                 i.GetGenericTypeDefinition() == mapInterfaceType))
                    .ToList();

                foreach (var type in types)
                {
                    var instance = Activator.CreateInstance(type);
                    var methodInfo = type.GetMethod(mappingMethodName) ??
                                    type.GetInterface(mapInterfaceType.Name)?.GetMethod(mappingMethodName);

                    methodInfo?.Invoke(instance, new object[] { this });
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var loaderMessages = string.Join("\n", ex.LoaderExceptions.Select(e => e.Message));
                throw new InvalidOperationException(
                    $"Error loading types from assembly {assembly.FullName}:\n{loaderMessages}", ex);
            }
        }
    }
}