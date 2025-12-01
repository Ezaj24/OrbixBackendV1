namespace OrbixBackend.Models
{
    public class RefineRequest
    {
        public string Input { get; set; } = "";
        public string Mode { get; set; } = "ChatGPT";
        public string Language { get; set; } = "English";
    }
}
