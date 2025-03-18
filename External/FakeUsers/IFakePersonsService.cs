namespace SimpleSocialApp.External.AI
{
    public interface IFakePersonService
    {
        public Task<string> FetchRandomImageAsync();

        public string DetectGenderAsync(string imagePath);
    }
}
