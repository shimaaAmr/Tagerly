﻿namespace Tagerly.ViewModels.AdminViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public bool IsActive { get; set; } // ← علشان نعرض حالة التفعيل
    }
}
