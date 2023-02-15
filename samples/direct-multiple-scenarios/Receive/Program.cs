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

    var consumerErro = new EventingBasicConsumer(channel);
    
    consumerErro.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [Erro] Received {0}", message);
    };
    
    var consumerAll = new EventingBasicConsumer(channel);
    
    consumerAll.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [All] Received {0}", message);
    };
    
    channel.BasicConsume(queue: "erros",
                            autoAck: true,
                            consumer: consumerErro);
             
    channel.BasicConsume(queue: "all",
                            autoAck: true,
                            consumer: consumerAll);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}