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

public class UsersService
{
    private ApiContext _context;

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