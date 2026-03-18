using Microsoft.AspNetCore.Mvc;
using PostManagementApp.Models;
using PostManagementApp.Services;

namespace PostManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GravesController : ControllerBase
    {
        private readonly IGraveService _graveService;

        public GravesController(IGraveService graveService)
        {
            _graveService = graveService;
        }

        // GET: api/graves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grave>>> GetAllGraves()
        {
            var graves = await _graveService.GetAllGravesAsync();
            return Ok(graves);
        }

        // GET: api/graves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grave>> GetGraveById(int id)
        {
            var grave = await _graveService.GetGraveByIdAsync(id);
            if (grave == null)
                return NotFound(new { message = $"Không tìm thấy mộ với ID {id}" });
            return Ok(grave);
        }

        // POST: api/graves
        [HttpPost]
        public async Task<ActionResult<Grave>> CreateGrave([FromBody] Grave grave)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdGrave = await _graveService.CreateGraveAsync(grave);
            return CreatedAtAction(nameof(GetGraveById), new { id = createdGrave.GraveId }, createdGrave);
        }

        // PUT: api/graves/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Grave>> UpdateGrave(int id, [FromBody] Grave grave)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedGrave = await _graveService.UpdateGraveAsync(id, grave);
            if (updatedGrave == null)
                return NotFound(new { message = $"Không tìm thấy mộ với ID {id}" });
            return Ok(updatedGrave);
        }

        // DELETE: api/graves/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGrave(int id)
        {
            var success = await _graveService.DeleteGraveAsync(id);
            if (!success)
                return NotFound(new { message = $"Không tìm thấy mộ với ID {id}" });
            return Ok(new { message = "Xóa mộ thành công" });
        }
    }
}
