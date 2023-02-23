﻿using System.Reflection;
using System.Text;
using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace createApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ConnectionFactory _factory;
        public TweetController()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "142.93.173.18",
                UserName = "admin",
                Password = "devintwitter"
            };
        }

        // POST api/<TweetController>
        [HttpPost]
        public ActionResult Post([FromBody] Tweet model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);
            }
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            try {
                PublicaTweet(model, channel);
                return Ok();
            }
            catch(Exception ex) {
                return BadRequest(ex);
            }
        }

        private void PublicaTweet(Tweet model, IModel channel)
        {
            channel.QueueDeclare(queue: "SalvaRapidoQueue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
            arguments: null);

            var body = JsonConvert.SerializeObject(model);

            var modelBytes = Encoding.UTF8.GetBytes(body);
            channel.BasicPublish(exchange: string.Empty,
                            routingKey: "SalvaRapidoQueue",
                            basicProperties: null,
                            body: modelBytes);
        }

    }
}
