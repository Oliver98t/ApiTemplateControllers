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

    [Authorize]
    [HttpPost("test")]
    public async Task<ActionResult<User>> Post(UserInput userInput)
    {
        User newUser = new();
        newUser.Email = userInput.Email;
        newUser.Name = userInput.Name;
        newUser.HashedPassword = AuthService.HashPassword(userInput.Password ?? string.Empty);
        return await base.Post(newUser);
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