using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ttxaphuong.Models.Introduce
{
    public class IntroduceModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_introduce { get; set; }
        public string? Name_introduce { get; set; }
        public int Id_cate_introduce { get; set; }
        [ForeignKey("Id_cate_introduce")]
        public Categories_introduceModel? Categories_IntroduceModel { get; set; }
        public string? FormatHTML { get; set; }
        public string? Description { get; set; }
        public DateTime? Create_at { get; set; }
        public string? Image_url { get; set; }
    }
}
