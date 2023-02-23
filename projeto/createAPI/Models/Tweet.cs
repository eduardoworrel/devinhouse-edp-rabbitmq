using System;

public class Tweet
{
    public string Email { get; set; }
    public string Mensagem { get; set; }
    public string Ipv4 { get; set; }
    public DateTime DataPublicacao { get; set; } = DateTime.Now;

}
