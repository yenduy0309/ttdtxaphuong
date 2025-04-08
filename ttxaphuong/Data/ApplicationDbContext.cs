
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ttxaphuong.Models.Accounts;
using ttxaphuong.Models.Documents;
using ttxaphuong.Models.Feedbacks;
using ttxaphuong.Models.Introduce;
using ttxaphuong.Models.Loads;
using ttxaphuong.Models.News_events;
using ttxaphuong.Models.Procedures;
using ttxaphuong.Models.Settings;

namespace ttxaphuong.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<AccountsModel> Accounts { get; set; }
        public DbSet<PermissionsModel> Permissions { get; set; }

        /**************************************/
        public DbSet<CategoriesModel> Categories { get; set; }
        public DbSet<News_eventsModel> News_Events { get; set; }
        public DbSet<DocumentModel> Documents { get; set; }
        public DbSet<Category_documentsModel> Category_Documents { get; set; }
        public DbSet<FeedbacksModel> Feedbacks { get; set; }
        public DbSet<ProceduresModel> Procedures { get; set; }
        public DbSet<Categogy_fieldModel> Categogy_Fields { get; set; }
        public DbSet<SettingsModel> Settings { get; set; }
        public DbSet<Categories_introduceModel> Categories_Introduces { get; set; }
        public DbSet<IntroduceModel> Introduces { get; set; }

        /**************************************/
        public DbSet<FolderModel> Folders { get; set; }
        public DbSet<PostImageModel> PostImages { get; set; }

        /**************************************/
        public DbSet<FolderPdfModel> FolderPdfs { get; set; }
        public DbSet<PostPdfModel> PostPdfs { get; set; }

        /**************************************/
        public DbSet<WebsiteSettingsModel> WebsiteSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*******************************************/
            // Seed data for Accounts
            modelBuilder.Entity<AccountsModel>().HasData(
                new AccountsModel
                {
                    Id_account = 1,
                    Username = "ThiHoa",
                    Password = "03092003",
                    Email = "hoanguyentranthi32@gmail.com",
                    Fullname = "Nguyễn Trần Thị Hòa",
                    Role = "Manager",
                    Status = "IsActive"
                },
                new AccountsModel
                {
                    Id_account = 2,
                    Username = "HoaThi",
                    Password = "123456789",
                    Email = "TuiLaHoa25@gmail.com",
                    Fullname = "Nguyễn Hòa Trần Thị",
                    Role = "Admin",
                    Status = "IsActive"
                }
            );

            modelBuilder.Entity<PermissionsModel>().HasData(
                new PermissionsModel
                {
                    Id = 1,
                    ManagerId = 2, // Gán quyền cho Admin
                    CanAddUser = true,
                    CanEditUser = true,
                    CanDeleteUser = true,
                    CanViewUsers = true,
                    CanManageRoles = true,
                    CanManagePermissions = true
                },
                new PermissionsModel
                {
                    Id = 2,
                    ManagerId = 1, // Gán quyền mặc định cho Manager
                    CanAddUser = false,
                    CanEditUser = false,
                    CanDeleteUser = false,
                    CanViewUsers = true,
                    CanManageRoles = false,
                    CanManagePermissions = false
                }
            );

            // Seed data for Feedbacks   
            modelBuilder.Entity<FeedbacksModel>().HasData(
                new FeedbacksModel
                {
                    Id_feedbacks = 1,
                    Fullname = "Nguyễn Văn A",
                    Email = "NguyenVanA@gmail.com",
                    Content = "Nội dung 1",
                    Received_at = new DateTime(2025, 2, 14),
                    Phone = "0772962490",
                    Status = "Pending"
                },
                new FeedbacksModel
                {
                    Id_feedbacks = 2,
                    Fullname = "Nguyễn Văn B",
                    Email = "NguyenVanB@gmail.com",
                    Content = "Nội dung 2",
                    Received_at = new DateTime(2025, 2, 14),
                    Phone = "0772962490",
                    Status = "Approved"
                },
                new FeedbacksModel
                {
                    Id_feedbacks = 3,
                    Fullname = "Nguyễn Văn C",
                    Email = "NguyenVanC@gmail.com",
                    Content = "Nội dung 3",
                    Received_at = new DateTime(2025, 2, 14),
                    Phone = "0772962490",
                    Status = "Rejected"
                },
                new FeedbacksModel
                {
                    Id_feedbacks = 4,
                    Fullname = "Nguyễn Thị Hoa",
                    Email = "NguyenVanC@gmail.com",
                    Content = "Nội dung 4",
                    Received_at = new DateTime(2025, 2, 8),
                    Phone = "0772962490",
                    Status = "Pending"
                }
            );

            // Seed data for Categogy_field     
            modelBuilder.Entity<Categogy_fieldModel>().HasData(
                new Categogy_fieldModel
                {
                    Id_Field = 1,
                    Name_Field = "Danh mục lĩnh vực 1",
                    FielParentId = null
                },
                new Categogy_fieldModel
                {
                    Id_Field = 2,
                    Name_Field = "Danh mục lĩnh vực 2",
                    FielParentId = 1
                },
                new Categogy_fieldModel
                {
                    Id_Field = 3,
                    Name_Field = "Danh mục lĩnh vực 3",
                    FielParentId = 2
                },
                new Categogy_fieldModel
                {
                    Id_Field = 4,
                    Name_Field = "Danh mục lĩnh vực 4",
                    FielParentId = null
                },
                new Categogy_fieldModel
                {
                    Id_Field = 5,
                    Name_Field = "Danh mục lĩnh vực 5",
                    FielParentId = 2
                },
                new Categogy_fieldModel
                {
                    Id_Field = 6,
                    Name_Field = "Danh mục lĩnh vực 6",
                    FielParentId = 1
                }
            );

            // Seed data for Procedures    
            modelBuilder.Entity<ProceduresModel>().HasData(
                new ProceduresModel
                {
                    Id_procedures = 1,
                    Id_thutuc = "N1",
                    Name_procedures = "Tên thủ tục 1",
                    Id_Field = 1,
                    Description = "Mô tả 1",
                    Create_at = new DateTime(2025, 3, 6),
                    Id_account = 2,
                    Date_issue = new DateTime(2025, 4, 6),
                    FormatText = "abcd",
                    IsVisible = true
                },
                new ProceduresModel
                {
                    Id_procedures = 2,
                    Id_thutuc = "N2",
                    Name_procedures = "Tên thủ tục 2",
                    Id_Field = 2,
                    Description = "Mô tả 2",
                    Create_at = new DateTime(2025, 3, 6),
                    Id_account = 2,
                    Date_issue = new DateTime(2025, 3, 2),
                    FormatText = "abcd",
                    IsVisible = true
                },
                new ProceduresModel
                {
                    Id_procedures = 3,
                    Id_thutuc = "N3",
                    Name_procedures = "Tên thủ tục 3",
                    Id_Field = 2,
                    Description = "Mô tả 3",
                    Create_at = new DateTime(2025, 3, 6),
                    Id_account = 1,
                    Date_issue = new DateTime(2025, 5, 10),
                    FormatText = "abcd",
                    IsVisible = true
                }
            );

            // Seed data for Settings     
            modelBuilder.Entity<SettingsModel>().HasData(
                new SettingsModel
                {
                    Id_settings = 1,
                    Key_name = "Cấu hình 1",
                    Description = "Mô tả cấu hình 1"
                },
                new SettingsModel
                {
                    Id_settings = 2,
                    Key_name = "Cấu hình 2",
                    Description = "Mô tả cấu hình 2"
                }
            );

            // Seed data for Categories_parent      
            modelBuilder.Entity<CategoriesModel>().HasData(
                new CategoriesModel
                {
                    Id_categories = 1,
                    Name_category = " Danh mục A",
                    ParentId = null
                },
                new CategoriesModel
                {
                    Id_categories = 2,
                    Name_category = " Danh mục A1",
                    ParentId = 1
                },
                new CategoriesModel
                {
                    Id_categories = 3,
                    Name_category = " Danh mục A1.1",
                    ParentId = 2
                },
                new CategoriesModel
                {
                    Id_categories = 4,
                    Name_category = " Danh mục A2",
                    ParentId = 1
                },
                new CategoriesModel
                {
                    Id_categories = 5,
                    Name_category = " Danh mục B",
                    ParentId = null
                },
                new CategoriesModel
                {
                    Id_categories = 6,
                    Name_category = " Danh mục B1",
                    ParentId = 5
                },
                new CategoriesModel
                {
                    Id_categories = 7,
                    Name_category = " Danh mục B1.1",
                    ParentId = 6
                },
                new CategoriesModel
                {
                    Id_categories = 8,
                    Name_category = " Danh mục B1.2",
                    ParentId = 6
                }
            );

            // Seed data for Category_documents_parentModel      
            modelBuilder.Entity<Category_documentsModel>().HasData(
                new Category_documentsModel
                {
                    Id_category_document = 1,
                    Name_category_document = "Danh mục VB",
                    DocumentParentId = null
                },
                new Category_documentsModel
                {
                    Id_category_document = 2,
                    Name_category_document = "Danh mục VB1",
                    DocumentParentId = 1
                },
                new Category_documentsModel
                {
                    Id_category_document = 3,
                    Name_category_document = "Danh mục VB1.1",
                    DocumentParentId = 2
                },
                new Category_documentsModel
                {
                    Id_category_document = 4,
                    Name_category_document = "Danh mục VB2",
                    DocumentParentId = 1
                },
                new Category_documentsModel
                {
                    Id_category_document = 5,
                    Name_category_document = "Danh mục VB3",
                    DocumentParentId = null
                },
                new Category_documentsModel
                {
                    Id_category_document = 6,
                    Name_category_document = "Danh mục VB3.1",
                    DocumentParentId = 5
                },
                new Category_documentsModel
                {
                    Id_category_document = 7,
                    Name_category_document = "Danh mục VB3.1",
                    DocumentParentId = 5
                }
            );

            // Seed data cho News_Events
            modelBuilder.Entity<News_eventsModel>().HasData(
                new News_eventsModel
                {
                    Id_newsevent = 1,
                    Title = "Tin tức & Sự kiện 1",
                    Description_short = "Nội dung tin tức 1",
                    Content = "Chi tiết nội dung 1",
                    Id_categories = 2,
                    Id_account = 2,
                    Image = "Anh1.jpg",
                    Create_at = new DateTime(2022, 4, 1),
                    Formatted_content = "abc",
                    View = 26,
                    IsVisible = true
                },
                new News_eventsModel
                {
                    Id_newsevent = 2,
                    Title = "Tin tức & Sự kiện 2",
                    Description_short = "Nội dung tin tức 2",
                    Content = "Chi tiết nội dung 2",
                    Id_categories = 3,
                    Id_account = 1,
                    Image = "Anh2.jpg",
                    Create_at = new DateTime(2023, 4, 1),
                    Formatted_content = "abc",
                    View = 10,
                    IsVisible = true
                },
                new News_eventsModel
                {
                    Id_newsevent = 3,
                    Title = "Tin tức & Sự kiện 3",
                    Description_short = "Nội dung tin tức 3",
                    Content = "Chi tiết nội dung 3",
                    Id_categories = 2,
                    Id_account = 2,
                    Image = "Anh3.jpg",
                    Create_at = new DateTime(2025, 8, 1),
                    Formatted_content = "abc",
                    View = 12,
                    IsVisible = false
                },
                new News_eventsModel
                {
                    Id_newsevent = 4,
                    Title = "Tin tức & Sự kiện 4",
                    Description_short = "Nội dung tin tức 4",
                    Content = "Chi tiết nội dung 4",
                    Id_categories = 4,
                    Id_account = 2,
                    Image = "Anh4.jpg",
                    Create_at = new DateTime(2025, 10, 1),
                    Formatted_content = "abc",
                    View = 16,
                    IsVisible = false
                }
            );

            // Seed data for Document  
            modelBuilder.Entity<DocumentModel>().HasData(
                new DocumentModel
                {
                    Id_document = 1,
                    Title = "Văn bản pháp luật 1",
                    Id_category_document = 2,
                    File_path = "Doc1.docx",
                    Description_short = "Đó là nội dung ngắn mô tả về văn bản pháp luật 1",
                    Description = "Mô tả văn bản 1",
                    Create_at = new DateTime(2025, 5, 6),
                    Id_account = 2,
                    View_documents = 5,
                    IsVisible = true
                },
                new DocumentModel
                {
                    Id_document = 2,
                    Title = "Văn bản pháp luật 2",
                    Id_category_document = 1,
                    File_path = "Doc2.docx",
                    Description_short = "Đó là nội dung ngắn mô tả về văn bản pháp luật 2",
                    Description = "Mô tả văn bản 2",
                    Create_at = new DateTime(2025, 5, 6),
                    Id_account = 1,
                    View_documents = 10,
                    IsVisible = true
                },
                new DocumentModel
                {
                    Id_document = 3,
                    Title = "Văn bản pháp luật 3",
                    Id_category_document = 2,
                    File_path = "Doc3.docx",
                    Description_short = "Đó là nội dung ngắn mô tả về văn bản pháp luật 3",
                    Description = "Mô tả văn bản 3",
                    Create_at = new DateTime(2025, 5, 6),
                    Id_account = 1,
                    View_documents = 15,
                    IsVisible = false
                }
            );

            // Seed data for Cate_introduce      
            modelBuilder.Entity<Categories_introduceModel>().HasData(
                new Categories_introduceModel
                {
                    Id_cate_introduce = 1,
                    Name_cate_introduce = "Giới thiệu 1",
                },
                new Categories_introduceModel
                {
                    Id_cate_introduce = 2,
                    Name_cate_introduce = "Giới thiệu 2",
                },
                new Categories_introduceModel
                {
                    Id_cate_introduce = 3,
                    Name_cate_introduce = "Giới thiệu 3",
                },
                new Categories_introduceModel
                {
                    Id_cate_introduce = 4,
                    Name_cate_introduce = "Giới thiệu 4",
                },
                new Categories_introduceModel
                {
                    Id_cate_introduce = 5,
                    Name_cate_introduce = "Giới thiệu 5",
                }
            );


            // Seed data for Introduce      
            modelBuilder.Entity<IntroduceModel>().HasData(
                new IntroduceModel
                {
                    Id_introduce = 1,
                    Name_introduce = "Name giới thiệu 1",
                    Id_cate_introduce = 1,
                    FormatHTML = "Mô tả ngắn 1",
                    Description = "Mô tả chính 1",
                    Create_at = new DateTime(2025, 2, 1),
                    Image_url = "Anh1.jpg",
                },
                new IntroduceModel
                {
                    Id_introduce = 2,
                    Name_introduce = "Name giới thiệu 2",
                    Id_cate_introduce = 3,
                    FormatHTML = "Mô tả ngắn 2",
                    Description = "Mô tả chính 2",
                    Create_at = new DateTime(2025, 3, 12),
                    Image_url = "Anh2.jpg",
                },
                new IntroduceModel
                {
                    Id_introduce = 3,
                    Name_introduce = "Name giới thiệu 3",
                    Id_cate_introduce = 2,
                    FormatHTML = "Mô tả ngắn 3",
                    Description = "Mô tả chính 3",
                    Create_at = new DateTime(2025, 5, 18),
                    Image_url = "Anh3.jpg",
                }
            );


            // Seed data for Folder      
            modelBuilder.Entity<FolderModel>().HasData(
                new FolderModel
                {
                    Id_folder = 1,
                    Name_folder = " Danh mục hình ảnh 1",
                    ParentId = null,
                    CreatedAt = new DateTime(2025, 6, 12)
                },
                new FolderModel
                {
                    Id_folder = 2,
                    Name_folder = " Danh mục hình ảnh 1.1",
                    ParentId = 1,
                    CreatedAt = new DateTime(2025, 6, 12)
                },
                new FolderModel
                {
                    Id_folder = 3,
                    Name_folder = " Danh mục hình ảnh 1.2",
                    ParentId = 1,
                    CreatedAt = new DateTime(2025, 6, 12)
                },
                new FolderModel
                {
                    Id_folder = 4,
                    Name_folder = " Danh mục hình ảnh 2",
                    ParentId = null,
                    CreatedAt = new DateTime(2025, 6, 12)
                },
                new FolderModel
                {
                    Id_folder = 5,
                    Name_folder = " Danh mục hình ảnh 2.1",
                    ParentId = 4,
                    CreatedAt = new DateTime(2025, 6, 12)
                },
                new FolderModel
                {
                    Id_folder = 6,
                    Name_folder = " Danh mục hình ảnh 2.2",
                    ParentId = 4,
                    CreatedAt = new DateTime(2025, 6, 12)
                }
            );

            // Seed data cho PostImage
            modelBuilder.Entity<PostImageModel>().HasData(
                new PostImageModel
                {
                    Id_Image = 1,
                    FileName = "Anh1.jpg",
                    FilePath = "AAAAA",
                    Id_folder = 1,
                    UploadedAt = new DateTime(2022, 4, 1)
                },
                new PostImageModel
                {
                    Id_Image = 2,
                    FileName = "Anh2.jpg",
                    FilePath = "BBBB",
                    Id_folder = 2,
                    UploadedAt = new DateTime(2022, 4, 1)
                },
                new PostImageModel
                {
                    Id_Image = 3,
                    FileName = "Anh3.jpg",
                    FilePath = "CCCC",
                    Id_folder = 1,
                    UploadedAt = new DateTime(2022, 4, 1)
                },
                new PostImageModel
                {
                    Id_Image = 4,
                    FileName = "Anh4.jpg",
                    FilePath = "DDDDD",
                    Id_folder = 2,
                    UploadedAt = new DateTime(2022, 4, 1)
                }
            );

            /******************************************/
            // Seed data for FolderPdf      
            modelBuilder.Entity<FolderPdfModel>().HasData(
                new FolderPdfModel
                {
                    Id_folder_pdf = 1,
                    Name_folder = " Danh mục pdf 1",
                    ParentId = null,
                    CreatedAt = new DateTime(2025, 6, 12)
                },
                new FolderPdfModel
                {
                    Id_folder_pdf = 2,
                    Name_folder = " Danh mục pdf 1.1",
                    ParentId = 1,
                    CreatedAt = new DateTime(2025, 6, 12)
                },
                new FolderPdfModel
                {
                    Id_folder_pdf = 3,
                    Name_folder = " Danh mục pdf 1.2",
                    ParentId = 1,
                    CreatedAt = new DateTime(2025, 6, 12)
                },
                new FolderPdfModel
                {
                    Id_folder_pdf = 4,
                    Name_folder = " Danh mục pdf 2",
                    ParentId = null,
                    CreatedAt = new DateTime(2025, 6, 12)
                },
                new FolderPdfModel
                {
                    Id_folder_pdf = 5,
                    Name_folder = " Danh mục pdf 2.1",
                    ParentId = 4,
                    CreatedAt = new DateTime(2025, 6, 12)
                },
                new FolderPdfModel
                {
                    Id_folder_pdf = 6,
                    Name_folder = " Danh mục pdf 2.2",
                    ParentId = 4,
                    CreatedAt = new DateTime(2025, 6, 12)
                }
            );

            // Seed data cho PostPdf
            modelBuilder.Entity<PostPdfModel>().HasData(
                new PostPdfModel
                {
                    Id_Pdf = 1,
                    Id_folder_pdf = 1,
                    FileName = "Anh1.jpg",
                    FilePath = "AAAAA",
                    UploadedAt = new DateTime(2022, 4, 1)
                },
                new PostPdfModel
                {
                    Id_Pdf = 2,
                    Id_folder_pdf = 2,
                    FileName = "Anh2.jpg",
                    FilePath = "BBBB",
                    UploadedAt = new DateTime(2022, 4, 1)
                },
                new PostPdfModel
                {
                    Id_Pdf = 3,
                    Id_folder_pdf = 1,
                    FileName = "Anh3.jpg",
                    FilePath = "CCCC",
                    UploadedAt = new DateTime(2022, 4, 1)
                },
                new PostPdfModel
                {
                    Id_Pdf = 4,
                    Id_folder_pdf = 2,
                    FileName = "Anh4.jpg",
                    FilePath = "DDDDD",
                    UploadedAt = new DateTime(2022, 4, 1)
                }
            );


            // Seed data cho WebsiteSettings
            modelBuilder.Entity<WebsiteSettingsModel>().HasData(
                new WebsiteSettingsModel
                {
                    Id_webiste = 1,
                    LogoUrl = "logo.png",
                    BannerUrl = "banner.jpg",
                    WebsiteName = "Tin Tức 24h",
                    ThemeColor = "#ff5733",
                    BannerText = "Chào mừng bạn đến với Tin Tức 24h",
                    BannerBackgroundColor = "#f4f4f4",
                    TextRunning = "#f0f0f0",
                    BannnerTextColor = "#333333",
                    FooterBackgroundColor = "#333333",
                    FooterTextColor = "#ffffff",
                    FooterAddress = "123 Đường ABC, Quận 1, TP.HCM",
                    FooterPhone = "0123-456-789",
                    FooterEmail = "contact@tintuc24h.com",
                    GoogleMapEmbedLink = "<iframe src='https://www.google.com/maps/embed?...'></iframe>",
                    MenuBackgroundColor = "#ffffff",
                    MenuTextColor = "#000000",
                    SidebarBackgroundColor = "#1a1a1a",
                    HeaderBackgroundColor = "#000000",
                    SidebarTextColor = "#ffffff",
                    HeaderTextColor = "#ffcc00",
                    SidebarLayout = "{\"position\": \"left\", \"width\": \"250px\"}",
                    HeaderLayout = "{\"fixed\": true, \"height\": \"60px\"}"
                }
            );
        }
    }
}