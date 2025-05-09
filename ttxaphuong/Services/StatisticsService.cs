﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.Data;
using ttxaphuong.Interfaces;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StatisticsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Tính tổng số lượng bài viết tin tức - sự kiện
        public async Task<int> GetTotalNewsEventsAsync()
        {
            return await _context.News_Events.CountAsync();
        }

        // Tính tổng số lượng văn bản - pháp luật
        public async Task<int> GetTotalDocumentsAsync()
        {
            return await _context.Documents.CountAsync();
        }

        // Tính tổng số lượng thủ tục hành chính
        public async Task<int> GetTotalProceduresAsync()
        {
            return await _context.Procedures.CountAsync();
        }

        // Tính tổng số lượng bài giới thiệu
        public async Task<int> GetTotalIntroducesAsync()
        {
            return await _context.Introduces.CountAsync();
        }

        // Tính tổng tất cả các số lượng
        public async Task<int> GetStatistics()
        {
            var totalNewEvents = await GetTotalNewsEventsAsync();
            var totalDocuments = await GetTotalDocumentsAsync();
            var totalProcedure = await GetTotalProceduresAsync();
            var totalIntroduces = await GetTotalIntroducesAsync();

            // Tổng hợp tất cả các số lượng
            int totalCount = totalNewEvents + totalDocuments + totalProcedure + totalIntroduces;
            return totalCount;
        }

        // Trả về lượt xem theo danh mục
        public async Task<List<object>> GetNewsViewsByCategoryAsync()
        {
            var categoryViews = await _context.News_Events
                .Include(n => n.CategoriesModel) // ✅ Load danh mục bài viết
                .Where(n => n.CategoriesModel != null) // ✅ Loại bỏ bài viết không có danh mục
                .GroupBy(n => n.CategoriesModel.Name_category) // ✅ Nhóm theo tên danh mục
                .Select(group => new
                {
                    Category = group.Key,  // ✅ Lấy tên danh mục
                    TotalViews = group.Sum(n => n.View) // ✅ Tổng lượt xem trong danh mục đó
                })
                .ToListAsync();

            return categoryViews.Cast<object>().ToList();
        }

        //public async Task<List<object>> GetNewsViewsOverTimeAsync()
        //{
        //    var viewsOverTime = await _context.News_Events
        //        .GroupBy(n => n.Create_at.Date) // ✅ Nhóm theo ngày
        //        .Select(group => new
        //        {
        //            Date = group.Key,
        //            TotalViews = group.Sum(n => n.View)
        //        })
        //        .ToListAsync();

        //    return viewsOverTime.Cast<object>().ToList();
        //}

        public async Task<List<object>> GetNewsAndDocumentsViewsOverTimeAsync()
        {
            // Tin tức
            var newsViews = await _context.News_Events
                .GroupBy(n => n.Create_at.Date)
                .Select(g => new {
                    Date = g.Key,
                    TotalViewsNews = g.Sum(n => n.View) ?? 0
                }).ToListAsync();

            // Văn bản
            var docViews = await _context.Documents
                .GroupBy(d => d.Create_at.Date)
                .Select(g => new {
                    Date = g.Key,
                    TotalViewsDocs = g.Sum(d => d.View_documents) ?? 0
                }).ToListAsync();

            // Gộp theo ngày
            var combined = newsViews
                .Union(docViews.Select(d => new { d.Date, TotalViewsNews = 0 })) // để merge các ngày chỉ có văn bản
                .GroupBy(x => x.Date)
                .Select(g => new {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    TotalViewsNews = newsViews.FirstOrDefault(n => n.Date == g.Key)?.TotalViewsNews ?? 0,
                    TotalViewsDocs = docViews.FirstOrDefault(d => d.Date == g.Key)?.TotalViewsDocs ?? 0
                })
                .OrderBy(x => x.Date)
                .ToList<object>();

            return combined;
        }


    }
}

