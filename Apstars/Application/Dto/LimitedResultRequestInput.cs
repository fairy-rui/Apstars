using System.ComponentModel.DataAnnotations;

namespace Apstars.Application.Dto
{
    /// <summary>
    /// Simply implements <see cref="ILimitedResultRequest"/>.
    /// </summary>
    public class LimitedResultRequestInput : ILimitedResultRequest
    {
        private int limit = 40;
        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount 
        { 
            get
            {
                return limit;
            }
            set
            {
                limit = value;
            }
        }
    }
}
