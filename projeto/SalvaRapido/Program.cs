using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.QueueDeclare(queue: "SalvaRapidoQueue",
                           durable: true,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Tweet tweet = JsonConvert.DeserializeObject<Tweet>(message);
    Console.WriteLine($"Recebido {tweet.Mensagem} de {tweet.Email}.");

    using (HttpClient client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://localhost:5151");
        //Está zoado!!
        var content = new FormUrlEncodedContent(new[]
        {
        new KeyValuePair<string, string>("corDeFundo", "yellow"),
        new KeyValuePair<string, string>("corDoTexto", "black"),
        new KeyValuePair<string, string>("tamanhoDaFonte", "160px"),
        new KeyValuePair<string, string>("conteudoDinamico", "Salvê by from form")
    });

        var result = await client.PostAsync("/WeatherForecast/SalvaPrefencia", content);
        Console.WriteLine(result.RequestMessage);
        string resultContent = await result.Content.ReadAsStringAsync();
        Console.WriteLine(resultContent);
        Console.WriteLine(content);
    }



};

channel.BasicConsume(queue: "SalvaRapidoQueue",
                        autoAck: true,
                        consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();