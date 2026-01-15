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