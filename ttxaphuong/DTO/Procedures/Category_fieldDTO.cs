using ttxaphuong.DTO.Documents;
using ttxaphuong.Models.Procedures;

namespace ttxaphuong.DTO.Category_field
{
    public class Category_fieldDTO
    {
        public int? Id_Field { get; set; }

        public string? Name_Field { get; set; }

        public int? FielParentId { get; set; }
        public List<Category_fieldDTO> Children { get; set; } = new List<Category_fieldDTO>();
    }
}
