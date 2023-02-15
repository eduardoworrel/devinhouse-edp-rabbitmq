using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

var factory = new ConnectionFactory() { 
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
    };
    
using(var connection = factory.CreateConnection())
using(var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "hello2",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

    var consumer = new EventingBasicConsumer(channel);
    
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Received {0}", message.ToUpper());
    };
    //SE NÃO DEFINO O BIND, O EXCHANGE VIRA O PADRÃO E O ROUTING KEY VIRA O NAME DA QUEUE
    channel.BasicConsume(queue: "hello",
                            autoAck: true,
                            consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}