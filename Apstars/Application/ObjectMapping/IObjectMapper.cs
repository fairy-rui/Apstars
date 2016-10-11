
namespace Apstars.Application.ObjectMapping
{
    public interface IObjectMapper //: IDependency
    {        
        TDestination Map<TDestination>(object source);
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
