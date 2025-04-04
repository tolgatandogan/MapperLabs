namespace SmartMapper.Core
{
    public class Mapper
    {
        public TDestination Map<TDestination>(object source)
        {
            return TypeAdapter.Adapt<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return TypeAdapter.Adapt<TDestination>(source);
        }
    }
}