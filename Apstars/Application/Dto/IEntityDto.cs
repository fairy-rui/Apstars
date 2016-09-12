
namespace Apstars.Application.Dto
{
    /// <summary>
    /// Defines common properties for entity based DTOs.
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// ID of the entity.
        /// </summary>
        TPrimaryKey ID { get; set; }
    }

    /// <summary>
    /// A shortcut of <see cref="IEntityDto{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    public interface IEntityDto : IEntityDto<int>
    {

    }
}
