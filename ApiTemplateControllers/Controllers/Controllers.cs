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
 * Controllers.cs - Main API controllers implementing business logic endpoints.
 * Contains the primary REST API controllers for handling entity operations and business workflows.
 */

using Microsoft.AspNetCore.Mvc;
using ApiTemplateControllers.Models;
using ApiTemplateControllers.BaseController;
using Microsoft.EntityFrameworkCore;
using ApiTemplateControllers.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiTemplateControllers.Controllers;

/// <summary>
/// Controller for managing user-related operations and endpoints.
/// Handles HTTP requests for user creation, retrieval, updating, and deletion.
/// Implements custom actions on base routes for user management.
/// </summary>
// implemented using custom actions on base routes
[Route("api/[controller]")]
[ApiController]
public class UsersController: ControllerBase
{
    private readonly ApiContext _context;
    
    /// <summary>
    /// Initializes a new instance of the UsersController class.
    /// </summary>
    /// <param name="context">The database context for user data operations.</param>
    public UsersController(ApiContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all users from the database.
    /// </summary>
    /// <returns>A collection of all users.</returns>
    // GET: api/Item
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/Item/5
    [Authorize]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<User>> Get(long id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return new NotFoundResult();
        }
        return user;
    }

    // PUT: api/Item/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Put(long id, UserInput userInput)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
        {
            return new NotFoundResult();
        }

        existingUser.Email = userInput.Email;
        existingUser.Name = userInput.Name;
        if (!string.IsNullOrEmpty(userInput.Password))
        {
            existingUser.HashedPassword = AuthService.HashPassword(userInput.Password);
        }

        _context.Entry(existingUser).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!Exists(id))
            {
                return new NotFoundResult();
            }
            else
            {
                throw;
            }
        }

        return new NoContentResult();
    }

    // POST: api/Item
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<User>> Post(UserInput userInput)
    {
        User newUser = new User();
        newUser.Email = userInput.Email;
        newUser.Name = userInput.Name;
        newUser.HashedPassword = AuthService.HashPassword(userInput.Password ?? string.Empty);
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = userInput.Id }, userInput);
    }

    // DELETE: api/Item/5
    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return new NotFoundResult();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return new NoContentResult();
    }

    private bool Exists(long id)
    {
        if (_context == null)
        {
            throw new InvalidOperationException("Operations DbSet is not initialized");
        }
        return _context.Users.Any(e => e.Id == id);
    }
}


// implemented using generic contorller class
public class ItemsController : Controller<Item>
{
    private ItemsService _service;
    public ItemsController(ApiContext context) : base(context)
    {
        _service = new(context);
    }

    // example of how to set custom route
    [Authorize]
    [HttpGet("special/path/{amount:int}")]
    public async Task<ActionResult<IEnumerable<Item>>> GetAmount(int amount)
    {
        return await _service.GetAmount(amount);
    }
}