using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.Data;
using ttxaphuong.DTO.Introduces;
using ttxaphuong.DTO.News_events;
using ttxaphuong.DTO.Procedures;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.News_events;
using ttxaphuong.Models.Procedures;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class ProceduresService : IProceduresService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProceduresService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public async Task<IEnumerable<ProceduresDTO>> GetProcedures1()
        //{
        //    try
        //    {
        //        var procedures = await _context.Procedures
        //            .Include(ne => ne.Categogy_Field)
        //            .Include(ne => ne.Accounts).Where(h => h.IsVisible == true)
        //            .ToListAsync();

        //        return _mapper.Map<IEnumerable<ProceduresDTO>>(procedures);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Có lỗi xảy ra khi lấy danh sách thủ tục hành chính: " + ex.Message, ex);
        //    }
        //}

        public async Task<IEnumerable<ProceduresDTO>> GetAllProceduresAsync(bool? isVisible = null)
        {
            var query = _context.Procedures.AsQueryable();

            if (isVisible.HasValue)
            {
                query = query.Where(n => n.IsVisible == isVisible.Value);
            }

            var procedures = await query
                .Include(ne => ne.Categogy_Field)
                .Include(ne => ne.Accounts)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProceduresDTO>>(procedures);
        }

        public async Task<object> SetVisibility(int id, bool isVisible)
        {
            try
            {
                var procedures = await _context.Procedures.FindAsync(id);
                if (procedures == null)
                {
                    throw new NotFoundException("Không tìm thấy thủ tục hành chính");
                }

                procedures.IsVisible = isVisible;
                await _context.SaveChangesAsync();

                return new { message = "Cập nhật trạng thái thành công", isVisible };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật trạng thái hiển thị", ex);
            }
        }

        private void BuildCategoryHierarchy(Categogy_fieldModel parent, List<Categogy_fieldModel> allCategories)
        {
            parent.Children = allCategories.Where(c => c.FielParentId == parent.Id_Field).ToList();
            foreach (var child in parent.Children)
            {
                BuildCategoryHierarchy(child, allCategories);
            }
        }

        public async Task<ProceduresDTO> CreateProceduresAsync(ProceduresDTO procedures)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(procedures.Name_procedures))
                    throw new BadRequestException("Tên thủ tục hành chính không được để trống.");

                var proceduresModel = _mapper.Map<ProceduresModel>(procedures);

                _context.Procedures.Add(proceduresModel);
                await _context.SaveChangesAsync();

                proceduresModel.Id_procedures = proceduresModel.Id_procedures;

                return _mapper.Map<ProceduresDTO>(proceduresModel);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi tạo thủ tục hành chính mới: " + ex.Message, ex);
            }
        }

        public async Task<ProceduresDTO> UpdateProceduresAsync(int id, ProceduresDTO procedures)
        {
            try
            {
                var existingNewsEvent = await _context.Procedures.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy thủ tục hành chính");

                if (string.IsNullOrWhiteSpace(procedures.Name_procedures))
                    throw new BadRequestException("Tên thủ tục hành chính không được để trống.");

                existingNewsEvent.Name_procedures = procedures.Name_procedures;
                existingNewsEvent.Id_thutuc = procedures.Id_thutuc;
                existingNewsEvent.Id_Field = procedures.Id_Field;
                existingNewsEvent.Id_account = procedures.Id_account;
                existingNewsEvent.Description = procedures.Description;
                existingNewsEvent.Date_issue = procedures.Date_issue;

                //if (procedures.Date_issue.HasValue && procedures.Date_issue.Value != DateTime.MinValue)
                //{
                //    existingNewsEvent.Date_issue = procedures.Date_issue.Value;
                //}

                existingNewsEvent.FormatText = procedures.FormatText;

                await _context.SaveChangesAsync();
                return _mapper.Map<ProceduresDTO>(existingNewsEvent);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật thủ tục hành chính", ex);
            }
        }

        public async Task<object> DeleteProceduresAsync(int id)
        {
            try
            {
                var procedures = await _context.Procedures.FindAsync(id)
                            ?? throw new NotFoundException("Không tìm thủ tục hành chính");
                _context.Procedures.Remove(procedures);
                await _context.SaveChangesAsync();
                return new { message = "Xóa thủ tục hành chính thành công" };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa thủ tục hành chính", ex);
            }
        }

        public async Task<ProceduresDTO> GetProceduresByIdAsync(int id)
        {
            try
            {
                var abc = await _context.Procedures
                    .Where (b => b.IsVisible == true)
                    .FirstOrDefaultAsync(b => b.Id_procedures == id);
                return abc == null ? throw new NotFoundException("Không tìm thấy thủ tục hành chính") : _mapper.Map<ProceduresDTO>(abc);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin thủ tục hành chính", ex);
            }
        }

        /// ///////////////////////////////////////////////
        public async Task<List<ProceduresDTO>> GetProceduresByIdField(int idField)
        {
            try
            {
                var procedures = await _context.Procedures
                    .Where(b => b.Id_Field == idField && b.IsVisible == true)
                    .ToListAsync();  // Chuyển đổi thành danh sách;
                if (procedures == null || !procedures.Any())
                {
                    //throw new NotFoundException("Không tìm thấy thủ tục hành chính");
                    return new List<ProceduresDTO>();
                }
                return _mapper.Map<List<ProceduresDTO>>(procedures);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return new List<ProceduresDTO>();
            }
        }

        public async Task<ProceduresDTO> GetProceduresById_thutuc(string id_thutuc)
        {
            try
            {
                var procedures = await _context.Procedures
                    .Where(b => b.Id_thutuc == id_thutuc && b.IsVisible == true)
                    .FirstOrDefaultAsync(); 
                if (procedures == null)
                {
                    return null;
                }
                return _mapper.Map<ProceduresDTO>(procedures);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return new ProceduresDTO();
            }
        }
    }
}
