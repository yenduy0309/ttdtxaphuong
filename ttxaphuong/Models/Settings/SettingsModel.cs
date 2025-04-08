using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ttxaphuong.Models.Settings
{
    public class SettingsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_settings { get; set; }
        public string? Key_name { get; set; }
        public string? Description { get; set; }
    }
}
