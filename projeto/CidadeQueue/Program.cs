using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Net;

var factory = new ConnectionFactory()
{
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.QueueDeclare(queue: "CidadeQueue",
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
    Console.WriteLine($"Recebido {tweetModel.Ipv4} de {tweetModel.Id}.");
    
    try{
        
        using var cxt = new CoreApiContext();
        Tweet tweet = cxt.Tweets.First(e => e.Id == tweetModel.Id);
        
        if(tweet == null){
             throw new Exception("Id invalido");
        }

        tweet.Cidade = "Desconhecido";        
        
        if(tweetModel.Ipv4 != "::1"){
            tweet.Cidade = BuscaCidadeByIPV4(tweet.Ipv4);
        }
        
        cxt.Entry(tweet).State = EntityState.Modified;

        await cxt.SaveChangesAsync();
        Console.WriteLine("Salvou");

    }catch(Exception e){
        Console.WriteLine(e);
    }

};

channel.BasicConsume(queue: "CidadeQueue",
                        autoAck: true,
                        consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();



string BuscaCidadeByIPV4(string ipv4){

    string info = new WebClient().DownloadString("http://ipinfo.io/" + ipv4);
    var ipInfo = JsonConvert.DeserializeObject<dynamic>(info);
    return ipInfo.city;
}
