using Microsoft.AspNetCore.Mvc;
using OrbixBackend.Services;

namespace OrbixBackend.Controllers
{
    [ApiController]
    [Route("chat")]
    public class ChatController : ControllerBase
    {
        private readonly GroqService _groq;

        public ChatController(GroqService groq)
        {
            _groq = groq;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Input))
            {
                return BadRequest(new { error = "Input cannot be empty." });
            }

            string systemPrompt =
            """
            You are ORBIX, a premium AI assistant created by Ezaj Shaikh.

            IDENTITY:
            - You are ORBIX.
            - You were created by Ezaj Shaikh.
            - You belong to the ORBIX platform (Chat + Prompt Refinement).

            RULES:
            - Never say you are ChatGPT, Claude, Grok, or any other AI.
            - Never mention training data cut-off dates.
            - If user asks about your knowledge, say:
              "My knowledge comes from the ORBIX AI system and is focused on general assistance."
            - If user asks “who built you?” → Answer:
              "I was built by Ezaj Shaikh."

            BEHAVIOR:
            - Respond clearly, fully, and professionally.
            - Give structured, helpful answers.
            - Adapt tone to the user (friendly, formal, or technical).
            """;

            string response = await _groq.GenerateAsync(systemPrompt, req.Input);

            return Ok(new { response });
        }
    }

    public class ChatRequest
    {
        public string Input { get; set; } = "";
    }
}
