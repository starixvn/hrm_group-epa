using System.ComponentModel.DataAnnotations;

namespace SMS.EPA.Models.Authentication;

public class LoginModel
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string ReturnPath { get; set; }
}