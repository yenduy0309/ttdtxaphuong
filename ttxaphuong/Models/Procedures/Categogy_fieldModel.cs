using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ttxaphuong.Models.Procedures
{
    public class Categogy_fieldModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Field { get; set; }
        public string? Name_Field { get; set; }

        public int? FielParentId { get; set; }
        [ForeignKey("FielParentId")]
        public Categogy_fieldModel? Categogy_Field { get; set; }
        public List<Categogy_fieldModel> Children { get; set; } = new List<Categogy_fieldModel>();
    }
}
