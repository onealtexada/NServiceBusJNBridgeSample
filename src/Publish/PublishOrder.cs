using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Publish
{
    public class PublishOrder
    {
        private readonly EndpointConfiguration _endpointConfiguration;
        private IEndpointInstance _endpointInstance;

        public PublishOrder()
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.Publisher");
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            _endpointConfiguration = endpointConfiguration;
        }

        public async void StartEndpoint()
        {
            _endpointInstance = await Endpoint.Start(_endpointConfiguration).ConfigureAwait(false);
        }

        public async void StopEndpoint()
        {
            await _endpointInstance.Stop().ConfigureAwait(false);
        }

        public async Task<Guid> PublishingOrder()
        {
            var orderReceivedId = Guid.NewGuid();
            var orderReceived = new OrderReceived
            {
                OrderId = orderReceivedId
            };

            await _endpointInstance.Publish(orderReceived)
                .ConfigureAwait(false);

            return orderReceivedId;
        }
    }
}
