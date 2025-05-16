using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tagerly.Repositories.Interfaces;
using Tagerly.Services.Interfaces.Admin;
using Tagerly.ViewModels;
using Tagerly.ViewModels.AdminViewModel;

namespace Tagerly.Services.Implementations.Admin
{
    public class AdminOrderService:IAdminOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;

        public AdminOrderService(IOrderRepo orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }
        public async Task<List<AdminOrderViewModel>> GetPendingOrdersAsync()
        {
            var pendingOrders = await _orderRepo.FindAsync(o => o.Status == "Pending");
            return _mapper.Map<List<AdminOrderViewModel>>(pendingOrders);
        }

        public async Task<bool> MarkAsDeliveredAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = "Delivered";
            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();

            return true;
        }

        public async Task<List<AdminOrderViewModel>> GetFilteredOrdersAsync(string status, string search)
        {
            var query = _orderRepo.GetAllWithUserAndDetails(); // يرجّع IQueryable<Order>

            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.Status == status);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(o => o.User.Email.Contains(search) || o.Id.ToString().Contains(search));

            var orders = await query.ToListAsync();

            return _mapper.Map<List<AdminOrderViewModel>>(orders);
        }

        public async Task<OrderDetailsViewModel> GetOrderDetailsViewModelByIdAsync(int orderId)
        {
            var order = await _orderRepo.GetOrderByIdWithDetails(orderId);

            if (order == null)
                return null;

            return new OrderDetailsViewModel
            {
                OrderId = order.Id,
                UserEmail = order.Email,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Address = order.Address,
                Governorate = order.SelectedGovernorate.ToString(),
                Items = order.OrderDetails.Select(od => new OrderItemViewModel
                {
                    ProductId = od.Product.Id,
                    ProductName = od.Product.Name,
                    ImageUrl = od.Product.ImageUrl,
                    Price = od.Price,
                    Quantity = od.Quantity
                }).ToList()
            };
        }


    }
}
