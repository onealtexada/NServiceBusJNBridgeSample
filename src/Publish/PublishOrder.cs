using NServiceBus;
using Shared;
using System;
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

            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ExcludeAssemblies("JNBJavaEntry2_x64.dll", "JNBShare.dll", "JNBSharedMem_x64.dll");

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
