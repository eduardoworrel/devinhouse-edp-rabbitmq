using System;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { 
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};

using(var connection = factory.CreateConnection())
using(var channel = connection.CreateModel()){

channel.ExchangeDeclare("correios", ExchangeType.Direct);


channel.QueueDeclare(queue: "foz-do-iguaçu",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

channel.QueueDeclare(queue: "Paraná",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

channel.QueueDeclare(queue: "Nacional",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);



channel.QueueBind(queue: "Nacional",
                    exchange: "correios",
                    routingKey: "Santa Catarina");
                    
channel.QueueBind(queue: "Nacional",
                    exchange: "correios",
                    routingKey: "São Paulo");

channel.QueueBind(queue: "Paraná",
                    exchange: "correios",
                    routingKey: "Cafelandia");

channel.QueueBind(queue: "foz-do-iguaçu",
                    exchange: "correios",
                    routingKey: "Jardim Paraná");
                    
Console.WriteLine("Tudo criado");                
}         