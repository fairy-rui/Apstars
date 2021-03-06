﻿using System;

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
    /// A shortcut of <see cref="IdInput{TPrimaryKey}"/> for <see cref="int"/>.
    /// </summary>
    [Serializable]
    public class IdInput : IdInput<int>
    {
        public IdInput()
        {

        }

        public IdInput(int id)
            : base(id)
        {

        }
    }
}
