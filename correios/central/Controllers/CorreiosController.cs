using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
namespace central.Controllers;

[ApiController]
[Route("api/correios")]
public class CorreiosController : ControllerBase
{
    private ConnectionFactory factory;
    public CorreiosController(){
        this.factory = new ConnectionFactory() { 
            HostName = "142.93.173.18",
            UserName = "admin",
            Password = "devintwitter"
        };
    }


    [HttpPost(Name = "Post")]
    public bool Post([FromForm] Carta carta)
    {
        try{
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {

                var body = JsonConvert.SerializeObject(carta);

                var cartaBytes = Encoding.UTF8.GetBytes(body);
                Console.WriteLine( carta.Destino.ToString());
                channel.BasicPublish(exchange: "correios",
                                routingKey: carta.Destino.ToString(),
                                basicProperties: null,
                                body: cartaBytes);
            }
            return true;
        }catch(Exception e){
            throw e;
            return false;
        }

        
    }
}
