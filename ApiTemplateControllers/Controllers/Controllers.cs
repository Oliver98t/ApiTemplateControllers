using Microsoft.AspNetCore.Mvc;
using ApiTemplateControllers.Models;
using ApiTemplateControllers.BaseController;
using Microsoft.EntityFrameworkCore;
using ApiTemplateControllers.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiTemplateControllers.Controllers;

public class UsersController : Controller<User>
{
    public UsersController(ApiContext context) : base(context)
    {
    }
}

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