using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { 
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};

try{

    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();

    channel.ExchangeDeclare("chat", ExchangeType.Fanout);

    channel.QueueDeclare(queue: "chat-eduardoworrel",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

    channel.QueueBind(queue:  "chat-eduardoworrel",
                            exchange: "chat",
                            routingKey: "");
                            

    
    RecebeMensagensChat(channel);
    


    Console.WriteLine("escreva seu nome");

    var nome = Console.ReadLine();
    var message = "";
    do{
        Console.WriteLine("escreva /sair para sair");
        message = Console.ReadLine();

        if(message != "/sair"){
        
            var body = Encoding.UTF8.GetBytes("["+nome+"]:" + message);
        
            channel.BasicPublish(exchange: "chat",
                            routingKey: "",
                            basicProperties: null,
                            body: body);
        }
        
    }while(message != "/sair");

}catch(Exception e){
   
}
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();


static void RecebeMensagensChat(IModel channel){
    var consumer = new EventingBasicConsumer(channel);
   
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(message);
    };
    channel.BasicConsume(queue: "chat-eduardoworrel",
                            autoAck: false,
                            consumer: consumer);
}