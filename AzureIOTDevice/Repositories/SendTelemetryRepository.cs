using AzureIOT.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Repositories
{
    public class SendTelemetryRepository
    {
        public static RegistryManager? registryManager;
        public static DeviceClient? client;
        private static string connStringIotHub = "HostName=sauraviothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=i7f2+RTduSS8shWhrbG87ipeZlxpwKPbJYQfShDqJf4=HostName=sauraviothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=i7f2+RTduSS8shWhrbG87ipeZlxpwKPbJYQfShDqJf4=";
        public static string connStringDevice = "HostName=sauraviothub.azure-devices.net;DeviceId=sensor-th-001;SharedAccessKey=y+TmSV/dwR3VmFPkxz1r0+lFh/buCUMuRnPVn59R+VQ=";

        public static async Task SendMessage(string deviceId)
        {
            try
            {
                registryManager = RegistryManager.CreateFromConnectionString(connStringIotHub);
                var device = await registryManager.GetTwinAsync(deviceId);
                DevicePropertiesModel properties = new DevicePropertiesModel();
                TwinCollection Prop;
                Prop = device.Properties.Reported;
                client = DeviceClient.CreateFromConnectionString(connStringDevice,
                    Microsoft.Azure.Devices.Client.TransportType.Mqtt);

                Random rand = new Random();

                while (true)
                {
                    var telemetry = new
                    {
                        temperature = Prop["temperature"] + rand.NextDouble() * 15,
                        humidity = Prop["humidity"] + rand.NextDouble() * 20
                };
                    var telemetryString = JsonConvert.SerializeObject(telemetry);
                    var message = new Microsoft.Azure.Devices.Client.Message(
                        Encoding.ASCII.GetBytes(telemetryString));
                    await client.SendEventAsync(message);
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, telemetryString);
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
