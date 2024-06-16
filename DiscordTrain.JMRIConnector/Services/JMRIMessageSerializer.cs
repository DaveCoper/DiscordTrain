using System.Diagnostics;
using System.Globalization;
using System.Text;
using DiscordTrain.JMRIConnector.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace DiscordTrain.JMRIConnector.Services
{
    public class JMRIMessageSerializer : IMessageSerializer
    {
        /*
        public byte[] Serialize<TData>(TData? data)
        {
            var typeName = typeof(TData).Name;

            if (typeName.EndsWith("Data"))
            {
                typeName = typeName.Remove(typeName.Length - "Data".Length);
            }

            typeName = typeName.Substring(0, 1).ToLower() + typeName.Substring(1);
            var message = new JMRIMessage<TData?>
            {
                Data = data,
                Type = typeName
            };

            return Serialize(message);
        }

        public byte[] Serialize(JMRIMessage message)
        {
            var jsonOptions = GetSerializerSettings();
            var messageString = JsonConvert.SerializeObject(message, jsonOptions);

            Debug.WriteLine($"output: {0}", messageString);

            var messageBytes = Encoding.UTF8.GetBytes(messageString);

            return messageBytes;
        }

        public IEnumerable<JMRIMessage> Deserialize(string json)
        {
            Debug.WriteLine($"input: {0}", json);
            var message = JToken.Parse(json);

            if (message == null)
                throw new NotImplementedException();

            switch (message.Type)
            {
                case JTokenType.Object:
                    return [Deserialize((JObject)message)];

                case JTokenType.Array:
                    return Deserialize((JArray)message);
            }

            throw new NotImplementedException();
        }

        private IEnumerable<JMRIMessage> Deserialize(JArray message)
        {
            foreach (var token in message)
            {
                if (token.Type == JTokenType.Object)
                {
                    yield return Deserialize((JObject)token);
                }
            }
        }

        private JMRIMessage Deserialize(JObject message)
        {
            var type = message.Value<string>("type");
            switch (type)
            {
                case "ping":
                case "pong":
                    return message.ToObject<JMRIMessage>() ?? throw new NotImplementedException();

                case "hello":
                    return Deserialize<HelloData>(message);

                case "throttle":
                    return Deserialize<ThrottleData>(message);

                case "rosterEntry":
                    return Deserialize<RosterEntryData>(message);

                case "error":
                    return Deserialize<ErrorData>(message);

                default:
                    throw new NotImplementedException();
            }
        }

        private JMRIMessage<TData> Deserialize<TData>(JObject obj)
        {
            var settings = GetSerializerSettings();
            var serializer = JsonSerializer.CreateDefault(settings);
            var message = obj.ToObject<JMRIMessage<TData>>(serializer);
            return message ?? throw new NotImplementedException();
        }
        */

        public string Serialize<TMessage>(TMessage message)
        {
            var settings = GetSerializerSettings();
            return JsonConvert.SerializeObject(message, settings);
        }

        public TOut? Deserialize<TOut>(string json)
        {
            var settings = GetSerializerSettings();
            return JsonConvert.DeserializeObject<TOut>(json, settings);
        }

        private JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Culture = CultureInfo.InvariantCulture,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
        }
    }
}