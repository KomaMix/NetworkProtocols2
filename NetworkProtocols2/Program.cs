using MQTTnet;


var broker = "srv2.clusterfly.ru";
var port = 9991;
var username = "user_8fc82227";
var password = "vmCjMZtwghDO0";
var studentN = "5";

var factory = new MqttClientFactory();
var mqttClient = factory.CreateMqttClient();

var options = new MqttClientOptionsBuilder()
            .WithTcpServer(broker, port)
            .WithCredentials(username, password)
            .WithCleanSession()
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