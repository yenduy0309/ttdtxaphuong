using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ttxaphuong.Models.Accounts
{
    public class PermissionsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("AccountsModel")]
        public int ManagerId { get; set; }

        public bool CanAddUser { get; set; } = false;
        public bool CanEditUser { get; set; } = false;
        public bool CanDeleteUser { get; set; } = false;
        public bool CanViewUsers { get; set; } = true;
        public bool CanManageRoles { get; set; } = false;
        public bool CanManagePermissions { get; set; } = false;
        public virtual AccountsModel Manager { get; set; }
    }
}
