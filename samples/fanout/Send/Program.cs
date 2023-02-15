using System;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { 
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};
try{
    
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

    channel.ExchangeDeclare("logs", ExchangeType.Fanout);

    channel.QueueDeclare(queue: "helloA",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

    channel.QueueDeclare(queue: "helloB",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

    channel.QueueBind(queue:  "helloA",
                            exchange: "logs",
                            routingKey: "");
                            
    channel.QueueBind(queue:  "helloB",
                            exchange: "logs",
                            routingKey: "");
                            
    string message = "Hello World!";
    
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "logs",
                        routingKey: "",
                        basicProperties: null,
                        body: body);

    Console.WriteLine(" [x] Sent {0}", message);


}catch(Exception e){
    Console.WriteLine(e.Message);
}
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();