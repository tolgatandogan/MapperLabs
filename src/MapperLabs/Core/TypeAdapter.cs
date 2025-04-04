using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace SmartMapper.Core
{
    public static class TypeAdapter
    {
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, Delegate>> _mappings = new();

        public static void AddMapping<TSource, TDestination>(Func<TSource, TDestination> mapFunc)
        {
            var wrappedFunc = CreateWrappedFunc(mapFunc);
            var sourceType = typeof(TSource);
            var destType = typeof(TDestination);

            _mappings
                .GetOrAdd(sourceType, _ => new ConcurrentDictionary<Type, Delegate>())
                .AddOrUpdate(destType, wrappedFunc, (_, _) => wrappedFunc);
        }

        private static Func<object, TDestination> CreateWrappedFunc<TSource, TDestination>(Func<TSource, TDestination> mapFunc)
        {
            var sourceParam = Expression.Parameter(typeof(object), "src");
            var castSource = Expression.Convert(sourceParam, typeof(TSource));
            var invokeMap = Expression.Invoke(Expression.Constant(mapFunc), castSource);
            return Expression.Lambda<Func<object, TDestination>>(invokeMap, sourceParam).Compile();
        }

        public static TDestination Adapt<TDestination>(this object source)
        {
            var sourceType = source.GetType();
            var destType = typeof(TDestination);

            if (!_mappings.TryGetValue(sourceType, out var destMappings))
                throw new InvalidOperationException($"No mapping found for {sourceType.Name}");

            if (!destMappings.TryGetValue(destType, out var func))
                throw new InvalidOperationException($"No mapping from {sourceType.Name} to {destType.Name}");

            return ((Func<object, TDestination>)func)(source);
        }
    }
}