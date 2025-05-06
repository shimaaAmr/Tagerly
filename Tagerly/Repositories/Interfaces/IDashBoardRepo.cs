namespace Tagerly.Repositories.Interfaces
{
    public interface IDashBoardRepo
    {
        int GetUsersCount();
        int GetProductsCount();
        int GetOrdersCount();
        decimal GetTotalRevenue();
        List<int> GetOrdersPerMonth();
        Dictionary<string, int> GetProductsPerCategory();
    }
}
