
using ttxaphuong.DTO.Introduces;

namespace ttxaphuong.Interfaces
{
    public interface IIntroduceService
    {
        Task<IEnumerable<IntroduceDTO>> GetAllIntroduceAsync();
        Task<IntroduceDTO> GetIntroduceByIdAsync(int id);
        Task<IntroduceDTO> CreateIntroduceAsync(IntroduceDTO abc);
        Task<IntroduceDTO> UpdateIntroduceAsync(int id, IntroduceDTO abc);
        Task<object> DeleteIntroduceAsync(int id);
        Task<string> UploadImageIntroduceAsync(IFormFile imageFile);

        /******************************/
        Task<List<IntroduceDTO>> GetIntroByNameCategogyAsync(string nameCategogy);
        Task<IntroduceDTO> GetIntroByNameAsync(string name);
        Task<List<IntroduceDTO>> GetRelatedIntroAsync(int id);
    }
}
