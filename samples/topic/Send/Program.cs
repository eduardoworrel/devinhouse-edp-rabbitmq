using System;
using RabbitMQ.Client;
using System.Text;
using System.Collections.Generic;

var factory = new ConnectionFactory() { 
    HostName = "142.93.173.18",
    UserName = "admin",
    Password = "devintwitter"
};
try{
    
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

    channel.ExchangeDeclare("topicos", ExchangeType.Topic);

    channel.QueueDeclare(queue: "helloA",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

    channel.QueueBind(queue:  "helloA",
                            exchange: "topicos",
                            routingKey: "#.azul");
                            //fila quer processar meio de transporte azul
                            // de qualquer marca
    channel.QueueDeclare(queue: "helloB",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

    channel.QueueBind(queue:  "helloB",
                            exchange: "topicos",
                            routingKey: "carro.*.azul");
                            //fila quer processar carros azuis
                            // de qualquer marca

    channel.QueueDeclare(queue: "helloC",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);


    channel.QueueBind(queue:  "helloC",
                            exchange: "topicos",
                            routingKey: "moto.honda.vermelho");
                            // fila especifica

                            

    var messages = new Dictionary<string,string>{
        {"moto.bmw.azul","100" }, // A
        {"carro.bmw.vermelho","200" }, // nao lida
        {"moto.honda.azul" ,"300"}, // A
        {"carro.fiat.azul" ,"500"}, // A e B
        {"moto.honda.vermelho" ,"600"}, // C
        {"bicicleta.caloi.azul" ,"600"}, // C
        {"bicicleta.caloi.vermelho" ,"600"}, // C
        // exemplo insifuciente uma vez que as keys poderiam se repetir e a estrutura Dictionary não o permite
    }; 

    foreach(var m in messages){
        
        var body = Encoding.UTF8.GetBytes(m.Value + "routinkey:"+m.Key);

        channel.BasicPublish(exchange: "topicos",
                            routingKey: m.Key,
                            basicProperties: null,
                            body: body);

        Console.WriteLine(" [x] Sent {0}", m.Value);
    }
   


}catch(Exception e){
    Console.WriteLine(e.Message);
}
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();