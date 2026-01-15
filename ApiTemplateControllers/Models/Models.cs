using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApiTemplateControllers.Models;

[Index(nameof(Email), IsUnique = true)]
public class User : IBaseModel
{
    public long Id { get; set; }
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class Item : IBaseModel
{
    public long Id { get; set; }
    public string? String { get; set; }
    public int Int { get; set; }
}