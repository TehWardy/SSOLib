using System;
using SharedObjects.Extensions;

namespace Security.Data.Brokers.Serialization
{
	public class SerializationBroker : ISerializationBroker
	{
		public SerializationBroker()
		{
		}

		public T Deserialize<T>(string input)
			=> SharedObjects.Data.ParseJson<T>(input);

		public string Serialize(object obj)
			=> obj.ToJson();
	}
}

