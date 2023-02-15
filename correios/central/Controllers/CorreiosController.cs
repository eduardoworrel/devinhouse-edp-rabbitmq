using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
namespace central.Controllers;

[ApiController]
[Route("api/correios")]
public class CorreiosController : ControllerBase
{
    private IModel channel;
    public CorreiosController(){
        var factory = new ConnectionFactory() { 
            HostName = "142.93.173.18",
            UserName = "admin",
            Password = "devintwitter"
        };
        var connection = factory.CreateConnection();
        this.channel = connection.CreateModel();
    }


    [HttpPost(Name = "Post")]
    public bool Post([FromForm] Carta carta)
    {
        var cartaBytes = Encoding.UTF8.GetBytes(carta);

        channel.BasicPublish(exchange: "correios",
                        routingKey: carta.Destino,
                        basicProperties: null,
                        body: cartaBytes);

        
    }
}
