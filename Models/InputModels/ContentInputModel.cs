using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Data.Models;
using SimpleSocialApp.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Models.InputModels
{

    public class ContentInputModel
    {    
        public string? Content { get; set; }      

        public ICollection<IFormFile>? MediaFiles{ get; set; }
    }
}
