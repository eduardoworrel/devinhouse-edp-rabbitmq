using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var factory = new ConnectionFactory() { 
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};

var list = new Dictionary<int,dynamic>();


app.MapPost("/", (Postagem postagem) => {

    using(var connection = factory.CreateConnection()) //disposable
    using(var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "SalvaRapidoQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

    }
});

app.Run();
