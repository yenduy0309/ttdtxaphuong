using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.Data;
using ttxaphuong.DTO.Feedbacks;
using ttxaphuong.DTO.Introduces;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Feedbacks;
using ttxaphuong.Models.Introduce;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class FeedbacksService : IFeedbacksService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FeedbacksService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FeedbacksDTO>> GetAllFeedbacksAsync()
        {
            try
            {
                var services = await _context.Feedbacks.ToListAsync();
                return _mapper.Map<IEnumerable<FeedbacksDTO>>(services);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách phản hồi", ex);
            }
        }

        public async Task<FeedbacksDTO?> ApproveFeedbackAsync(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return null;

            feedback.Status = "Approved"; // ✅ Đánh dấu phản hồi là đã duyệt
            await _context.SaveChangesAsync();

            return _mapper.Map<FeedbacksDTO>(feedback);
        }

        public async Task<FeedbacksDTO?> RejectFeedbackAsync(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null) return null;

            feedback.Status = "Rejected"; // ❌ Đánh dấu phản hồi là bị từ chối
            await _context.SaveChangesAsync();

            return _mapper.Map<FeedbacksDTO>(feedback);
        }

        public async Task<object> GetFeedbackStatisticsAsync()
        {
            var totalFeedbacks = await _context.Feedbacks.CountAsync();
            var approvedCount = await _context.Feedbacks.CountAsync(f => f.Status == "Approved");
            var pendingCount = await _context.Feedbacks.CountAsync(f => f.Status == "Pending");
            var rejectedCount = await _context.Feedbacks.CountAsync(f => f.Status == "Rejected");

            return new
            {
                TotalFeedbacks = totalFeedbacks,
                ApprovedCount = approvedCount,
                PendingCount = pendingCount,
                RejectedCount = rejectedCount
            };
        }

        public async Task<FeedbacksDTO> CreateFeedbackAsync(FeedbacksDTO feedbacks)
        {
            try
            {
                var feed_backModel = _mapper.Map<FeedbacksModel>(feedbacks);

                _context.Feedbacks.Add(feed_backModel);
                await _context.SaveChangesAsync();

                return _mapper.Map<FeedbacksDTO>(feed_backModel);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra: " + ex.Message, ex);
            }
        }
    }
}

