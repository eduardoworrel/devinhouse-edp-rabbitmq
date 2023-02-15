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

    var consumerA = new EventingBasicConsumer(channel);
    
    var consumerB = new EventingBasicConsumer(channel);
    
                            
    consumerA.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [A] Received {0}", message);
    };
    consumerB.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [B] Received {0}", message);
    };


    channel.BasicConsume(queue: "helloA",
                            autoAck: true,
                            consumer: consumerA);

    channel.BasicConsume(queue: "helloB",
                            autoAck: true,
                            consumer: consumerB);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}