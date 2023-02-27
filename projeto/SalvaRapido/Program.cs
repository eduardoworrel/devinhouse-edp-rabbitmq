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

consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    TweetViewModel tweetModel = JsonConvert.DeserializeObject<TweetViewModel>(message);
    Console.WriteLine($"Recebido {tweetModel.Mensagem} de {tweetModel.Email}.");
    
    try{

        Tweet tweet = await SalvaTweet(tweetModel);
        await PublicaNaCidadeQueue(channel, tweet);
        await PublicaNaGPTQueue(channel, tweet);

    }catch(Exception e){
        Console.WriteLine("falha ao salvar");
    }

};

channel.BasicConsume(queue: "SalvaRapidoQueue",
                        autoAck: true,
                        consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();


async Task PublicaNaCidadeQueue(IModel channel, Tweet tweet){


    channel.QueueDeclare(queue: "CidadeQueue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

    var stringJson = JsonConvert.SerializeObject(new {
        Id = tweet.Id,
        Ipv4 = tweet.Ipv4
    });

    var body = Encoding.UTF8.GetBytes(stringJson);

    channel.BasicPublish(exchange: string.Empty,
                            routingKey: "CidadeQueue",
                            basicProperties: null,
                            body: body);

}
async Task PublicaNaGPTQueue(IModel channel, Tweet tweet){


    channel.QueueDeclare(queue: "GPTQueue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

    var stringJson = JsonConvert.SerializeObject(new {
        Id = tweet.Id,
        Mensagem = tweet.Mensagem
    });

    var body = Encoding.UTF8.GetBytes(stringJson);

    channel.BasicPublish(exchange: string.Empty,
                            routingKey: "GPTQueue",
                            basicProperties: null,
                            body: body);

}

async Task<Tweet> SalvaTweet(TweetViewModel tweetViewModel)
{
   
    using var ctx = new CoreApiContext();
    var tweet = new Tweet
    {
        Id = 0,
        Email = tweetViewModel.Email,
        Mensagem = tweetViewModel.Mensagem,
        Ipv4 = tweetViewModel.Ipv4,
        DataPublicacao = (DateTime)tweetViewModel.DataPublicacao,
        Status = 0,

    };

    ctx.Tweets.Add(tweet);
    await ctx.SaveChangesAsync();

    return tweet;
}