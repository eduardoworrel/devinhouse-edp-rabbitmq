using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;

var factory = new ConnectionFactory()
{
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        outro.Carta carta = JsonConvert.DeserializeObject<outro.Carta>(message);
        Console.WriteLine("Agencia de Foz do Iguaçu interceptou esta carta:\n", carta.Conteudo);
    };

    channel.BasicConsume(queue: "foz-do-iguaçu",
                            autoAck: true,
                            consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}