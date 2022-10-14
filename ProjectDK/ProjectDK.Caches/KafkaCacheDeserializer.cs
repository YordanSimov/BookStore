using Confluent.Kafka;
using MessagePack;

namespace ProjectDK.Caches
{
    public class KafkaCacheDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePackSerializer.Deserialize<T>(data.ToArray());
        }
    }
}