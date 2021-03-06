﻿using System;

namespace Apstars.Application.Dto
{
    /// <summary>
    /// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class PagedAndSortedResultRequestInput : PagedResultRequestInput, IPagedAndSortedResultRequest
    {
        public virtual string Sorting { get; set; }
    }
}
