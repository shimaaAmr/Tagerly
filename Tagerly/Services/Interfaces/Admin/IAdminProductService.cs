using Tagerly.ViewModels;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Services.Interfaces.Admin
{
    public interface IAdminProductService
    {
        Task<IEnumerable<ProductApprovingVM>> GetAllProductsAsync();
        Task<bool> ApproveProductAsync(int id);
        Task<bool> RejectProductAsync(int id);
        Task<bool> DeleteProductAsync(int id);

        Task<bool> ChangeApprovalStatusAsync(int id, bool isApproved);
        Task<ProductApprovingVM> GetProductByIdAsync(int id);


    }
}
