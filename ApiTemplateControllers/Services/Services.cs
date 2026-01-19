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
 * Services.cs - Main business logic services implementing application-specific operations.
 * Contains service classes that handle business rules, data processing, and application logic.
 */

using ApiTemplateControllers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiTemplateControllers.Services;

/*
TEMPLATE
public class YOURService
{
    private ApiContext _context;

    public YOURService(ApiContext context)
    {
        _context = context;
    }
}
*/

/// <summary>
/// Service class for managing user-related business operations.
/// Handles user creation, validation, and database operations.
/// </summary>
public class UsersService
{
    private ApiContext _context;

    /// <summary>
    /// Initializes a new instance of the UsersService class.
    /// </summary>
    /// <param name="context">The database context for user data operations.</param>

    public UsersService(ApiContext context)
    {
        _context = context;
    }
}

public class ItemsService
{
    private ApiContext _context;

    public ItemsService(ApiContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<IEnumerable<Item>>> GetAmount(int amount)
    {
        return await _context.Items.Take(amount).Order().ToListAsync();
    }
}