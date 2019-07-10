using Publish;
using System;
using System.Threading.Tasks;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.PubSub.Publisher";
        await Start().ConfigureAwait(false);
    }

    static async Task Start()
    {
        Console.WriteLine("Press '1' to publish the OrderReceived event");
        Console.WriteLine("Press any other key to exit");

        #region PublishLoop
        var publishOrder = new PublishOrder();
        publishOrder.StartEndpoint();

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key == ConsoleKey.D1)
            {
                var orderReceivedId = await publishOrder.PublishingOrder();
                Console.WriteLine($"Published OrderReceived Event with Id {orderReceivedId}.");
            }
            else
            {
                publishOrder.StopEndpoint();
                return;
            }
        }
        #endregion
    }
}
