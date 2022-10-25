using Cryptocop.Software.API.Services.Interfaces;
using System;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        public void PublishMessage(string routingKey, object body)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // TODO: Dispose the connection and channel
            throw new NotImplementedException();
        }
    }
}