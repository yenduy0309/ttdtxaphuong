namespace ttxaphuong.DTO
{
    public class WebsiteSettingsDTO
    {
        public int Id_webiste { get; set; } = 1; // Đặt mặc định là 1 để chỉ có 1 bản ghi duy nhất

        // Cài đặt Trang Người Dùng
        public string? LogoUrl { get; set; }
        public string? BannerUrl { get; set; }
        public string? WebsiteName { get; set; }
        public string? ThemeColor { get; set; } // Màu chủ đề giao diện (hex color)
        public string? BannerText { get; set; }
        public string? BannerBackgroundColor { get; set; }
        public string? BannnerTextColor { get; set; } //
        public string? TextRunning { get; set; } //
        public string? FooterBackgroundColor { get; set; }
        public string? FooterTextColor { get; set; }
        public string? FooterAddress { get; set; }
        public string? FooterPhone { get; set; }
        public string? FooterEmail { get; set; }
        public string? GoogleMapEmbedLink { get; set; }
        public string? MenuBackgroundColor { get; set; }
        public string? MenuTextColor { get; set; }

        // Cài đặt Trang Quản Trị Viên
        public string? SidebarBackgroundColor { get; set; }
        public string? HeaderBackgroundColor { get; set; }
        public string? SidebarTextColor { get; set; }
        public string? HeaderTextColor { get; set; }
        public string? SidebarLayout { get; set; } // Lưu JSON nếu có nhiều tùy chỉnh
        public string? HeaderLayout { get; set; } // Lưu JSON nếu có nhiều tùy chỉnh
    }
}
