using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ttxaphuong.Migrations
{
    /// <inheritdoc />
    public partial class Lan1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id_account = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fullname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    VerificationCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodeExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id_account);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categogy_Fields",
                columns: table => new
                {
                    Id_Field = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_Field = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FielParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categogy_Fields", x => x.Id_Field);
                    table.ForeignKey(
                        name: "FK_Categogy_Fields_Categogy_Fields_FielParentId",
                        column: x => x.FielParentId,
                        principalTable: "Categogy_Fields",
                        principalColumn: "Id_Field");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id_categories = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_category = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id_categories);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id_categories");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories_Introduces",
                columns: table => new
                {
                    Id_cate_introduce = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_cate_introduce = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories_Introduces", x => x.Id_cate_introduce);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Category_Documents",
                columns: table => new
                {
                    Id_category_document = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_category_document = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DocumentParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category_Documents", x => x.Id_category_document);
                    table.ForeignKey(
                        name: "FK_Category_Documents_Category_Documents_DocumentParentId",
                        column: x => x.DocumentParentId,
                        principalTable: "Category_Documents",
                        principalColumn: "Id_category_document");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id_feedbacks = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fullname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Received_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id_feedbacks);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FolderPdfs",
                columns: table => new
                {
                    Id_folder_pdf = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_folder = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderPdfs", x => x.Id_folder_pdf);
                    table.ForeignKey(
                        name: "FK_FolderPdfs_FolderPdfs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FolderPdfs",
                        principalColumn: "Id_folder_pdf");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id_folder = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_folder = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id_folder);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Folders",
                        principalColumn: "Id_folder");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id_settings = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id_settings);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WebsiteSettings",
                columns: table => new
                {
                    Id_webiste = table.Column<int>(type: "int", nullable: false),
                    LogoUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BannerUrl = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WebsiteName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ThemeColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BannerText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BannerBackgroundColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BannnerTextColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TextRunning = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FooterBackgroundColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FooterTextColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FooterAddress = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FooterPhone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FooterEmail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GoogleMapEmbedLink = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuBackgroundColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuTextColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SidebarBackgroundColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HeaderBackgroundColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SidebarTextColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HeaderTextColor = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SidebarLayout = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HeaderLayout = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteSettings", x => x.Id_webiste);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    CanAddUser = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanEditUser = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanDeleteUser = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanViewUsers = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanManageRoles = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanManagePermissions = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Accounts_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Accounts",
                        principalColumn: "Id_account",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Procedures",
                columns: table => new
                {
                    Id_procedures = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_thutuc = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name_procedures = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_Field = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date_issue = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Id_account = table.Column<int>(type: "int", nullable: false),
                    FormatText = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsVisible = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => x.Id_procedures);
                    table.ForeignKey(
                        name: "FK_Procedures_Accounts_Id_account",
                        column: x => x.Id_account,
                        principalTable: "Accounts",
                        principalColumn: "Id_account",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Procedures_Categogy_Fields_Id_Field",
                        column: x => x.Id_Field,
                        principalTable: "Categogy_Fields",
                        principalColumn: "Id_Field",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "News_Events",
                columns: table => new
                {
                    Id_newsevent = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description_short = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_account = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Formatted_content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    View = table.Column<int>(type: "int", nullable: true),
                    Id_categories = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News_Events", x => x.Id_newsevent);
                    table.ForeignKey(
                        name: "FK_News_Events_Accounts_Id_account",
                        column: x => x.Id_account,
                        principalTable: "Accounts",
                        principalColumn: "Id_account",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_News_Events_Categories_Id_categories",
                        column: x => x.Id_categories,
                        principalTable: "Categories",
                        principalColumn: "Id_categories",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Introduces",
                columns: table => new
                {
                    Id_introduce = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name_introduce = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_cate_introduce = table.Column<int>(type: "int", nullable: false),
                    FormatHTML = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Create_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Image_url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Introduces", x => x.Id_introduce);
                    table.ForeignKey(
                        name: "FK_Introduces_Categories_Introduces_Id_cate_introduce",
                        column: x => x.Id_cate_introduce,
                        principalTable: "Categories_Introduces",
                        principalColumn: "Id_cate_introduce",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id_document = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    File_path = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description_short = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Create_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Id_account = table.Column<int>(type: "int", nullable: false),
                    View_documents = table.Column<int>(type: "int", nullable: true),
                    Id_category_document = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id_document);
                    table.ForeignKey(
                        name: "FK_Documents_Accounts_Id_account",
                        column: x => x.Id_account,
                        principalTable: "Accounts",
                        principalColumn: "Id_account",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Category_Documents_Id_category_document",
                        column: x => x.Id_category_document,
                        principalTable: "Category_Documents",
                        principalColumn: "Id_category_document",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PostPdfs",
                columns: table => new
                {
                    Id_Pdf = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_folder_pdf = table.Column<int>(type: "int", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostPdfs", x => x.Id_Pdf);
                    table.ForeignKey(
                        name: "FK_PostPdfs_FolderPdfs_Id_folder_pdf",
                        column: x => x.Id_folder_pdf,
                        principalTable: "FolderPdfs",
                        principalColumn: "Id_folder_pdf",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PostImages",
                columns: table => new
                {
                    Id_Image = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_folder = table.Column<int>(type: "int", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostImages", x => x.Id_Image);
                    table.ForeignKey(
                        name: "FK_PostImages_Folders_Id_folder",
                        column: x => x.Id_folder,
                        principalTable: "Folders",
                        principalColumn: "Id_folder",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id_account", "CodeExpiry", "Create_at", "Email", "Fullname", "Password", "RefreshToken", "RefreshTokenExpiry", "Role", "Status", "Username", "VerificationCode" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 2, 4, 15, 36, 995, DateTimeKind.Utc).AddTicks(2238), "hoanguyentranthi32@gmail.com", "Nguyễn Trần Thị Hòa", "03092003", null, null, "Manager", "IsActive", "ThiHoa", null },
                    { 2, null, new DateTime(2025, 4, 2, 4, 15, 36, 995, DateTimeKind.Utc).AddTicks(2243), "TuiLaHoa25@gmail.com", "Nguyễn Hòa Trần Thị", "123456789", null, null, "Admin", "IsActive", "HoaThi", null }
                });

            migrationBuilder.InsertData(
                table: "Categogy_Fields",
                columns: new[] { "Id_Field", "FielParentId", "Name_Field" },
                values: new object[,]
                {
                    { 1, null, "Danh mục lĩnh vực 1" },
                    { 4, null, "Danh mục lĩnh vực 4" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id_categories", "Name_category", "ParentId" },
                values: new object[,]
                {
                    { 1, " Danh mục A", null },
                    { 5, " Danh mục B", null }
                });

            migrationBuilder.InsertData(
                table: "Categories_Introduces",
                columns: new[] { "Id_cate_introduce", "Name_cate_introduce" },
                values: new object[,]
                {
                    { 1, "Giới thiệu 1" },
                    { 2, "Giới thiệu 2" },
                    { 3, "Giới thiệu 3" },
                    { 4, "Giới thiệu 4" },
                    { 5, "Giới thiệu 5" }
                });

            migrationBuilder.InsertData(
                table: "Category_Documents",
                columns: new[] { "Id_category_document", "DocumentParentId", "Name_category_document" },
                values: new object[,]
                {
                    { 1, null, "Danh mục VB" },
                    { 5, null, "Danh mục VB3" }
                });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "Id_feedbacks", "Content", "Email", "Fullname", "Phone", "Received_at", "Status" },
                values: new object[,]
                {
                    { 1, "Nội dung 1", "NguyenVanA@gmail.com", "Nguyễn Văn A", "0772962490", new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending" },
                    { 2, "Nội dung 2", "NguyenVanB@gmail.com", "Nguyễn Văn B", "0772962490", new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved" },
                    { 3, "Nội dung 3", "NguyenVanC@gmail.com", "Nguyễn Văn C", "0772962490", new DateTime(2025, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rejected" },
                    { 4, "Nội dung 4", "NguyenVanC@gmail.com", "Nguyễn Thị Hoa", "0772962490", new DateTime(2025, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending" }
                });

            migrationBuilder.InsertData(
                table: "FolderPdfs",
                columns: new[] { "Id_folder_pdf", "CreatedAt", "Name_folder", "ParentId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục pdf 1", null },
                    { 4, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục pdf 2", null }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id_folder", "CreatedAt", "Name_folder", "ParentId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục hình ảnh 1", null },
                    { 4, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục hình ảnh 2", null }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id_settings", "Description", "Key_name" },
                values: new object[,]
                {
                    { 1, "Mô tả cấu hình 1", "Cấu hình 1" },
                    { 2, "Mô tả cấu hình 2", "Cấu hình 2" }
                });

            migrationBuilder.InsertData(
                table: "WebsiteSettings",
                columns: new[] { "Id_webiste", "BannerBackgroundColor", "BannerText", "BannerUrl", "BannnerTextColor", "FooterAddress", "FooterBackgroundColor", "FooterEmail", "FooterPhone", "FooterTextColor", "GoogleMapEmbedLink", "HeaderBackgroundColor", "HeaderLayout", "HeaderTextColor", "LogoUrl", "MenuBackgroundColor", "MenuTextColor", "SidebarBackgroundColor", "SidebarLayout", "SidebarTextColor", "TextRunning", "ThemeColor", "WebsiteName" },
                values: new object[] { 1, "#f4f4f4", "Chào mừng bạn đến với Tin Tức 24h", "banner.jpg", "#333333", "123 Đường ABC, Quận 1, TP.HCM", "#333333", "contact@tintuc24h.com", "0123-456-789", "#ffffff", "<iframe src='https://www.google.com/maps/embed?...'></iframe>", "#000000", "{\"fixed\": true, \"height\": \"60px\"}", "#ffcc00", "logo.png", "#ffffff", "#000000", "#1a1a1a", "{\"position\": \"left\", \"width\": \"250px\"}", "#ffffff", "#f0f0f0", "#ff5733", "Tin Tức 24h" });

            migrationBuilder.InsertData(
                table: "Categogy_Fields",
                columns: new[] { "Id_Field", "FielParentId", "Name_Field" },
                values: new object[,]
                {
                    { 2, 1, "Danh mục lĩnh vực 2" },
                    { 6, 1, "Danh mục lĩnh vực 6" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id_categories", "Name_category", "ParentId" },
                values: new object[,]
                {
                    { 2, " Danh mục A1", 1 },
                    { 4, " Danh mục A2", 1 },
                    { 6, " Danh mục B1", 5 }
                });

            migrationBuilder.InsertData(
                table: "Category_Documents",
                columns: new[] { "Id_category_document", "DocumentParentId", "Name_category_document" },
                values: new object[,]
                {
                    { 2, 1, "Danh mục VB1" },
                    { 4, 1, "Danh mục VB2" },
                    { 6, 5, "Danh mục VB3.1" },
                    { 7, 5, "Danh mục VB3.1" }
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id_document", "Create_at", "Description", "Description_short", "File_path", "Id_account", "Id_category_document", "IsVisible", "Title", "View_documents" },
                values: new object[] { 2, new DateTime(2025, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mô tả văn bản 2", "Đó là nội dung ngắn mô tả về văn bản pháp luật 2", "Doc2.docx", 1, 1, true, "Văn bản pháp luật 2", 10 });

            migrationBuilder.InsertData(
                table: "FolderPdfs",
                columns: new[] { "Id_folder_pdf", "CreatedAt", "Name_folder", "ParentId" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục pdf 1.1", 1 },
                    { 3, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục pdf 1.2", 1 },
                    { 5, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục pdf 2.1", 4 },
                    { 6, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục pdf 2.2", 4 }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id_folder", "CreatedAt", "Name_folder", "ParentId" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục hình ảnh 1.1", 1 },
                    { 3, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục hình ảnh 1.2", 1 },
                    { 5, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục hình ảnh 2.1", 4 },
                    { 6, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), " Danh mục hình ảnh 2.2", 4 }
                });

            migrationBuilder.InsertData(
                table: "Introduces",
                columns: new[] { "Id_introduce", "Create_at", "Description", "FormatHTML", "Id_cate_introduce", "Image_url", "Name_introduce" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mô tả chính 1", "Mô tả ngắn 1", 1, "Anh1.jpg", "Name giới thiệu 1" },
                    { 2, new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mô tả chính 2", "Mô tả ngắn 2", 3, "Anh2.jpg", "Name giới thiệu 2" },
                    { 3, new DateTime(2025, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mô tả chính 3", "Mô tả ngắn 3", 2, "Anh3.jpg", "Name giới thiệu 3" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CanAddUser", "CanDeleteUser", "CanEditUser", "CanManagePermissions", "CanManageRoles", "CanViewUsers", "ManagerId" },
                values: new object[,]
                {
                    { 1, true, true, true, true, true, true, 2 },
                    { 2, false, false, false, false, false, true, 1 }
                });

            migrationBuilder.InsertData(
                table: "PostImages",
                columns: new[] { "Id_Image", "FileName", "FilePath", "Id_folder", "UploadedAt" },
                values: new object[,]
                {
                    { 1, "Anh1.jpg", "AAAAA", 1, new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Anh3.jpg", "CCCC", 1, new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "PostPdfs",
                columns: new[] { "Id_Pdf", "FileName", "FilePath", "Id_folder_pdf", "UploadedAt" },
                values: new object[,]
                {
                    { 1, "Anh1.jpg", "AAAAA", 1, new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Anh3.jpg", "CCCC", 1, new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Procedures",
                columns: new[] { "Id_procedures", "Create_at", "Date_issue", "Description", "FormatText", "Id_Field", "Id_account", "Id_thutuc", "IsVisible", "Name_procedures" },
                values: new object[] { 1, new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mô tả 1", "abcd", 1, 2, "N1", true, "Tên thủ tục 1" });

            migrationBuilder.InsertData(
                table: "Categogy_Fields",
                columns: new[] { "Id_Field", "FielParentId", "Name_Field" },
                values: new object[,]
                {
                    { 3, 2, "Danh mục lĩnh vực 3" },
                    { 5, 2, "Danh mục lĩnh vực 5" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id_categories", "Name_category", "ParentId" },
                values: new object[,]
                {
                    { 3, " Danh mục A1.1", 2 },
                    { 7, " Danh mục B1.1", 6 },
                    { 8, " Danh mục B1.2", 6 }
                });

            migrationBuilder.InsertData(
                table: "Category_Documents",
                columns: new[] { "Id_category_document", "DocumentParentId", "Name_category_document" },
                values: new object[] { 3, 2, "Danh mục VB1.1" });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id_document", "Create_at", "Description", "Description_short", "File_path", "Id_account", "Id_category_document", "IsVisible", "Title", "View_documents" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mô tả văn bản 1", "Đó là nội dung ngắn mô tả về văn bản pháp luật 1", "Doc1.docx", 2, 2, true, "Văn bản pháp luật 1", 5 },
                    { 3, new DateTime(2025, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mô tả văn bản 3", "Đó là nội dung ngắn mô tả về văn bản pháp luật 3", "Doc3.docx", 1, 2, false, "Văn bản pháp luật 3", 15 }
                });

            migrationBuilder.InsertData(
                table: "News_Events",
                columns: new[] { "Id_newsevent", "Content", "Create_at", "Description_short", "Formatted_content", "Id_account", "Id_categories", "Image", "IsVisible", "Title", "View" },
                values: new object[,]
                {
                    { 1, "Chi tiết nội dung 1", new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nội dung tin tức 1", "abc", 2, 2, "Anh1.jpg", true, "Tin tức & Sự kiện 1", 26 },
                    { 3, "Chi tiết nội dung 3", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nội dung tin tức 3", "abc", 2, 2, "Anh3.jpg", false, "Tin tức & Sự kiện 3", 12 },
                    { 4, "Chi tiết nội dung 4", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nội dung tin tức 4", "abc", 2, 4, "Anh4.jpg", false, "Tin tức & Sự kiện 4", 16 }
                });

            migrationBuilder.InsertData(
                table: "PostImages",
                columns: new[] { "Id_Image", "FileName", "FilePath", "Id_folder", "UploadedAt" },
                values: new object[,]
                {
                    { 2, "Anh2.jpg", "BBBB", 2, new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Anh4.jpg", "DDDDD", 2, new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "PostPdfs",
                columns: new[] { "Id_Pdf", "FileName", "FilePath", "Id_folder_pdf", "UploadedAt" },
                values: new object[,]
                {
                    { 2, "Anh2.jpg", "BBBB", 2, new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Anh4.jpg", "DDDDD", 2, new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Procedures",
                columns: new[] { "Id_procedures", "Create_at", "Date_issue", "Description", "FormatText", "Id_Field", "Id_account", "Id_thutuc", "IsVisible", "Name_procedures" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mô tả 2", "abcd", 2, 2, "N2", true, "Tên thủ tục 2" },
                    { 3, new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mô tả 3", "abcd", 2, 1, "N3", true, "Tên thủ tục 3" }
                });

            migrationBuilder.InsertData(
                table: "News_Events",
                columns: new[] { "Id_newsevent", "Content", "Create_at", "Description_short", "Formatted_content", "Id_account", "Id_categories", "Image", "IsVisible", "Title", "View" },
                values: new object[] { 2, "Chi tiết nội dung 2", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nội dung tin tức 2", "abc", 1, 3, "Anh2.jpg", true, "Tin tức & Sự kiện 2", 10 });

            migrationBuilder.CreateIndex(
                name: "IX_Categogy_Fields_FielParentId",
                table: "Categogy_Fields",
                column: "FielParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Documents_DocumentParentId",
                table: "Category_Documents",
                column: "DocumentParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Id_account",
                table: "Documents",
                column: "Id_account");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Id_category_document",
                table: "Documents",
                column: "Id_category_document");

            migrationBuilder.CreateIndex(
                name: "IX_FolderPdfs_ParentId",
                table: "FolderPdfs",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentId",
                table: "Folders",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Introduces_Id_cate_introduce",
                table: "Introduces",
                column: "Id_cate_introduce");

            migrationBuilder.CreateIndex(
                name: "IX_News_Events_Id_account",
                table: "News_Events",
                column: "Id_account");

            migrationBuilder.CreateIndex(
                name: "IX_News_Events_Id_categories",
                table: "News_Events",
                column: "Id_categories");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ManagerId",
                table: "Permissions",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_Id_folder",
                table: "PostImages",
                column: "Id_folder");

            migrationBuilder.CreateIndex(
                name: "IX_PostPdfs_Id_folder_pdf",
                table: "PostPdfs",
                column: "Id_folder_pdf");

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_Id_account",
                table: "Procedures",
                column: "Id_account");

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_Id_Field",
                table: "Procedures",
                column: "Id_Field");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Introduces");

            migrationBuilder.DropTable(
                name: "News_Events");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PostImages");

            migrationBuilder.DropTable(
                name: "PostPdfs");

            migrationBuilder.DropTable(
                name: "Procedures");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "WebsiteSettings");

            migrationBuilder.DropTable(
                name: "Category_Documents");

            migrationBuilder.DropTable(
                name: "Categories_Introduces");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "FolderPdfs");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Categogy_Fields");
        }
    }
}
