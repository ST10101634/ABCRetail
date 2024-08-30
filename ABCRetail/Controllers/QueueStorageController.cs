using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using ABCRetail.Service;
using System.Threading.Tasks;

namespace ABCRetail.Controllers
{
    public class QueueStorageController : Controller
    {
        private readonly QueueStorageService _queueService;

        public QueueStorageController(QueueStorageService queueService)
        {
            _queueService = queueService;
        }

        // GET: /QueueStorage/SendMessage
        public IActionResult SendMessage()
        {
            return View();
        }

        // POST: /QueueStorage/SendMessage
        [HttpPost]
        public async Task<IActionResult> SendMessage(QueueMessageModel model)
        {
            try
            {
                await _queueService.AddMessageToQueueAsync(model.QueueName, model.Message);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(model);
            }
        }


        // GET: /QueueStorage/ReceiveMessage
        public async Task<IActionResult> ReceiveMessage(string queueName)
        {
            var message = await _queueService.ReceiveMessageFromQueueAsync(queueName);
            return Content(message ?? "No messages in queue.");
        }

        // GET: /QueueStorage/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /QueueStorage/Edit
        public async Task<IActionResult> Edit(string queueName)
        {
            var message = await _queueService.GetMessageByQueueNameAsync(queueName);
            if (message == null)
            {
                return NotFound();
            }

            var model = new QueueMessageModel
            {
                QueueName = queueName,
                Message = message
            };

            return View(model);
        }

        // POST: /QueueStorage/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(QueueMessageModel model)
        {
            if (ModelState.IsValid)
            {
                await _queueService.UpdateMessageInQueueAsync(model.QueueName, model.Message);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: /QueueStorage/Delete
        public async Task<IActionResult> Delete(string queueName)
        {
            var message = await _queueService.GetMessageByQueueNameAsync(queueName);
            if (message == null)
            {
                return NotFound();
            }

            var model = new QueueMessageModel
            {
                QueueName = queueName,
                Message = message
            };

            return View(model);
        }

        // POST: /QueueStorage/Delete
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string queueName)
        {
            await _queueService.DeleteMessageFromQueueAsync(queueName);
            return RedirectToAction("Index");
        }
    }
}
