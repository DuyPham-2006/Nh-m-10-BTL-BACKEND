using Microsoft.AspNetCore.Mvc;
using PostManagementApp.Models;
using PostManagementApp.Services;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelativesController : ControllerBase
    {
        private readonly IRelativeService _relativeService;

        public RelativesController(IRelativeService relativeService)
        {
            _relativeService = relativeService;
        }

        // GET: api/relatives
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Relative>>> GetAllRelatives()
        {
            var relatives = await _relativeService.GetAllRelativesAsync();
            return Ok(relatives);
        }

        // GET: api/relatives/deceased/5
        [HttpGet("deceased/{deceasedId}")]
        public async Task<ActionResult<IEnumerable<Relative>>> GetRelativesByDeceased(int deceasedId)
        {
            var relatives = await _relativeService.GetRelativesByDeceasedIdAsync(deceasedId);
            return Ok(relatives);
        }

        // GET: api/relatives/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Relative>> GetRelativeById(int id)
        {
            var relative = await _relativeService.GetRelativeByIdAsync(id);
            if (relative == null)
                return NotFound(new { message = $"Không tìm thấy thân nhân với ID {id}" });
            return Ok(relative);
        }

        // POST: api/relatives
        [HttpPost]
        public async Task<ActionResult<Relative>> CreateRelative([FromBody] Relative relative)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRelative = await _relativeService.CreateRelativeAsync(relative);
            return CreatedAtAction(nameof(GetRelativeById), new { id = createdRelative.RelativeId }, createdRelative);
        }

        // PUT: api/relatives/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Relative>> UpdateRelative(int id, [FromBody] Relative relative)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedRelative = await _relativeService.UpdateRelativeAsync(id, relative);
            if (updatedRelative == null)
                return NotFound(new { message = $"Không tìm thấy thân nhân với ID {id}" });
            return Ok(updatedRelative);
        }

        // DELETE: api/relatives/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRelative(int id)
        {
            var success = await _relativeService.DeleteRelativeAsync(id);
            if (!success)
                return NotFound(new { message = $"Không tìm thấy thân nhân với ID {id}" });
            return Ok(new { message = "Xóa thân nhân thành công" });
        }
    }
}
