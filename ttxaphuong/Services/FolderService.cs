using AutoMapper;
using ttxaphuong.Data;
using ttxaphuong.Interfaces;
using WebDoAn2.Exceptions;
using ttxaphuong.DTO.Uploads;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.Models.Loads;

namespace ttxaphuong.Services
{
    public class FolderService : IFolderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FolderService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FolderDTO>> GetAllFolderAsync()
        {
            try
            {
                var allCategories = await _context.Folders.ToListAsync();

                var topLevelCategories = allCategories.Where(c => c.ParentId == null).ToList();

                foreach (var category in topLevelCategories)
                {
                    BuildCategoryHierarchy(category, allCategories);
                }

                return _mapper.Map<IEnumerable<FolderDTO>>(topLevelCategories);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách danh mục hình ảnh: " + ex.Message, ex);
            }
        }

        public async Task<FolderDTO> GetFolderByIdAsync(int id)
        {
            try
            {
                var folder = await _context.Folders
                    .FirstOrDefaultAsync(b => b.Id_folder == id);

                if (folder == null)
                {
                    throw new NotFoundException("Không tìm thấy danh mục hình ảnh với Id: " + id);
                }

                var allCategories = await _context.Folders.ToListAsync();

                BuildCategoryHierarchy(folder, allCategories);

                return _mapper.Map<FolderDTO>(folder);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin danh mục: " + ex.Message, ex);
            }
        }

        private void BuildCategoryHierarchy(FolderModel parent, List<FolderModel> allCategories)
        {
            parent.Children = allCategories.Where(c => c.ParentId == parent.Id_folder).ToList();
            foreach (var child in parent.Children)
            {
                BuildCategoryHierarchy(child, allCategories);
            }
        }

        public async Task<FolderDTO> CreateFolderAsync(FolderDTO folder)
        {
            try
            {
                if (string.IsNullOrEmpty(folder.Name_folder))
                {
                    throw new BadRequestException("Tên danh mục không được để trống.");
                }

                if (folder.ParentId.HasValue)
                {
                    var parentExists = await _context.Folders.AnyAsync(c => c.Id_folder == folder.ParentId.Value);
                    if (!parentExists)
                    {
                        throw new NotFoundException($"Danh mục cha với Id {folder.ParentId.Value} không tồn tại.");
                    }
                }

                var categoryModel = _mapper.Map<FolderModel>(folder);

                _context.Folders.Add(categoryModel);
                await _context.SaveChangesAsync();

                var createdCategory = await _context.Folders
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_folder == categoryModel.Id_folder);

                return _mapper.Map<FolderDTO>(createdCategory);
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
                throw new Exception("Có lỗi xảy ra khi tạo danh mục hình ảnh mới: " + ex.Message, ex);
            }
        }

        public async Task<FolderDTO> UpdateFolderAsync(int id, FolderDTO folderDTO)
        {
            try
            {
                var model = await _context.Folders
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_folder == id)
                    ?? throw new NotFoundException($"Không tìm thấy danh mục hình ảnh với Id: {id}");

                if (string.IsNullOrWhiteSpace(folderDTO.Name_folder))
                {
                    throw new BadRequestException("Tên danh mục hình ảnh không được để trống.");
                }

                model.Name_folder = folderDTO.Name_folder;

                if (folderDTO.ParentId != model.ParentId)
                {
                    if (folderDTO.ParentId.HasValue)
                    {
                        var parentExists = await _context.Folders.AnyAsync(c => c.Id_folder == folderDTO.ParentId.Value);
                        if (!parentExists)
                        {
                            throw new NotFoundException($"Danh mục cha với Id {folderDTO.ParentId.Value} không tồn tại.");
                        }
                        if (await IsDescendant(id, folderDTO.ParentId.Value))
                        {
                            throw new BadRequestException("Không thể đặt danh mục làm cha của chính nó hoặc con của nó.");
                        }
                    }
                    model.ParentId = folderDTO.ParentId; // Cho phép ParentId = null
                }

                await _context.SaveChangesAsync();

                var allCategories = await _context.Folders.ToListAsync();
                BuildCategoryHierarchy(model, allCategories);

                return _mapper.Map<FolderDTO>(model);
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
                throw new Exception("Có lỗi xảy ra khi cập nhật danh mục tin tức: " + ex.Message, ex);
            }
        }

        private async Task<bool> IsDescendant(int id, int parentId)
        {
            var current = await _context.Folders.FirstOrDefaultAsync(c => c.Id_folder == parentId);
            while (current != null)
            {
                if (current.Id_folder == id)
                {
                    return true;
                }
                current = await _context.Folders.FirstOrDefaultAsync(c => c.Id_folder == current.ParentId);
            }
            return false;
        }

        // Xóa từng danh mục
        public async Task<object> DeleteFolderAsync(int id)
        {
            try
            {
                var category = await _context.Folders
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_folder == id)
                    ?? throw new NotFoundException($"Không tìm thấy danh mục hình ảnh với Id: {id}");

                foreach (var child in category.Children)
                {
                    child.ParentId = category.ParentId; // Chuyển lên cấp cha của danh mục bị xóa
                }

                // Lấy danh sách ảnh trong thư mục
                var imagesInFolder = await _context.PostImages.Where(img => img.Id_folder == id).ToListAsync();

                // Xóa ảnh khỏi thư mục vật lý
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", category.Name_folder);
                if (Directory.Exists(uploadsFolder))
                {
                    Directory.Delete(uploadsFolder, true); // Xóa toàn bộ thư mục và các file bên trong
                }

                // Xóa ảnh khỏi database
                _context.PostImages.RemoveRange(imagesInFolder);

                _context.Folders.Remove(category);
                await _context.SaveChangesAsync();

                return new { message = "Xóa danh mục hình ảnh thành công, các menu con đã được chuyển lên cấp cao hơn!" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa danh mục tin tức: " + ex.Message, ex);
            }
        }
    }
}
