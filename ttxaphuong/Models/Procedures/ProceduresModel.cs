using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ttxaphuong.Models.Accounts;

namespace ttxaphuong.Models.Procedures
{
    public class ProceduresModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_procedures { get; set; }
        public string? Id_thutuc { get; set; }
        public string? Name_procedures { get; set; }
        public int Id_Field { get; set; }
        [ForeignKey("Id_Field")]
        public Categogy_fieldModel? Categogy_Field { get; set; }
        public string? Description { get; set; }
        public DateTime? Date_issue { get; set; }
        public DateTime Create_at { get; set; } = DateTime.Now;
        public int Id_account { get; set; }
        [ForeignKey("Id_account")]
        public AccountsModel Accounts { get; set; }
        public string? FormatText { get; set; }
        public bool IsVisible { get; set; } = true; // Mặc định là hiển thị

    }
}
