using System.Text;

namespace OrbixBackend.Services
{
    public class PromptRefiner
    {
        private readonly GroqService _groq;

        public PromptRefiner(GroqService groq)
        {
            _groq = groq;
        }

        public async Task<string> RefineAsync(string input, string mode, string language)
        {
            string systemPrompt =
            """
            You are ORBIX Prompt Refiner.

            Your only job is to rewrite and improve the user's prompt.
            You MUST NOT answer the prompt.
            You MUST only output the refined prompt.

            RULES:
            - Keep the intent of the original prompt.
            - Improve clarity, grammar, and structure.
            - Adapt tone based on the requested mode.
            - Output must be in the selected language.
            - If the input is not in that language, translate it first, then refine.

            Never give an answer - only produce a better version of the prompt.
            """;

            var builder = new StringBuilder();

            builder.AppendLine(GetModeInstruction(mode));
            builder.AppendLine(GetLanguageInstruction(language));
            builder.AppendLine();
            builder.AppendLine("Rewrite this prompt only (do NOT answer it):");
            builder.AppendLine(input);

            string finalPrompt = builder.ToString();
            return await _groq.GenerateAsync(systemPrompt, finalPrompt);
        }

        private string GetModeInstruction(string mode)
        {
            return mode switch
            {
                "Creative" => "Tone: creative, expressive, and vivid.",
                "Coding" => "Tone: technical, clear, and precise for developers.",
                "Marketing" => "Tone: persuasive, benefit-driven, high-conversion.",
                "Teaching" => "Tone: simple, beginner-friendly, educational.",
                "Claude" => "Tone: thoughtful, analytical, and structured.",
                "ChatGPT" => "Tone: balanced, neutral, and professional.",
                _ => "Tone: professional and clear."
            };
        }

        private string GetLanguageInstruction(string lang)
        {
            return lang switch
            {
                "Hindi" => "Output ONLY in Hindi. Translate if needed.",
                "Urdu" => "Output ONLY in Urdu. Translate if needed.",
                "Arabic" => "Output ONLY in Arabic. Translate if needed.",
                "Spanish" => "Output ONLY in Spanish. Translate if needed.",
                "English" => "Output ONLY in English. Translate if needed.",
                _ => "Output ONLY in English."
            };
        }
    }
}
