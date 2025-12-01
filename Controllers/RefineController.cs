using Microsoft.AspNetCore.Mvc;
using OrbixBackend.Services;

namespace OrbixBackend.Controllers
{
    [ApiController]
    [Route("refine")]
    public class RefineController : ControllerBase
    {
        private readonly PromptRefiner _refiner;

        public RefineController(PromptRefiner refiner)
        {
            _refiner = refiner;
        }

        [HttpPost]
        public async Task<IActionResult> Refine([FromBody] RefineRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Input))
            {
                return BadRequest(new { error = "Input cannot be empty." });
            }

            string result = await _refiner.RefineAsync(
                req.Input,
                req.Mode,
                req.Language
            );

            return Ok(new { refined = result });
        }
    }

    public class RefineRequest
    {
        public string Input { get; set; } = "";
        public string Mode { get; set; } = "ChatGPT";
        public string Language { get; set; } = "English";
    }
}
