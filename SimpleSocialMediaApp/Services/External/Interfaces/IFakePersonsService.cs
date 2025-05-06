﻿namespace SimpleSociaMedialApp.Services.External.Interfaces
{
    public interface IFakePersonService
    {
        public Task<string> FetchRandomImageAsync();

        public string DetectGenderAsync(string imagePath);
    }
}
