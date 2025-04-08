using Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.Data;
using ttxaphuong.Interfaces;
using ttxaphuong.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using WebDoAn2.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            RoleClaimType = ClaimTypes.Role, // ✅ Sửa thành ClaimTypes.Role
            NameClaimType = ClaimTypes.Name
        };

    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    ));

//Đoạn Services cho chức năng Logic hoạt động
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<INews_eventsService, News_eventsService>();

builder.Services.AddScoped<ICategory_documentsService, Category_documentsService>();
builder.Services.AddScoped<IDocumentsService, DocumentsService>();

builder.Services.AddScoped<ICategories_introduceService, Categories_introduceService>();
builder.Services.AddScoped<IIntroduceService, IntroduceService>();

builder.Services.AddScoped<ICategory_fieldService, CategoryFieldService>();
builder.Services.AddScoped<IProceduresService, ProceduresService>();

builder.Services.AddScoped<IFeedbacksService, FeedbacksService>();

builder.Services.AddScoped<IUploadFileService, UploadFileService>();
builder.Services.AddScoped<IFolderService, FolderService>();

builder.Services.AddScoped<IUploadFilePdfService, UploadFilePdfService>();
builder.Services.AddScoped<IFolderPdfService, FolderPdfService>();

builder.Services.AddScoped<IStatisticsService, StatisticsService>();

builder.Services.AddScoped<IWebsiteSettingsService, WebsiteSettingsService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://ttdt03.id.vn") // Địa chỉ của ứng dụng 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
                  //.AllowCredentials(); // ✅ Cho phép gửi Authorization header
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Kiểm tra xem môi trường là development hay production
    if (app.Environment.IsDevelopment())
    {
        try
        {
            // Tự động thực hiện migration chỉ trong môi trường phát triển
            db.Database.Migrate(); // Tự động chạy migration khi ứng dụng khởi động
        }
        catch (Exception ex)
        {
            // Log lỗi nếu migration gặp vấn đề
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");

            // Có thể thêm logic xử lý hoặc thông báo cho người dùng ở đây
        }
    }
}


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.Urls.Add("http://*:8080");

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
//    RequestPath = "/api/images"
//});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Pdf")),
//    RequestPath = "/api/pdf"
//});


var uploadPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
if (!Directory.Exists(uploadPath))
{
    Directory.CreateDirectory(uploadPath);
}

var pdfPath = Path.Combine(builder.Environment.ContentRootPath, "Pdf");
if (!Directory.Exists(pdfPath))
{
    Directory.CreateDirectory(pdfPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/api/images"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(pdfPath),
    RequestPath = "/api/pdf"
});


app.UseCors("AllowAngularApp");

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();  // Đảm bảo đã gọi middleware xác thực

app.UseAuthorization();  // Đảm bảo đã gọi middleware phân quyền

app.MapControllers();

app.Run();
