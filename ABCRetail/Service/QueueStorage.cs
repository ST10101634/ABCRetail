using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Threading.Tasks;

namespace ABCRetail.Service
{
    public class QueueStorageService
    {
        private readonly QueueServiceClient _queueServiceClient;

        public QueueStorageService(QueueServiceClient queueServiceClient)
        {
            _queueServiceClient = queueServiceClient;
        }

        // Add message to a queue
        public async Task AddMessageToQueueAsync(string queueName, string message)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(message);
        }

        // Receive message from a queue
        public async Task<string> ReceiveMessageFromQueueAsync(string queueName)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            var response = await queueClient.ReceiveMessagesAsync(maxMessages: 1);
            if (response.Value.Length > 0)
            {
                var message = response.Value[0];
                await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                return message.MessageText;
            }
            return null;
        }

       
        public async Task<string> GetMessageByQueueNameAsync(string queueName)
        {
            
            return await ReceiveMessageFromQueueAsync(queueName);
        }

        // Update message in queue
        public async Task UpdateMessageInQueueAsync(string queueName, string message)
        {
            
           
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            var response = await queueClient.ReceiveMessagesAsync(maxMessages: 1);
            if (response.Value.Length > 0)
            {
                var oldMessage = response.Value[0];
                await queueClient.DeleteMessageAsync(oldMessage.MessageId, oldMessage.PopReceipt);
                await queueClient.SendMessageAsync(message);
            }
        }

        // Delete message from queue
        public async Task DeleteMessageFromQueueAsync(string queueName)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            var response = await queueClient.ReceiveMessagesAsync(maxMessages: 1);
            if (response.Value.Length > 0)
            {
                var message = response.Value[0];
                await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }
        }
    }
}
