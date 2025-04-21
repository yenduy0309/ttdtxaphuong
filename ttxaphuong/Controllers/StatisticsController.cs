using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.Interfaces;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetTotalNewsEvents()
        {
            var totalLocations = await _statisticsService.GetTotalNewsEventsAsync();
            return Ok(totalLocations);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetTotalDocuments()
        {
            var totalTransport = await _statisticsService.GetTotalDocumentsAsync();
            return Ok(totalTransport);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetTotalProcedures()
        {
            var totalHotels = await _statisticsService.GetTotalProceduresAsync();
            return Ok(totalHotels);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetTotalIntroduces()
        {
            var totalFoods = await _statisticsService.GetTotalIntroducesAsync();
            return Ok(totalFoods);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetStatistics()
        {
            var totalNewEvents = await _statisticsService.GetTotalNewsEventsAsync();
            var totalDocuments = await _statisticsService.GetTotalDocumentsAsync();
            var totalProcedure = await _statisticsService.GetTotalProceduresAsync();
            var totalIntroduces = await _statisticsService.GetTotalIntroducesAsync();

            var result = new
            {
                totalNewEvents,
                totalDocuments,
                totalProcedure,
                totalIntroduces
            };

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetNewsViewsByCategory()
        {
            var categoryViews = await _statisticsService.GetNewsViewsByCategoryAsync();
            return Ok(categoryViews);
        }

        //[HttpGet]
        //[Authorize(Roles = "Admin,Manager")]
        //public async Task<IActionResult> GetNewsViewsOverTime()
        //{
        //    var data = await _statisticsService.GetNewsViewsOverTimeAsync();
        //    return Ok(data);
        //}

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetNewsViewsOverTime()
        {
            var data = await _statisticsService.GetNewsAndDocumentsViewsOverTimeAsync();
            return Ok(data);
        }

    }
}
