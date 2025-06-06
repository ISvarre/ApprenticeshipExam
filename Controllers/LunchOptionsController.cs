using Microsoft.AspNetCore.Mvc;
using EPiServer.Data.Dynamic;
using System.Linq;
using ApprenticeshipExam.Models;
using EPiServer.Data;

namespace ApprenticeshipExam.Controllers;

[Route("api/lunchoptions")]
public class LunchOptionsController : ControllerBase
{
    private readonly DynamicDataStore _store = DynamicDataStoreFactory.Instance.CreateStore(typeof(LunchOptions));

    private void EnsureDefaultLunchOptions()
    {
        var items = _store.Items<LunchOptions>().ToList();
        var default1 = new LunchOptions { Id = Identity.NewIdentity(), Name = "Kantina", Description = "Dagens varme måltider"};
        var default2 = new LunchOptions { Id = Identity.NewIdentity(), Name = "Lokalet", Description = "Påsmurt og delt måltid"};
        
        if (!items.Any(x => x.Name == default1.Name))
            _store.Save(default1);
        if (!items.Any(x => x.Name == default2.Name))
            _store.Save(default2); 
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        EnsureDefaultLunchOptions();
        return Ok(_store.Items<LunchOptions>().ToList());
    }

    [HttpPost]
    public IActionResult Add([FromBody] LunchOptions option)
    {
        EnsureDefaultLunchOptions();
        _store.Save(option);
        return Ok(option);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] LunchOptions option)
    {
        EnsureDefaultLunchOptions();
        option.Id = Identity.Parse(id.ToString());
        _store.Save(option);
        return Ok(option);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        EnsureDefaultLunchOptions();
        _store.Delete(id);
        return NoContent();
    }
}