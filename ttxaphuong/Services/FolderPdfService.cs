using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.Data;
using ttxaphuong.DTO.Uploads;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Loads;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class FolderPdfService : IFolderPdfService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FolderPdfService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FolderPdfDTO>> GetAllFolderPdfAsync()
        {
            try
            {
                var allCategories = await _context.FolderPdfs.ToListAsync();

                var topLevelCategories = allCategories.Where(c => c.ParentId == null).ToList();

                foreach (var category in topLevelCategories)
                {
                    BuildCategoryHierarchy(category, allCategories);
                }

                return _mapper.Map<IEnumerable<FolderPdfDTO>>(topLevelCategories);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách danh mục pdf: " + ex.Message, ex);
            }
        }

        public async Task<FolderPdfDTO> GetFolderPdfByIdAsync(int id)
        {
            try
            {
                var folder = await _context.FolderPdfs
                    .FirstOrDefaultAsync(b => b.Id_folder_pdf == id);

                if (folder == null)
                {
                    throw new NotFoundException("Không tìm thấy danh mục pdf với Id: " + id);
                }

                var allCategories = await _context.FolderPdfs.ToListAsync();

                BuildCategoryHierarchy(folder, allCategories);

                return _mapper.Map<FolderPdfDTO>(folder);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin thư mục: " + ex.Message, ex);
            }
        }

        private void BuildCategoryHierarchy(FolderPdfModel parent, List<FolderPdfModel> allCategories)
        {
            parent.Children = allCategories.Where(c => c.ParentId == parent.Id_folder_pdf).ToList();
            foreach (var child in parent.Children)
            {
                BuildCategoryHierarchy(child, allCategories);
            }
        }

        //public async Task<FolderPdfDTO> CreateFolderPdfAsync(FolderPdfDTO folderPdf)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(folderPdf.Name_folder))
        //        {
        //            throw new BadRequestException("Tên thư mục không được để trống.");
        //        }

        //        if (folderPdf.ParentId.HasValue)
        //        {
        //            var parentExists = await _context.FolderPdfs.AnyAsync(c => c.Id_folder_pdf == folderPdf.ParentId.Value);
        //            if (!parentExists)
        //            {
        //                throw new NotFoundException($"Danh mục cha với Id {folderPdf.ParentId.Value} không tồn tại.");
        //            }
        //        }

        //        var categoryModel = _mapper.Map<FolderPdfModel>(folderPdf);

        //        _context.FolderPdfs.Add(categoryModel);
        //        await _context.SaveChangesAsync();

        //        var createdCategory = await _context.FolderPdfs
        //            .Include(c => c.Children)
        //            .FirstOrDefaultAsync(c => c.Id_folder_pdf == categoryModel.Id_folder_pdf);

        //        return _mapper.Map<FolderPdfDTO>(createdCategory);
        //    }
        //    catch (NotFoundException)
        //    {
        //        throw;
        //    }
        //    catch (BadRequestException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Có lỗi xảy ra khi tạo danh mục pdf mới: " + ex.Message, ex);
        //    }
        //}

        public async Task<FolderPdfDTO> CreateFolderPdfAsync(FolderPdfDTO folderPdf)
        {
            try
            {
                if (string.IsNullOrEmpty(folderPdf.Name_folder))
                {
                    throw new BadRequestException("Tên thư mục không được để trống.");
                }

                // Kiểm tra nếu thư mục có parentId (có thư mục cha)
                if (folderPdf.ParentId.HasValue)
                {
                    var parentExists = await _context.FolderPdfs.AnyAsync(c => c.Id_folder_pdf == folderPdf.ParentId.Value);
                    if (!parentExists)
                    {
                        throw new NotFoundException($"Danh mục cha với Id {folderPdf.ParentId.Value} không tồn tại.");
                    }
                }

                var categoryModel = _mapper.Map<FolderPdfModel>(folderPdf);

                _context.FolderPdfs.Add(categoryModel);
                await _context.SaveChangesAsync();

                var createdCategory = await _context.FolderPdfs
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_folder_pdf == categoryModel.Id_folder_pdf);

                return _mapper.Map<FolderPdfDTO>(createdCategory);
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
                throw new Exception("Có lỗi xảy ra khi tạo danh mục pdf mới: " + ex.Message, ex);
            }
        }


        public async Task<FolderPdfDTO> UpdateFolderPdfAsync(int id, FolderPdfDTO folderDTO)
        {
            try
            {
                var model = await _context.FolderPdfs
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_folder_pdf == id)
                    ?? throw new NotFoundException($"Không tìm thấy danh mục pdf với Id: {id}");

                if (string.IsNullOrWhiteSpace(folderDTO.Name_folder))
                {
                    throw new BadRequestException("Tên danh mục pdf không được để trống.");
                }

                model.Name_folder = folderDTO.Name_folder;

                if (folderDTO.ParentId != model.ParentId)
                {
                    if (folderDTO.ParentId.HasValue)
                    {
                        var parentExists = await _context.FolderPdfs.AnyAsync(c => c.Id_folder_pdf == folderDTO.ParentId.Value);
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

                var allCategories = await _context.FolderPdfs.ToListAsync();
                BuildCategoryHierarchy(model, allCategories);

                return _mapper.Map<FolderPdfDTO>(model);
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
                throw new Exception("Có lỗi xảy ra khi cập nhật danh mục pdf: " + ex.Message, ex);
            }
        }

        private async Task<bool> IsDescendant(int id, int parentId)
        {
            var current = await _context.FolderPdfs.FirstOrDefaultAsync(c => c.Id_folder_pdf == parentId);
            while (current != null)
            {
                if (current.Id_folder_pdf == id)
                {
                    return true;
                }
                current = await _context.FolderPdfs.FirstOrDefaultAsync(c => c.Id_folder_pdf == current.ParentId);
            }
            return false;
        }

        // Xóa từng danh mục
        public async Task<object> DeleteFolderPdfAsync(int id)
        {
            try
            {
                var category = await _context.FolderPdfs
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_folder_pdf == id)
                    ?? throw new NotFoundException($"Không tìm thấy danh mục pdf với Id: {id}");

                foreach (var child in category.Children)
                {
                    child.ParentId = category.ParentId; // Chuyển lên cấp cha của danh mục bị xóa
                }

                // Lấy danh sách ảnh trong thư mục
                var pdfInFolder = await _context.PostPdfs.Where(img => img.Id_Pdf == id).ToListAsync();

                // Xóa ảnh khỏi thư mục vật lý
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Pdf", category.Name_folder);
                if (Directory.Exists(uploadsFolder))
                {
                    Directory.Delete(uploadsFolder, true); // Xóa toàn bộ thư mục và các file bên trong
                }

                // Xóa ảnh khỏi database
                _context.PostPdfs.RemoveRange(pdfInFolder);

                _context.FolderPdfs.Remove(category);
                await _context.SaveChangesAsync();

                return new { message = "Xóa danh mục pdf thành công, các menu con đã được chuyển lên cấp cao hơn!" };
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

