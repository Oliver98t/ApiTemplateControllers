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
 * ApiContext.cs - Entity Framework database context for the application.
 * Defines database connection, entity sets, and configuration for data access layer.
 */

using Microsoft.EntityFrameworkCore;

namespace ApiTemplateControllers.Models;

/// <summary>
/// Entity Framework database context for the API Template Controllers application.
/// Manages database connections, entity sets, and provides data access functionality.
/// </summary>
public class ApiContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the ApiContext class with the specified options.
    /// </summary>
    /// <param name="options">The database context options for configuration.</param>
    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the Users entity set for user data operations.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the Items entity set for item data operations.
    /// </summary>
    public DbSet<Item> Items { get; set; } = null!;
}