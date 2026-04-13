using System.Globalization;

using DiscordTrain.JMRIConnector.Messages;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace DiscordTrain.JMRIConnector.Services
{
    public class JMRIMessageSerializer : IMessageSerializer
    {

        /*
         [{"type":"type","data":{"name":"oblocks","server":true,"client":true}},{"type":"type","data":{"name":"metadata","server":true,"client":true}},{"type":"type","data":{"name":"configProfiles","server":true,"client":true}},{"type":"type","data":{"name":"idTag","server":true,"client":true}},{"type":"type","data":{"name":"type","server":true,"client":true}},{"type":"type","data":{"name":"trains","server":true,"client":true}},{"type":"type","data":{"name":"audioicon","server":true,"client":true}},{"type":"type","data":{"name":"consist","server":true,"client":true}},{"type":"type","data":{"name":"signalMast","server":true,"client":true}},{"type":"type","data":{"name":"networkService","server":true,"client":true}},{"type":"type","data":{"name":"routes","server":true,"client":true}},{"type":"type","data":{"name":"engines","server":true,"client":true}},{"type":"type","data":{"name":"signalHead","server":true,"client":true}},{"type":"type","data":{"name":"signalHeads","server":true,"client":true}},{"type":"type","data":{"name":"block","server":true,"client":true}},{"type":"type","data":{"name":"train","server":true,"client":true}},{"type":"type","data":{"name":"rollingStock","server":true,"client":true}},{"type":"type","data":{"name":"systemConnection","server":true,"client":true}},{"type":"type","data":{"name":"configProfile","server":true,"client":true}},{"type":"type","data":{"name":"list","server":true,"client":false}},{"type":"type","data":{"name":"version","server":true,"client":true}},{"type":"type","data":{"name":"layoutBlocks","server":true,"client":true}},{"type":"type","data":{"name":"cars","server":true,"client":true}},{"type":"type","data":{"name":"node","server":true,"client":true}},{"type":"type","data":{"name":"route","server":true,"client":true}},{"type":"type","data":{"name":"light","server":true,"client":true}},{"type":"type","data":{"name":"sensor","server":true,"client":true}},{"type":"type","data":{"name":"hello","server":true,"client":true}},{"type":"type","data":{"name":"schema","server":true,"client":true}},{"type":"type","data":{"name":"throttle","server":true,"client":true}},{"type":"type","data":{"name":"reporters","server":true,"client":true}},{"type":"type","data":{"name":"memory","server":true,"client":true}},{"type":"type","data":{"name":"rosterGroup","server":true,"client":true}},{"type":"type","data":{"name":"turnout","server":true,"client":true}},{"type":"type","data":{"name":"panels","server":true,"client":true}},{"type":"type","data":{"name":"ping","server":false,"client":true}},{"type":"type","data":{"name":"goodbye","server":true,"client":true}},{"type":"type","data":{"name":"layoutBlock","server":true,"client":true}},{"type":"type","data":{"name":"locale","server":false,"client":true}},{"type":"type","data":{"name":"error","server":true,"client":false}},{"type":"type","data":{"name":"pong","server":true,"client":false}},{"type":"type","data":{"name":"signalMasts","server":true,"client":true}},{"type":"type","data":{"name":"logixngicon","server":true,"client":true}},{"type":"type","data":{"name":"roster","server":true,"client":true}},{"type":"type","data":{"name":"carType","server":true,"client":true}},{"type":"type","data":{"name":"car","server":true,"client":true}},{"type":"type","data":{"name":"engine","server":true,"client":true}},{"type":"type","data":{"name":"memories","server":true,"client":true}},{"type":"type","data":{"name":"client","server":true,"client":true}},{"type":"type","data":{"name":"json","server":true,"client":true}},{"type":"type","data":{"name":"audio","server":true,"client":true}},{"type":"type","data":{"name":"power","server":true,"client":true}},{"type":"type","data":{"name":"track","server":true,"client":true}},{"type":"type","data":{"name":"panel","server":true,"client":true}},{"type":"type","data":{"name":"lights","server":true,"client":true}},{"type":"type","data":{"name":"railroad","server":true,"client":true}},{"type":"type","data":{"name":"blocks","server":true,"client":true}},{"type":"type","data":{"name":"kernel","server":true,"client":true}},{"type":"type","data":{"name":"reporter","server":true,"client":true}},{"type":"type","data":{"name":"message","server":true,"client":false}},{"type":"type","data":{"name":"turnouts","server":true,"client":true}},{"type":"type","data":{"name":"networkServices","server":true,"client":true}},{"type":"type","data":{"name":"sensors","server":true,"client":true}},{"type":"type","data":{"name":"rosterEntry","server":true,"client":true}},{"type":"type","data":{"name":"audios","server":true,"client":true}},{"type":"type","data":{"name":"consists","server":true,"client":true}},{"type":"type","data":{"name":"location","server":true,"client":true}},{"type":"type","data":{"name":"locations","server":true,"client":true}},{"type":"type","data":{"name":"rosterGroups","server":true,"client":true}},{"type":"type","data":{"name":"time","server":true,"client":true}},{"type":"type","data":{"name":"systemConnections","server":true,"client":true}},{"type":"type","data":{"name":"oblock","server":true,"client":true}}]         
         */
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

        public object? Deserialize(string json)
        {
            var settings = GetSerializerSettings();
            var token = JToken.Parse(json);
            return Deserialize(token, settings);
        }

        private object? Deserialize(JToken token, JsonSerializerSettings settings)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return Deserialize((JObject)token, settings);
                case JTokenType.Array:
                    return Deserialize((JArray)token, settings);

                default:
                    throw new NotImplementedException();

            }
        }

        private object? Deserialize(JObject obj, JsonSerializerSettings settings)
        {
            var serializer = JsonSerializer.CreateDefault(settings);
            var msg = obj.ToObject<JMRIMessage>(serializer);
            if (msg == null)
                return null;

            switch (msg.Type)
            {
                case "ping":
                case "pong":
                    return msg;

                case "hello":
                    return obj.ToObject<JMRIMessage<HelloData>>(serializer);

                case "throttle":
                    return obj.ToObject<JMRIMessage<ThrottleData>>(serializer);

                case "rosterEntry":
                    return obj.ToObject<JMRIMessage<RosterEntryData>>(serializer);

                case "error":
                    return obj.ToObject<JMRIMessage<ErrorData>>(serializer);

                case "sensor":
                    return obj.ToObject<JMRIMessage<SensorData>>(serializer);

                case "list":                    
                default:
                    throw new NotImplementedException();
            }
        }

        private object? Deserialize(JArray array, JsonSerializerSettings settings)
        {
            List<object> items = new List<object>(array.Count);
            foreach (var item in array)
            {
                var obj = Deserialize(item, settings);
                if (obj != null)
                    items.Add(obj);
            }

            throw new NotImplementedException();
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