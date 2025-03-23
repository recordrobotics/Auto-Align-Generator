using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.nt
{
    public class Subscriber<T>
    {
        private readonly NT_Subscriber subscriber;
        private readonly NT_Type type;
        private readonly Topic<T> topic;

        internal Subscriber(Topic<T> topic, NT_Subscriber subscriber, NT_Type type)
        {
            this.topic = topic;
            this.subscriber = subscriber;
            this.type = type;
        }

        public T Get(T? defaultValue)
        {
            switch (type)
            {
                case NT_Type.NT_BOOLEAN:
                    if (defaultValue is not bool)
                        throw new ArgumentException("Type is not boolean type");
                    return (T)(object)NTCore.NT_GetBoolean(subscriber, (NT_Bool)(bool)(object)defaultValue);
                case NT_Type.NT_DOUBLE:
                    if (defaultValue is not double)
                        throw new ArgumentException("Type is not double type");
                    return (T)(object)NTCore.NT_GetDouble(subscriber, (double)(object)defaultValue);
                case NT_Type.NT_RAW:
                    if (defaultValue is not ReadOnlyMemory<byte>)
                        throw new ArgumentException("Type is not raw type");
                    return (T)(object)NTCore.NT_GetRaw(subscriber, (ReadOnlyMemory<byte>)(object)defaultValue);
                default:
                    throw new Exception("Unsupported type");
            }
        }

        public T Get()
        {
            return Get(default);
        }

        public A GetStruct<A>() where A : struct, IRawStruct
        {
            var info = topic.GetInfo();

            if (info.TypeStr != new A().TypeStr)
            {
                return default;
            }

            var val = Get();

            if (val is ReadOnlyMemory<byte> raw)
            {
                unsafe
                {
                    return Marshal.PtrToStructure<A>(new nint(raw.Pin().Pointer));
                }
            }

            return default;
        }
    }
}
