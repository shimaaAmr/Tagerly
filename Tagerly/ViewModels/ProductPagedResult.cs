namespace Tagerly.ViewModels
{
    public class ProductPagedResult
    {
        #region Pagination Data
        public IEnumerable<ProductViewModel> Products { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        #endregion
    }
}