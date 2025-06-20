using Microsoft.AspNetCore.Mvc;
using EPiServer.Data.Dynamic;
using System.Linq;
using ApprenticeshipExam.Models;
using EPiServer.Data;

namespace ApprenticeshipExam.Controllers;

[Route("api/shopitems")]
public class ShopItemsController : ControllerBase
{
    private readonly DynamicDataStore _store = DynamicDataStoreFactory.Instance.CreateStore(typeof(ShopItems));
    
    public class BoughtStatusUpdate
    {
        public bool Bought { get; set; }
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_store.Items<ShopItems>().ToList());
    }

    [HttpPost]
    public IActionResult Add([FromBody] ShopItems item)
    {
        if (item == null)
        {
            return BadRequest("Invalid item data.");
        }
        _store.Save(item);
        return Ok(item);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] ShopItems item)
    {
        item.Id = Identity.Parse(id.ToString());
        _store.Save(item);
        return Ok(item);
    }
    
    [HttpPatch("{id}")]
    public IActionResult UpdateBoughtStatus(Guid id, [FromBody] BoughtStatusUpdate statusUpdate  )
    {
        var item = _store.Items<ShopItems>().FirstOrDefault(x => x.Id.ExternalId == id);
        if (item == null)
        {
            return NotFound($"Item with ID {id} not found.");
        }

        item.Bought = statusUpdate.Bought;
        _store.Save(item);

        return Ok(item);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _store.Delete(id);
        return NoContent();
    }
}