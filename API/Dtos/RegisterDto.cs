﻿using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; }
    [Required][EmailAddress]
    public string Email { get; set; }
    [Required]
    [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase letter, 1 Digit, 1 non alphanumeric and ate least 6 characters" )]
    public string Password { get; set; }
}