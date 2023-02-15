using System;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { 
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};
using(var connection = factory.CreateConnection())
using(var channel = connection.CreateModel())
{
    
    //3 tipos, erro, numero e texto
    channel.ExchangeDeclare("multiple", ExchangeType.Direct);


    channel.QueueDeclare(queue: "erros",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

    channel.QueueDeclare(queue: "all",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
    


    channel.QueueBind(queue: "erros",
                        exchange: "multiple",
                        routingKey: "erros");
                            
            
    channel.QueueBind(queue: "all",
                        exchange: "multiple",
                        routingKey: "erros"); //considera os erros também
                        
    channel.QueueBind(queue: "all",
                        exchange: "multiple",
                        routingKey: "numero");
                
   channel.QueueBind(queue: "all",
                        exchange: "multiple",
                        routingKey: "texto");
                
    
    var bodyErro = Encoding.UTF8.GetBytes("warning");
    var bodyNumero = Encoding.UTF8.GetBytes("1");
    var bodyTexto = Encoding.UTF8.GetBytes("Olá");

    channel.BasicPublish(exchange: "multiple",
                        routingKey: "erros",
                        basicProperties: null,
                        body: bodyErro);

    channel.BasicPublish(exchange: "multiple",
                        routingKey: "numero",
                        basicProperties: null,
                        body: bodyNumero);

    channel.BasicPublish(exchange: "multiple",
                        routingKey: "texto",
                        basicProperties: null,
                        body: bodyTexto);

}

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();