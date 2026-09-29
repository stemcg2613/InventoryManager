using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace InventoryManagerApp
{
    public partial class DataEntryPage : ContentPage
    {
        private readonly HttpClient _httpClient;

        private readonly ObservableCollection<ItemModel> _items = new();

        public DataEntryPage()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5072/")
            };

            itemsCollectionView.ItemsSource = _items;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadItems();
        }

        private async Task LoadItems()
        {
            try
            {
                List<ItemModel>? items =
                    await _httpClient.GetFromJsonAsync<List<ItemModel>>(
                        "api/Items");

                _items.Clear();

                if (items != null)
                {
                    foreach (ItemModel item in items)
                    {
                        _items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(
                    "Connection Error",
                    $"Unable to load items: {ex.Message}",
                    "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string itemID = txtItemID.Text ?? "";
            string itemName = txtItemName.Text ?? "";
            string itemDescription = txtItemDescription.Text ?? "";

            if (string.IsNullOrWhiteSpace(itemID) ||
                string.IsNullOrWhiteSpace(itemName) ||
                string.IsNullOrWhiteSpace(itemDescription))
            {
                await DisplayAlertAsync(
                    "Missing Information",
                    "Please complete all fields before saving.",
                    "OK");

                return;
            }

            try
            {
                ItemModel item = new ItemModel
                {
                    ItemID = itemID,
                    ItemName = itemName,
                    ItemDescription = itemDescription
                };

                HttpResponseMessage response =
                    await _httpClient.PostAsJsonAsync(
                        "api/Items",
                        item);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlertAsync(
                        "Success",
                        "Item saved successfully.",
                        "OK");

                    txtItemID.Text = "";
                    txtItemName.Text = "";
                    txtItemDescription.Text = "";

                    await LoadItems();
                }
                else
                {
                    await DisplayAlertAsync(
                        "Error",
                        "The item could not be saved.",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync(
                    "Connection Error",
                    $"Unable to save the item: {ex.Message}",
                    "OK");
            }
        }
    }

    public class ItemModel
    {
        public string ItemID { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;

        public string ItemDescription { get; set; } = string.Empty;
    }
}