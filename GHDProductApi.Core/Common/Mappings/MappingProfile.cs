using AutoMapper;
using GHDProductApi.Core.Common.Interfaces;
using System.Reflection;

namespace GHDProductApi.Core.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssemblies(typeof(MappingProfile).Assembly);
        }

        private void ApplyMappingsFromAssemblies(Assembly assembly)
        {
            List<Type> types = assembly.GetExportedTypes()
                                       .Where(
                                           t => t.GetInterfaces()
                                                 .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                                       )
                                       .ToList();

            foreach (var type in types)
            {
                object? instance = Activator.CreateInstance(type);

                MethodInfo? methodInfo = type.GetMethod("Mapping")
                                      ?? type.GetInterface("IMapFrom`1")
                                             ?.GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
