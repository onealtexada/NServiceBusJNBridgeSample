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
            scanner.ExcludeAssemblies("Ionic.Zip.dll", "jnbauth_x64.dll", "jnbauth_x86.dll",
                "JNBJavaEntry_x64.dll", "JNBJavaEntry_x86.dll", "JNBJavaEntry2_x64.dll", "JNBJavaEntry2_x86.dll",
                "jnbpcommon.dll", "JNBridgePlugin.11.dll", "JNBridgePlugin.dll", "JNBridgePlugin2017.dll", "JNBShare.dll",
                "JNBSharedMem_x64.dll", "JNBSharedMem_x86.dll", "JNBWPFEmbedding.dll");

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
