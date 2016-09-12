
namespace Apstars.Application.AutoMapping
{
    public interface IAutoMapper : IDependency
    {        
        TDestination Map<TDestination>(object source);
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
