using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApiTemplateControllers.Models;

public class UserInput
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

[Index(nameof(Email), IsUnique = true)]
public class User : IBaseModel
{
    public long Id { get; set; }
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    public string? HashedPassword { get; set; }
}

public class Item : IBaseModel
{
    public long Id { get; set; }
    public string? String { get; set; }
    public int Int { get; set; }
}