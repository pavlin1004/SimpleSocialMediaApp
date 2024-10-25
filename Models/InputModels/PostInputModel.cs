using Microsoft.AspNetCore.Mvc;
using SimpleSocialApp.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace SimpleSocialApp.Models.InputModels
{

    public class PostInputModel
    {    
        public string? Text { get; set; }      
    }
}
