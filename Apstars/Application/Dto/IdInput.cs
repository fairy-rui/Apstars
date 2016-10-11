using System;

namespace Apstars.Application.Dto
{
    /// <summary>
    /// This DTO can be directly used (or inherited)
    /// to pass an Id value to an application service method.
    /// </summary>
    /// <typeparam name="TId">Type of the Id</typeparam>
    [Serializable]
    public class IdInput<TId> : EntityDto<TId>
    {
        public IdInput()
        {

        }

        public IdInput(TId id)
            : base(id)
        {
            ID = id;
        }
    }

    /// <summary>
    /// A shortcut of <see cref="IdInput{TPrimaryKey}"/> for <see cref="Guid"/>.
    /// </summary>
    [Serializable]
    public class IdInput : IdInput<Guid>
    {
        public IdInput()
        {

        }

        public IdInput(Guid id)
            : base(id)
        {

        }
    }
}
