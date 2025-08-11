using Confluent.Kafka;
using Grpc.Core;
using KafkaGateway;

namespace KafkaProvider.Services
{
    public class KafkaProducerService : KafkaService.KafkaServiceBase
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducerService(IConfiguration config)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = config["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        }

        public override async Task<MessageReply> ProduceMessage(MessageRequest request, ServerCallContext context)
        {
            try
            {
                await _producer.ProduceAsync(request.Topic, new Message<string, string>
                {
                    Key = request.Key,
                    Value = request.Value
                });

                return new MessageReply { Success = true };
            }
            catch
            {
                return new MessageReply { Success = false };
            }
        }
    }
}
