using Computer_Store.Data;
using Computer_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Computer_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Moderator")]
    public class ReportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/<ReportController>
        [HttpGet]
        public IEnumerable<DailyReport> Get()
        {
            var dailyReport = _context.DailyReports.ToList();
            dailyReport.Sort((a, b) => DateTime.Compare(b.Date, a.Date));
            return dailyReport;
        }
    }
}
