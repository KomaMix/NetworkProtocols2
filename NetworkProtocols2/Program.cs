using System.Buffers;
using System.Text;
using MQTTnet;
using MQTTnet.Protocol;


var broker = "srv2.clusterfly.ru";
var port = 9991;
var username = "user_0d181660";
var password = "qaf0imzefxXVQ";
var studentN = 24;

var factory = new MqttClientFactory();
var mqttClient = factory.CreateMqttClient();

var options = new MqttClientOptionsBuilder()
            .WithTcpServer(broker, port)
            .WithCredentials(username, password)
            .Build();

var connectResult = await mqttClient.ConnectAsync(options);

if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
{
    Console.WriteLine("Подключено к брокеру!");
}
else
{
    Console.WriteLine("Ошибка подключения: " + connectResult.ResultCode);
    return;
}

var topic1 = $"{username}/Student{studentN}/Value1";
var topic2 = $"{username}/Student{studentN}/Value2";
var topic3 = $"{username}/+/Value3";

var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
            .WithTopicFilter(topic1, MqttQualityOfServiceLevel.AtLeastOnce)
            .WithTopicFilter(topic2, MqttQualityOfServiceLevel.AtLeastOnce)
            .WithTopicFilter(topic3, MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

var subscribeResult = await mqttClient.SubscribeAsync(subscribeOptions);

foreach (var result in subscribeResult.Items)
{
    Console.WriteLine($"Подписка на {result.TopicFilter.Topic}: {result.ResultCode}");
}

mqttClient.ApplicationMessageReceivedAsync += async args =>
{
    var message = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
    Console.WriteLine($"Получено сообщение по топику '{args.ApplicationMessage.Topic}': {message}");
    await Task.CompletedTask;
};

Console.WriteLine("Ожидание сообщений. Нажмите Enter для выхода.");
Console.ReadLine();
