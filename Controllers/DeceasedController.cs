using Microsoft.AspNetCore.Mvc;
using PostManagementApp.Models;
using PostManagementApp.Services;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeceasedController : ControllerBase
    {
        private readonly IDeceasedPersonService _deceasedService;

        public DeceasedController(IDeceasedPersonService deceasedService)
        {
            _deceasedService = deceasedService;
        }

        // GET: api/deceased
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeceasedPerson>>> GetAllDeceased()
        {
            var deceased = await _deceasedService.GetAllDeceasedAsync();
            return Ok(deceased);
        }

        // GET: api/deceased/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeceasedPerson>> GetDeceasedById(int id)
        {
            var deceased = await _deceasedService.GetDeceasedByIdAsync(id);
            if (deceased == null)
                return NotFound(new { message = $"Không tìm thấy người mất với ID {id}" });
            return Ok(deceased);
        }

        // POST: api/deceased
        [HttpPost]
        public async Task<ActionResult<DeceasedPerson>> CreateDeceased([FromBody] DeceasedPerson deceased)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDeceased = await _deceasedService.CreateDeceasedAsync(deceased);
            return CreatedAtAction(nameof(GetDeceasedById), new { id = createdDeceased.DeceasedId }, createdDeceased);
        }

        // PUT: api/deceased/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DeceasedPerson>> UpdateDeceased(int id, [FromBody] DeceasedPerson deceased)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedDeceased = await _deceasedService.UpdateDeceasedAsync(id, deceased);
            if (updatedDeceased == null)
                return NotFound(new { message = $"Không tìm thấy người mất với ID {id}" });
            return Ok(updatedDeceased);
        }

        // DELETE: api/deceased/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeceased(int id)
        {
            var success = await _deceasedService.DeleteDeceasedAsync(id);
            if (!success)
                return NotFound(new { message = $"Không tìm thấy người mất với ID {id}" });
            return Ok(new { message = "Xóa người mất thành công" });
        }

        // GET: api/deceased/fullinfo - API JOIN để lấy thông tin đầy đủ
        [HttpGet("fullinfo")]
        public async Task<ActionResult<IEnumerable<DeceasedFullInfo>>> GetDeceasedFullInfo()
        {
            var fullInfo = await _deceasedService.GetDeceasedFullInfoAsync();
            return Ok(fullInfo);
        }
    }
}
