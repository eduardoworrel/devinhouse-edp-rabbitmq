using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { 
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};

using(var connection = factory.CreateConnection()) 
using(var channel = connection.CreateModel())
{
    var eventoTudoAzul =  new EventingBasicConsumer(channel);
    var eventoCarroAzul =  new EventingBasicConsumer(channel);
    var eventoMotoHondaVermelha =  new EventingBasicConsumer(channel);

    eventoTudoAzul.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [eventoTudoAzul] recebido '{0}'", message);
    };

    eventoCarroAzul.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [eventoCarroAzul] recebido '{0}'", message);
    };

    eventoMotoHondaVermelha.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [eventoMotoHondaVermelha] recebido '{0}'", message);
    };

    channel.BasicConsume(queue: "helloC",
                            autoAck: true,
                            consumer: eventoMotoHondaVermelha);

    channel.BasicConsume(queue: "helloB",
                            autoAck: true,
                            consumer: eventoCarroAzul);

    channel.BasicConsume(queue: "helloA",
                                autoAck: true,
                                consumer: eventoTudoAzul);

    Console.ReadLine();
}