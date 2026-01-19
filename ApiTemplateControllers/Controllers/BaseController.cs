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
 * BaseController.cs - Base controller class providing common functionality for all API controllers.
 * Contains shared methods, properties, and utilities that can be inherited by specific controllers.
 */

using Microsoft.AspNetCore.Mvc;
using ApiTemplateControllers.Models;
using ApiTemplateControllers.BaseServices;
using Microsoft.AspNetCore.Authorization;

namespace ApiTemplateControllers.BaseController;

/// <summary>
/// Generic base controller providing common CRUD operations for entities.
/// Contains shared HTTP endpoints and utilities that can be inherited by specific controllers.
/// </summary>
/// <typeparam name="TModel">The entity model type that implements IBaseModel.</typeparam>
[Route("api/[controller]")]
[ApiController]
public class Controller<TModel> : ControllerBase
    where TModel : class, IBaseModel
{
    protected readonly ApiContext _context;
    private readonly BaseService<TModel> _service;

    /// <summary>
    /// Initializes a new instance of the Controller class.
    /// </summary>
    /// <param name="context">The database context for data operations.</param>
    public Controller(ApiContext context)
    {
        _context = context;
        _service = new(context);
    }

    /// <summary>
    /// Gets all entities of the specified type.
    /// </summary>
    /// <returns>A collection of all entities.</returns>
    // GET: api/Item
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TModel>>> GetAll()
    {
        return await _service.GetAll();
    }

    // GET: api/Item/5
    [Authorize]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<TModel>> Get(long id)
    {
        return await _service.Get(id);
    }

    // PUT: api/Item/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize]
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Put(long id, TModel item)
    {
        return await _service.Put(id, item);
    }

    // POST: api/Item
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<User>> Post(TModel item)
    {
        await _service.Post(item);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    // DELETE: api/Item/5
    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        return await _service.Delete(id);
    }
}