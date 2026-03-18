using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostManagementApp.Data;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CemeteryInfoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CemeteryInfoController(AppDbContext context)
        {
            _context = context;
        }

// GET: api/cemeteryinfo/fullinfo - Lấy thông tin đầy đủ tất cả
        // Trả về thông tin đầy đủ: Người mất + Mộ + Thân nhân
        [HttpGet("fullinfo")]
        public async Task<ActionResult> GetFullCemeteryInfo()
        {
            var result = await _context.DeceasedPersons
                .AsNoTracking()
                .Include(d => d.Grave)
                .Include(d => d.Relatives)
                .Select(d => new
                {
                    DeceasedId = d.DeceasedId,
                    DeceasedName = d.Name,
                    BirthYear = d.BirthYear,
                    DeathYear = d.DeathYear,
                    Age = d.DeathYear - d.BirthYear,
                    Description = d.Description,
                    GraveInfo = d.Grave != null ? new
                    {
                        GraveId = d.Grave.GraveId,
                        Area = d.Grave.Area,
                        RowNumber = d.Grave.RowNumber,
                        GraveNumber = d.Grave.GraveNumber,
                        Location = d.Grave.Location,
                        Status = d.Grave.Status
                    } : null,
                    Relatives = d.Relatives.Select(r => new
                    {
                        RelativeId = r.RelativeId,
                        FullName = r.FullName,
                        Relationship = r.Relationship,
                        PhoneNumber = r.PhoneNumber,
                        Email = r.Email,
                        Address = r.Address
                    }).ToList()
                })
                .ToListAsync();

            return Ok(result);
        }

// GET: api/cemeteryinfo/gravearea/A - Lấy mộ theo khu vực
        // Lấy thông tin tất cả mộ trong khu vực A cùng người mất
        [HttpGet("gravearea/{area}")]
        public async Task<ActionResult> GetGravesByArea(string area)
        {
            var result = await _context.Graves
                .AsNoTracking()
                .Where(g => g.Area == area)
                .Include(g => g.DeceasedPerson)
                .ThenInclude(d => d!.Relatives)
                .Select(g => new
                {
                    GraveId = g.GraveId,
                    Area = g.Area,
                    RowNumber = g.RowNumber,
                    GraveNumber = g.GraveNumber,
                    Location = g.Location,
                    Status = g.Status,
                    DeceasedInfo = g.DeceasedPerson != null ? new
                    {
                        DeceasedId = g.DeceasedPerson.DeceasedId,
                        Name = g.DeceasedPerson.Name,
                        BirthYear = g.DeceasedPerson.BirthYear,
                        DeathYear = g.DeceasedPerson.DeathYear,
                        Description = g.DeceasedPerson.Description
                    } : null
                })
                .ToListAsync();

            if (!result.Any())
                return NotFound(new { message = $"Không tìm thấy mộ trong khu vực {area}" });

            return Ok(result);
        }

        // GET: api/cemeteryinfo/deceased/1/relatives
        // Lấy thông tin người mất và tất cả thân nhân
        [HttpGet("deceased/{id}/relatives")]
        public async Task<ActionResult> GetDeceasedWithRelatives(int id)
        {
            var result = await _context.DeceasedPersons
                .AsNoTracking()
                .Where(d => d.DeceasedId == id)
                .Include(d => d.Grave)
                .Include(d => d.Relatives)
                .Select(d => new
                {
                    DeceasedId = d.DeceasedId,
                    Name = d.Name,
                    BirthYear = d.BirthYear,
                    DeathYear = d.DeathYear,
                    Age = d.DeathYear - d.BirthYear,
                    Description = d.Description,
                    GraveLocation = d.Grave != null ? $"Khu {d.Grave.Area} - Hàng {d.Grave.RowNumber} - Mộ {d.Grave.GraveNumber}" : "Chưa có mộ",
                    RelativesCount = d.Relatives.Count,
                    Relatives = d.Relatives.Select(r => new
                    {
                        RelativeId = r.RelativeId,
                        FullName = r.FullName,
                        Relationship = r.Relationship,
                        PhoneNumber = r.PhoneNumber,
                        Email = r.Email
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (result == null)
                return NotFound(new { message = $"Không tìm thấy người mất với ID {id}" });

            return Ok(result);
        }

        // GET: api/cemeteryinfo/statistics
        // Thống kê: Tổng mộ, Mộ được sử dụng, Mộ còn trống
        [HttpGet("statistics")]
        public async Task<ActionResult> GetCemeteryStatistics()
        {
            var totalGraves = await _context.Graves.CountAsync();
            var occupiedGraves = await _context.Graves.CountAsync(g => g.Status == "Occupied");
            var availableGraves = await _context.Graves.CountAsync(g => g.Status == "Available");
            var totalDeceased = await _context.DeceasedPersons.CountAsync();
            var totalRelatives = await _context.Relatives.CountAsync();

            return Ok(new
            {
                TotalGraves = totalGraves,
                OccupiedGraves = occupiedGraves,
                AvailableGraves = availableGraves,
                ReservedGraves = totalGraves - occupiedGraves - availableGraves,
                TotalDeceased = totalDeceased,
                TotalRelatives = totalRelatives,
                OccupancyRate = totalGraves > 0 ? Math.Round((double)occupiedGraves / totalGraves * 100, 2) + "%" : "0%"
            });
        }
    }
}
