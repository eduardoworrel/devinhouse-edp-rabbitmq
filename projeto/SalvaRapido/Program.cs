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
    TweetViewModel tweet = JsonConvert.DeserializeObject<TweetViewModel>(message);
    Console.WriteLine($"Recebido {tweet.Mensagem} de {tweet.Email}.");
    SalvaTweet(tweet);

};

channel.BasicConsume(queue: "SalvaRapidoQueue",
                        autoAck: true,
                        consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

async Task SalvaTweet(TweetViewModel tweet)
{
    using var ctx = new SalvaRapidoContext();
    ctx.Tweets.Add(new Tweet
    {
        Id = 0,
        Email = tweet.Email,
        Mensagem = tweet.Mensagem,
        Ipv4 = tweet.Ipv4,
        DataPublicacao = (DateTime)tweet.DataPublicacao,
        Status = 0
    });
    await ctx.SaveChangesAsync();


}