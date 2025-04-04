using SmartMapper.Core;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System;

public class TypeAdapterConfig<TSource, TDestination>
{
    private Func<TSource, TDestination> _mapFunction;
    private readonly Dictionary<string, Func<TSource, object>> _memberConfigs = new();

    public static TypeAdapterConfig<TSource, TDestination> ForType()
    {
        return new TypeAdapterConfig<TSource, TDestination>();
    }

    public TypeAdapterConfig<TSource, TDestination> ConstructUsing(Func<TSource, TDestination> mapFunc)
    {
        _mapFunction = mapFunc;
        return this;
    }

    public TypeAdapterConfig<TSource, TDestination> ForMember<TProperty>(
        Expression<Func<TDestination, TProperty>> destinationMember,
        Func<TSource, object> sourceExpression)
    {
        var memberName = ((MemberExpression)destinationMember.Body).Member.Name;
        _memberConfigs[memberName] = sourceExpression;
        return this;
    }

    // YENİ: Konfigürasyonu tamamlayıp mapping'i kaydeden metot
    public void Register()
    {
        var mapFunc = BuildMap();
        TypeAdapter.AddMapping(mapFunc);
    }

    private Func<TSource, TDestination> BuildMap()
    {
        if (_mapFunction != null)
            return _mapFunction;

        // Expression Tree ile otomatik mapping
        var sourceParam = Expression.Parameter(typeof(TSource), "src");
        var bindings = new List<MemberBinding>();

        foreach (var destProp in typeof(TDestination).GetProperties())
        {
            if (_memberConfigs.TryGetValue(destProp.Name, out var sourceFunc))
            {
                // Özel konfigürasyon
                var value = Expression.Invoke(Expression.Constant(sourceFunc), sourceParam);
                bindings.Add(Expression.Bind(destProp, Expression.Convert(value, destProp.PropertyType)));
            }
            else
            {
                // Otomatik eşleme
                var srcProp = typeof(TSource).GetProperty(destProp.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (srcProp?.CanRead == true && destProp.CanWrite)
                {
                    bindings.Add(Expression.Bind(destProp, Expression.Property(sourceParam, srcProp)));
                }
            }
        }

        var memberInit = Expression.MemberInit(Expression.New(typeof(TDestination)), bindings);
        return Expression.Lambda<Func<TSource, TDestination>>(memberInit, sourceParam).Compile();
    }
}