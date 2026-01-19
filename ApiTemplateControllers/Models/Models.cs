/*
 * Copyright (c) 2026 Oliver Tattersfield
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/**
 * Models.cs - Main data models and entities for the application.
 * Contains the primary database entity classes representing the application's data structure.
 */

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApiTemplateControllers.Models;

/// <summary>
/// Represents user input data for user creation or registration operations.
/// Contains unhashed password and user information before processing.
/// </summary>
public class UserInput: IBaseModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the user input.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Gets or sets the user's display name.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Gets or sets the user's plain text password (before hashing).
    /// </summary>
    public string? Password { get; set; }
}

/// <summary>
/// Represents a registered user in the system with secure password storage.
/// Contains user account information and authentication credentials.
/// </summary>
[Index(nameof(Email), IsUnique = true)]
public class User : IBaseModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Gets or sets the user's display name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the user's email address. Must be unique and in valid email format.
    /// </summary>
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    /// <summary>
    /// Gets or sets the user's BCrypt hashed password for secure storage.
    /// </summary>
    public string? HashedPassword { get; set; }
}

public class Item : IBaseModel
{
    public long Id { get; set; }
    public string? String { get; set; }
    public int Int { get; set; }
}