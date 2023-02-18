using System;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { 
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
    };
    

using(var connection = factory.CreateConnection()) //disposable
using(var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "hello2",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
    for(var i = 0; i < 10; i++){
        string message = (i+1)+"- mensagem";
        
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                            routingKey: "hello",
                            basicProperties: null,
                            body: body);
        Console.WriteLine(" [x] Sent {0}", message);
    }
   
}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();