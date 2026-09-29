using InventoryManagerAPI.Models;

namespace InventoryManagerAPI.DataAccess
{
    public class ItemDataAccess
    {
        private static readonly List<Item> items = new();

        public List<Item> GetItems()
        {
            return items;
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }
    }
}