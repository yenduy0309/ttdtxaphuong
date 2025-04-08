namespace ttxaphuong.Models.Accounts
{
    public class AssignPermissionsModel
    {
        public int ManagerId { get; set; }
        public bool CanAddUser { get; set; } = false;
        public bool CanEditUser { get; set; } = false;
        public bool CanDeleteUser { get; set; } = false;
        public bool CanViewUsers { get; set; } = true;
        public bool CanManageRoles { get; set; } = false;
        public bool CanManagePermissions { get; set; } = false;

    }
}
