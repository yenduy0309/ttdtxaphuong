using System.ComponentModel.DataAnnotations.Schema;

namespace ttxaphuong.DTO.Accounts
{
    public class PermissionsDTO
    {
        public int? Id { get; set; }
        public int? ManagerId { get; set; }
        public bool CanAddUser { get; set; } = false;  // Có thể thêm
        public bool CanEditUser { get; set; } = false; // Có thể sửa
        public bool CanDeleteUser { get; set; } = false; // Có thể xóa
        public bool CanViewUsers { get; set; } = true; // Có thể xem danh sách
        public bool CanManageRoles { get; set; } = false;  // Có thể thay đổi vai trò
        public bool CanManagePermissions { get; set; } = false; // Có thể quản lý quyền hạn
    }
}
