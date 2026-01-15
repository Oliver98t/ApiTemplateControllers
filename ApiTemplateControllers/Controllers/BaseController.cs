using Microsoft.AspNetCore.Mvc;
using ApiTemplateControllers.Models;
using ApiTemplateControllers.BaseServices;

namespace ApiTemplateControllers.BaseController;

[Route("api/[controller]")]
[ApiController]
public class Controller<TModel> : ControllerBase
    where TModel : class, IBaseModel
{
    protected readonly ApiContext _context;
    private readonly BaseService<TModel> _service;

    public Controller(ApiContext context)
    {
        _context = context;
        _service = new(context);
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TModel>>> GetAll()
    {
        return await _service.GetAll();
    }

    // GET: api/Users/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<TModel>> Get(long id)
    {
        return await _service.Get(id);
    }

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Put(long id, TModel item)
    {
        return await _service.Put(id, item);
    }

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<User>> Post(TModel item)
    {
        await _service.Post(item);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    // DELETE: api/Users/5
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        return await _service.Delete(id);
    }
}