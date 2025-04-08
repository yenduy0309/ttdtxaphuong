using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ttxaphuong.Models.Introduce
{
    public class Categories_introduceModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_cate_introduce { get; set; }
        public string? Name_cate_introduce { get; set; }

    }
}
