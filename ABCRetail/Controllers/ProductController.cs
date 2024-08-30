﻿using ABCRetail.Models;
using ABCRetail.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ABCRetail.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataTable _dataTableService;

        public ProductController(DataTable dataTableService)
        {
            _dataTableService = dataTableService;
        }

        // GET: Product/Index
        public async Task<IActionResult> Index()
        {
            var tableClient = _dataTableService.GetTableClient("Products");
            var products = await _dataTableService.ListEntitiesAsync<Product>(tableClient);
            return View(products);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                var tableClient = _dataTableService.GetTableClient("Products");
                await _dataTableService.InsertOrMergeEntityAsync(tableClient, product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(string partitionKey, string rowKey)
        {
            var tableClient = _dataTableService.GetTableClient("Products");
            var product = await _dataTableService.RetrieveEntityAsync<Product>(tableClient, partitionKey, rowKey);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string partitionKey, string rowKey, Product product)
        {
            if (partitionKey != product.PartitionKey || rowKey != product.RowKey)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var tableClient = _dataTableService.GetTableClient("Products");
                await _dataTableService.InsertOrMergeEntityAsync(tableClient, product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(string partitionKey, string rowKey)
        {
            var tableClient = _dataTableService.GetTableClient("Products");
            var product = await _dataTableService.RetrieveEntityAsync<Product>(tableClient, partitionKey, rowKey);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            var tableClient = _dataTableService.GetTableClient("Products");
            var product = await _dataTableService.RetrieveEntityAsync<Product>(tableClient, partitionKey, rowKey);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            var tableClient = _dataTableService.GetTableClient("Products");
            await _dataTableService.DeleteEntityAsync(tableClient, partitionKey, rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
