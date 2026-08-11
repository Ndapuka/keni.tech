//using BusinessLogicLayer.RabbitMQ;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using RabbitMQ.Client;
//using System.Text;
//namespace BusinessLogicLayer.RabbitMQ;

//public class RabbitMQProducer : IRabbitMQProducer, IAsyncDisposable
//{
//    private readonly IConfiguration _configuration;
//    private readonly ILogger<RabbitMQProducer> _logger;
//    private IConnection _connection;
//    private IChannel _channel;

//    public RabbitMQProducer(IConfiguration configuration, ILogger<RabbitMQProducer> logger)
//    {
//        _configuration = configuration;
//        _logger = logger;
//    }
//    public async Task InitAsync()
//    {
//        Console.WriteLine($"RabbitMQ_Host:{_configuration["RABBITMQ_HOST"]}");
//        Console.WriteLine($"RabbitMQ_UserName:{_configuration["RABBITMQ_USERNAME"]}");
//        Console.WriteLine($"RabbitMQ_Psw:{_configuration["RABBITMQ_PASSWORD"]}");
//        Console.WriteLine($"RabbitMQ_Port:{_configuration["RABBITMQ_PORT"]}");

//        var factory = new ConnectionFactory
//        {
//            HostName = _configuration["RABBITMQ_HOST"]!,
//            UserName = _configuration["RABBITMQ_USERNAME"]!,
//            Password = _configuration["RABBITMQ_PASSWORD"]!,
//            Port = int.Parse(_configuration["RABBITMQ_PORT"]!)
//        };
//        for(int attempt = 1; attempt<=4; attempt++) { 
//            try
//            {
//                _connection = await factory.CreateConnectionAsync();
//                _channel = await _connection.CreateChannelAsync();

//                break; // Exit the loop if connection is successful
//                _logger.LogInformation("Successfully connected to RabbitMQ on attempt {Attempt}", attempt);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Attempt {Attempt} - Failed to connect to RabbitMQ. Retrying in 2 seconds...", attempt);
//                if (attempt == 4)
//                {
//                    throw; // Rethrow the exception if it's the last attempt
//                }
//                await Task.Delay(2000); // Wait for 2 seconds before retrying
//            }
//            _logger.LogError("inSuccessfully connected to RabbitMQ on attempt {Attempt}", attempt);
//        }
//    }

//    public async Task ProducerAsync<T>(Dictionary<string, object> headers, T message)
//    {
//        try
//        {
//            if (_connection == null || !_connection.IsOpen || _channel == null || !_channel.IsOpen)
//            {
//                await InitAsync();
//            }
//            string exchangeName = _configuration["RABBITMQ_PRODUCTS_EXCHANGE"]!;
//            string messageJson = System.Text.Json.JsonSerializer.Serialize(message);
//            byte[] body = Encoding.UTF8.GetBytes(messageJson);

//            //declare exchange
//            await _channel.ExchangeDeclareAsync(
//            exchange: exchangeName,
//            type: ExchangeType.Headers,
//            //type: ExchangeType.Direct,
//            durable: true,
//            autoDelete: false);
//            _logger.LogInformation("Config exchange: {ExchangeName}", exchangeName);


//            var props = new BasicProperties();

//            props.ContentType = "text/plain";
//            props.Headers = headers!;
//            props.Persistent = true; //message will be persisted to disk
//            props.DeliveryMode = (DeliveryModes)2; //make message persistent

//            //publish message to exchange
//            await _channel.BasicPublishAsync(
//            exchange: exchangeName,
//            //routingKey: routingKey,
//            routingKey: string.Empty, //Fanout, headers
//            mandatory: false,
//            basicProperties: props,
//            body: body);
//            _logger.LogInformation("Message published to exchange: {ExchangeName}", exchangeName);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error publish message RabbitMQ ");
//            throw;
//        }

//    }
//    public async ValueTask DisposeAsync()
//    {
//        if (_channel != null) await _channel.DisposeAsync();
//        if (_connection != null) await _connection.DisposeAsync();
//    }
//}



