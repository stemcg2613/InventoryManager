using InventoryManagerAPI.DataAccess;
using InventoryManagerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemDataAccess _dataAccess = new ItemDataAccess();

        [HttpGet]
        public ActionResult<List<Item>> GetItems()
        {
            return Ok(_dataAccess.GetItems());
        }

        [HttpPost]
        public IActionResult AddItem([FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _dataAccess.AddItem(item);

            return Ok(item);
        }
    }
}