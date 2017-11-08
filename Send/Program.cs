using System;
using System.Text;
using RabbitMQ.Client;

namespace Send
{


    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost",UserName="roy",Password= "roy" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var message = new SignalRMessage {MessageBody = "Hello World!"};

                    var body = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(message));

                    channel.QueueDeclare(queue: "parserqueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    channel.BasicPublish(exchange: "",
                        routingKey: "parserqueue",
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" [x] sent {0}", message);

                }
            }
        }
    }

    public class SignalRMessage
    {
        public Guid ItineraryId { get; set; }
        public string MessageBody { get; set; }
    }
}
