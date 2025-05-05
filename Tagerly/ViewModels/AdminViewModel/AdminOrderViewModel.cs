namespace Tagerly.ViewModels.AdminViewModel
{
    public class AdminOrderViewModel
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
