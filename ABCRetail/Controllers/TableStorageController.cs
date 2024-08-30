using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using ABCRetail.Service;
using System.Threading.Tasks;

namespace ABCRetail.Controllers
{
    public class TableStorageController : Controller
    {
        private readonly DataTable _storageService;

        public TableStorageController(DataTable storageService)
        {
            _storageService = storageService;
        }

        // Customer Actions

        public async Task<IActionResult> Index()
        {
            var tableClient = _storageService.GetTableClient("Customers");
            var customers = await _storageService.ListEntitiesAsync<CustomerProfile>(tableClient);
            return View(customers);
        }

        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerProfile customer)
        {
            var tableClient = _storageService.GetTableClient("Customers");
            await _storageService.InsertOrMergeEntityAsync(tableClient, customer);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditCustomer(string partitionKey, string rowKey)
        {
            var tableClient = _storageService.GetTableClient("Customers");
            var customer = await _storageService.RetrieveEntityAsync<CustomerProfile>(tableClient, partitionKey, rowKey);
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(CustomerProfile customer)
        {
            var tableClient = _storageService.GetTableClient("Customers");
            await _storageService.InsertOrMergeEntityAsync(tableClient, customer);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CustomerDetails(string partitionKey, string rowKey)
        {
            var tableClient = _storageService.GetTableClient("Customers");
            var customer = await _storageService.RetrieveEntityAsync<CustomerProfile>(tableClient, partitionKey, rowKey);
            return View(customer);
        }

        public async Task<IActionResult> DeleteCustomer(string partitionKey, string rowKey)
        {
            var tableClient = _storageService.GetTableClient("Customers");
            var customer = await _storageService.RetrieveEntityAsync<CustomerProfile>(tableClient, partitionKey, rowKey);
            return View(customer);
        }

        [HttpPost, ActionName("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomerConfirmed(string partitionKey, string rowKey)
        {
            var tableClient = _storageService.GetTableClient("Customers");
            await _storageService.DeleteEntityAsync(tableClient, partitionKey, rowKey);
            return RedirectToAction("Index");
        }

        // Product Actions

        public async Task<IActionResult> IndexProduct()
        {
            var tableClient = _storageService.GetTableClient("Products");
            var products = await _storageService.ListEntitiesAsync<Product>(tableClient);
            return View(products);
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var tableClient = _storageService.GetTableClient("Products");
            await _storageService.InsertOrMergeEntityAsync(tableClient, product);
            return RedirectToAction("IndexProduct");
        }

        public async Task<IActionResult> EditProduct(string partitionKey, string rowKey)
        {
            var tableClient = _storageService.GetTableClient("Products");
            var product = await _storageService.RetrieveEntityAsync<Product>(tableClient, partitionKey, rowKey);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            var tableClient = _storageService.GetTableClient("Products");
            await _storageService.InsertOrMergeEntityAsync(tableClient, product);
            return RedirectToAction("IndexProduct");
        }

        public async Task<IActionResult> ProductDetails(string partitionKey, string rowKey)
        {
            var tableClient = _storageService.GetTableClient("Products");
            var product = await _storageService.RetrieveEntityAsync<Product>(tableClient, partitionKey, rowKey);
            return View(product);
        }

        public async Task<IActionResult> DeleteProduct(string partitionKey, string rowKey)
        {
            var tableClient = _storageService.GetTableClient("Products");
            var product = await _storageService.RetrieveEntityAsync<Product>(tableClient, partitionKey, rowKey);
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteProductConfirmed(string partitionKey, string rowKey)
        {
            var tableClient = _storageService.GetTableClient("Products");
            await _storageService.DeleteEntityAsync(tableClient, partitionKey, rowKey);
            return RedirectToAction("IndexProduct");
        }
    }
}
