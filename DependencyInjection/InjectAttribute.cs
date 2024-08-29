using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjection
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false)]
    public class InjectAttribute : Attribute { }

    public class DIContainer
    {
        private readonly Dictionary<Type, Type> _registrations = new Dictionary<Type, Type>();

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _registrations[typeof(TInterface)] = typeof(TImplementation);
        }

        public T Resolve<T>()
        {
            return (T)ResolveType(typeof(T));
        }

        private object ResolveType(Type type)
        {
            if (_registrations.TryGetValue(type, out Type implementationType))
            {
                return CreateInstance(implementationType);
            }

            if (type.IsInterface || type.IsAbstract)
            {
                throw new InvalidOperationException($"No registration found for {type}");
            }

            return CreateInstance(type);
        }

        private object CreateInstance(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            ConstructorInfo constructor = constructors.FirstOrDefault(c => c.GetCustomAttribute<InjectAttribute>() != null) ?? constructors.First();

            ParameterInfo[] parameters = constructor.GetParameters();
            object[] parameterInstances = parameters.Select(p => ResolveType(p.ParameterType)).ToArray();

            object instance = constructor.Invoke(parameterInstances);

            foreach (MethodInfo method in type.GetMethods().Where(m => m.GetCustomAttribute<InjectAttribute>() != null))
            {
                ParameterInfo[] methodParameters = method.GetParameters();
                object[] methodParameterInstances = methodParameters.Select(p => ResolveType(p.ParameterType)).ToArray();
                method.Invoke(instance, methodParameterInstances);
            }

            return instance;
        }
    }
}
