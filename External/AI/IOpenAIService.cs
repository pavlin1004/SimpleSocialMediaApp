namespace SimpleSocialApp.External.AI
{
    public interface IOpenAIService
    {
        public Task<string?> Prompt(string message, string responseFormat = "text");
    }
}
