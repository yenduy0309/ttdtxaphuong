using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.Data;
using ttxaphuong.DTO.Introduces;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Introduce;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class Categories_introduceService : ICategories_introduceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Categories_introduceService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Categories_introduceDTO>> GetAllCategories_introducesAsync()
        {
            try
            {
                var services = await _context.Categories_Introduces.ToListAsync();
                return _mapper.Map<IEnumerable<Categories_introduceDTO>>(services);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy tất cả tên danh mục giới thiệu", ex);
            }
        }

        public async Task<Categories_introduceDTO> GetCategories_introducesByIdAsync(int id)
        {
            try
            {
                var transportType = await _context.Categories_Introduces.FirstOrDefaultAsync(b => b.Id_cate_introduce == id);
                return transportType == null ? throw new NotFoundException("Không tìm thấy phương tiện") : _mapper.Map<Categories_introduceDTO>(transportType);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin tên danh mục giới thiệu", ex);
            }
        }

        public async Task<Categories_introduceDTO> CreateCategories_introducesAsync(Categories_introduceDTO transportType)
        {
            try
            {
                var transportType1 = _mapper.Map<Categories_introduceModel>(transportType);

                _context.Categories_Introduces.Add(transportType1);
                await _context.SaveChangesAsync();

                transportType1.Id_cate_introduce = transportType1.Id_cate_introduce;

                return _mapper.Map<Categories_introduceDTO>(transportType1);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi danh mục mới" + ex.Message, ex);
            }
        }

        public async Task<Categories_introduceDTO> UpdateCategories_introducesAsync(int id, Categories_introduceDTO abcDTO)
        {
            try
            {
                var model = await _context.Categories_Introduces.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy danh mục");

                if (string.IsNullOrWhiteSpace(abcDTO.Name_cate_introduce))
                {
                    throw new BadRequestException("Tên danh mục không được để trống.");
                }

                model.Name_cate_introduce = abcDTO.Name_cate_introduce;

                await _context.SaveChangesAsync();

                return _mapper.Map<Categories_introduceDTO>(model);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật tên danh mục: " + ex.Message, ex);
            }
        }

        public async Task<object> DeleteCategories_introducesAsync(int id)
        {
            try
            {
                var models = await _context.Categories_Introduces.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy tên danh mục");

                _context.Categories_Introduces.Remove(models);
                await _context.SaveChangesAsync();

                return new { message = "Xóa tên danh mục thành công!" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa danh mục " + ex.Message, ex);
            }
        }

        public async Task<List<Categories_introduceDTO>> GetAllCategories()
        {
            return await _context.Categories_Introduces
            .Select(c => new Categories_introduceDTO
            {
                Id_cate_introduce = c.Id_cate_introduce,
                Name_cate_introduce = c.Name_cate_introduce
            })
            .ToListAsync();
        }



        /****************************************************************/
        public async Task<Categories_introduceDTO> GetCategoryById(int id)
        {
            var namecategory = await _context.Categories_Introduces
                .Where(c => c.Id_cate_introduce == id)
                .Select(c => c.Name_cate_introduce) // Chỉ lấy trường Name của danh mục
                .FirstOrDefaultAsync();

            // Tạo đối tượng Categories_introduceDTO và gán giá trị
            return new Categories_introduceDTO
            {
                Name_cate_introduce = namecategory // Trả về đối tượng DTO chứa tên danh mục
            };
        }

    }
}
