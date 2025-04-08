using ttxaphuong.DTO.News_events;
using ttxaphuong.DTO.Procedures;

namespace ttxaphuong.Interfaces
{
    public interface IProceduresService
    {
        //Task<IEnumerable<ProceduresDTO>> GetProcedures1();
        Task<IEnumerable<ProceduresDTO>> GetAllProceduresAsync(bool? isVisible = null);
        Task<object> SetVisibility(int id, bool isVisible);
        Task<ProceduresDTO> CreateProceduresAsync(ProceduresDTO procedures);
        Task<ProceduresDTO> GetProceduresByIdAsync(int id);
        Task<ProceduresDTO> UpdateProceduresAsync(int id, ProceduresDTO procedures);
        Task<object> DeleteProceduresAsync(int id);

        /*********************************************************************/
        Task<List<ProceduresDTO>> GetProceduresByIdField(int categoryId);
        Task<ProceduresDTO> GetProceduresById_thutuc(string id_thutuc);
    }
}
