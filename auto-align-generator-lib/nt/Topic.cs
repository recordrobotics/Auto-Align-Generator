using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.nt
{
    public class Topic<T>
    {
        private readonly NT_Topic topic;
        private readonly NT_Type type;

        internal Topic(NT_Topic topic, NT_Type type)
        {
            this.topic = topic;
            this.type = type;
        }

        public TopicInfo GetInfo()
        {
            NT_TopicInfo info = new NT_TopicInfo();

            if (!NTCore.NT_GetTopicInfo(topic, ref info))
            {
                throw new InvalidOperationException("Failed to get topic info");
            }
            return new TopicInfo(info.name, info.type, info.type_str, info.properties);
        }

        public Subscriber<T> Subscribe(T? defaultValue)
        {
            WPI_String str = new WPI_String(NTCore.NT_TypeToStr(type));
            NT_PubSubOptions options = new NT_PubSubOptions();

            return new Subscriber<T>(this, NTCore.NT_Subscribe(topic, type, ref str, ref options), type);
        }

        public Subscriber<T> Subscribe()
        {
            return Subscribe(default);
        }

        public readonly struct TopicInfo
        {
            public string? Name { get; }
            internal NT_Type Type { get; }
            public string? TypeStr { get; }
            public string? Properties { get; }

            internal TopicInfo(string? name, NT_Type type, string? typeStr, string? properties)
            {
                Name = name;
                Type = type;
                TypeStr = typeStr;
                Properties = properties;
            }

            public override string ToString()
            {
                return $"Name: {Name}, Type: {Type}, TypeStr: {TypeStr}, Properties: {Properties}";
            }
        }
    }
}
