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
 * CRUD.cs - Generic CRUD (Create, Read, Update, Delete) operations service.
 * Provides reusable database operations for entities, including querying, filtering, and data manipulation.
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTemplateControllers.Models;

using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;

namespace ApiTemplateControllers.BaseServices;

public class CRUD<TModel> where TModel : class, IBaseModel
{
    protected readonly ApiContext _context;
    private DbSet<TModel>? _operations = null;

    public CRUD(ApiContext context)
    {
        _context = context;

        PropertyInfo[] properties = _context.GetType().GetProperties();
        foreach (var prop in properties)
        {
            if( prop.PropertyType == typeof(DbSet<TModel>))
            {
                var value = prop.GetValue(_context);
                if (value != null)
                {
                    _operations = (DbSet<TModel>)value;
                    return;
                }
            }
        }
        if (_operations == null)
        {
            throw new InvalidOperationException("Operations DbSet is not initialized");
        }
    }

    public async Task<ActionResult<IEnumerable<TModel>>> GetAll()
    {
        if (_operations == null)
        {
            throw new InvalidOperationException("Operations DbSet is not initialized");
        }
        return await _operations.ToListAsync();
    }

    public async Task<ActionResult<TModel>> Get(long id)
    {
        if (_operations == null)
        {
            throw new InvalidOperationException("Operations DbSet is not initialized");
        }

        var item = await _operations.FindAsync(id);

        if (item == null)
        {
            return new NotFoundResult();
        }

        return item;
    }

    public async Task<IActionResult> Put(long id, TModel item)
    {
        if (id != item.Id)
        {
            return new BadRequestResult();
        }

        _context.Entry(item).State = EntityState.Modified;

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

    public async Task<ActionResult<TModel>> Post(TModel item)
    {
        if (_operations == null)
        {
            throw new InvalidOperationException("Operations DbSet is not initialized");
        }
        _operations.Add(item);
        await _context.SaveChangesAsync();

        return new OkObjectResult(item);
    }

    public async Task<IActionResult> Delete(long id)
    {
        if (_operations == null)
        {
            throw new InvalidOperationException("Operations DbSet is not initialized");
        }
        var item = await _operations.FindAsync(id);
        if (item == null)
        {
            return new NotFoundResult();
        }

        _operations.Remove(item);
        await _context.SaveChangesAsync();

        return new NoContentResult();
    }

    private bool Exists(long id)
    {
        if (_operations == null)
        {
            throw new InvalidOperationException("Operations DbSet is not initialized");
        }
        return _operations.Any(e => e.Id == id);
    }
}