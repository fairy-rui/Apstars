
namespace Apstars.Application
{
    /// <summary>
    /// 表示一个提供了分页相关信息的数据传输对象。
    /// </summary>
    public class Pagination
    {
        #region 私有属性        
        private int pageNumber;
        private int pageSize;
        private System.Nullable<int> totalPages;
        #endregion

        #region Public Members
        public int PageNumber
        {
            get
            {
                return this.pageNumber;
            }
            set
            {
                this.pageNumber = value;
            }
        }
        
        public int PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }
       
        public System.Nullable<int> TotalPages
        {
            get
            {
                return this.totalPages;
            }
            set
            {
                this.totalPages = value;
            }
        }
        
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return string.Format("PageSize={0} PageNumber={1} TotalPages={2}",
                this.PageSize,
                this.PageNumber,
                this.TotalPages ?? 0);
        }
        #endregion
    }
}
