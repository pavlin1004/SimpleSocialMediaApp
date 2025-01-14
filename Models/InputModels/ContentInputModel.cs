using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Models.InputModels
{

    public class ContentInputModel
    {    
        public string? Text { get; set; }      

        public ICollection<IFormFile>? MediaFiles{ get; set; }
    }
}
