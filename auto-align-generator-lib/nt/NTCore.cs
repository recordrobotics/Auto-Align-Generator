using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoAlignGenerator.nt
{
    internal partial class NTCore
    {
        /** Default network tables port number (NT4) */
        internal const uint NT_DEFAULT_PORT4 = 5810;

        [LibraryImport("ntcore.dll")]
        internal static partial NT_Inst NT_GetDefaultInstance();

        [DllImport("ntcore")]
        internal static extern NT_Subscriber NT_Subscribe(NT_Topic topic, NT_Type type, ref readonly WPI_String typeStr, ref readonly NT_PubSubOptions options);

        [DllImport("ntcore")]
        internal static extern NT_Topic NT_GetTopic(NT_Inst inst, ref readonly WPI_String name);

        [LibraryImport("ntcore")]
        internal static partial NT_Bool NT_GetTopicInfo(NT_Topic topic, ref NT_TopicInfo info);

        [DllImport("ntcore")]
        internal static extern void NT_StartClient4(NT_Inst inst, ref readonly WPI_String identity);

        [LibraryImport("ntcore")]
        internal static partial void NT_SetServerTeam(NT_Inst inst, uint team, uint port = NT_DEFAULT_PORT4);

        [LibraryImport("ntcore")]
        internal static partial void NT_StartDSClient(NT_Inst inst, uint port = NT_DEFAULT_PORT4);

        [DllImport("ntcore")]
        internal static extern NT_Bool NT_GetBoolean(NT_Handle subentry, NT_Bool defaultValue);

        [DllImport("ntcore")]
        internal static extern double NT_GetDouble(NT_Handle subentry, double defaultValue);

        [LibraryImport("ntcore")]
        internal static unsafe partial byte* NT_GetRaw(NT_Handle subentry, byte* defaultValue, uint defaultValueLen, ref uint len);

        internal static ReadOnlyMemory<byte> NT_GetRaw(NT_Handle subentry, ReadOnlyMemory<byte> defaultValue)
        {
            uint len = (uint)defaultValue.Length;
            unsafe
            {
                byte* ptr = NT_GetRaw(subentry, (byte*)defaultValue.Pin().Pointer, len, ref len);
                Memory<byte> buf = new Memory<byte>(new byte[len], 0, (int)len);
                new ReadOnlySpan<byte>(ptr, (int)len).CopyTo(buf.Span);
                return buf;
            }
        }

        [LibraryImport("ntcore")]
        internal static partial NT_Bool NT_IsConnected(NT_Inst inst);

        [LibraryImport("ntcore")]
        internal static partial void NT_StopDSClient(NT_Inst inst);

        [LibraryImport("ntcore")]
        internal static partial void NT_StopClient(NT_Inst inst);

        internal static string NT_TypeToStr(NT_Type type)
        {
            return type switch
            {
                NT_Type.NT_BOOLEAN => "boolean",
                NT_Type.NT_DOUBLE => "double",
                NT_Type.NT_STRING => "string",
                NT_Type.NT_RAW => "raw",
                NT_Type.NT_BOOLEAN_ARRAY => "boolean[]",
                NT_Type.NT_DOUBLE_ARRAY => "double[]",
                NT_Type.NT_STRING_ARRAY => "string[]",
                _ => throw new Exception("Unsupported Type")
            };
        }
    }

    #region Types

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_Handle
            : IEquatable<NT_Handle>
    {
        internal readonly uint Value;

        internal NT_Handle(uint value) => this.Value = value;
        internal static NT_Handle Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator uint(NT_Handle value) => value.Value;

        public static explicit operator NT_Handle(uint value) => new NT_Handle(value);

        public static bool operator ==(NT_Handle left, NT_Handle right) => left.Value == right.Value;

        public static bool operator !=(NT_Handle left, NT_Handle right) => !(left == right);

        public bool Equals(NT_Handle other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_Handle other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_Bool
            : IEquatable<NT_Bool>
    {
        internal readonly int Value;

        internal NT_Bool(int value) => this.Value = value;
        internal NT_Bool(bool value) => this.Value = value ? 1 : 0;
        internal static NT_Bool Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator int(NT_Bool value) => value.Value;
        public static implicit operator bool(NT_Bool value) => value.Value != 0;

        public static explicit operator NT_Bool(int value) => new NT_Bool(value);
        public static explicit operator NT_Bool(bool value) => new NT_Bool(value);

        public static bool operator ==(NT_Bool left, NT_Bool right) => left.Value == right.Value;

        public static bool operator !=(NT_Bool left, NT_Bool right) => !(left == right);

        public bool Equals(NT_Bool other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_Bool other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)this.Value);

        public override string ToString() => this.Value == 0 ? "False" : "True";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_ConnectionDataLogger
            : IEquatable<NT_ConnectionDataLogger>
    {
        internal readonly NT_Handle Value;

        internal NT_ConnectionDataLogger(NT_Handle value) => this.Value = value;
        internal static NT_ConnectionDataLogger Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator NT_Handle(NT_ConnectionDataLogger value) => value.Value;

        public static explicit operator NT_ConnectionDataLogger(NT_Handle value) => new NT_ConnectionDataLogger(value);

        public static bool operator ==(NT_ConnectionDataLogger left, NT_ConnectionDataLogger right) => left.Value == right.Value;

        public static bool operator !=(NT_ConnectionDataLogger left, NT_ConnectionDataLogger right) => !(left == right);

        public bool Equals(NT_ConnectionDataLogger other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_ConnectionDataLogger other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)(uint)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_DataLogger
            : IEquatable<NT_DataLogger>
    {
        internal readonly NT_Handle Value;

        internal NT_DataLogger(NT_Handle value) => this.Value = value;
        internal static NT_DataLogger Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator NT_Handle(NT_DataLogger value) => value.Value;

        public static explicit operator NT_DataLogger(NT_Handle value) => new NT_DataLogger(value);

        public static bool operator ==(NT_DataLogger left, NT_DataLogger right) => left.Value == right.Value;

        public static bool operator !=(NT_DataLogger left, NT_DataLogger right) => !(left == right);

        public bool Equals(NT_DataLogger other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_DataLogger other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)(uint)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_Entry
            : IEquatable<NT_Entry>
    {
        internal readonly NT_Handle Value;

        internal NT_Entry(NT_Handle value) => this.Value = value;
        internal static NT_Entry Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator NT_Handle(NT_Entry value) => value.Value;

        public static explicit operator NT_Entry(NT_Handle value) => new NT_Entry(value);

        public static bool operator ==(NT_Entry left, NT_Entry right) => left.Value == right.Value;

        public static bool operator !=(NT_Entry left, NT_Entry right) => !(left == right);

        public bool Equals(NT_Entry other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_Entry other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)(uint)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_Inst
            : IEquatable<NT_Inst>
    {
        internal readonly NT_Handle Value;

        internal NT_Inst(NT_Handle value) => this.Value = value;
        internal static NT_Inst Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator NT_Handle(NT_Inst value) => value.Value;

        public static explicit operator NT_Inst(NT_Handle value) => new NT_Inst(value);

        public static bool operator ==(NT_Inst left, NT_Inst right) => left.Value == right.Value;

        public static bool operator !=(NT_Inst left, NT_Inst right) => !(left == right);

        public bool Equals(NT_Inst other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_Inst other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)(uint)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_Listener
            : IEquatable<NT_Listener>
    {
        internal readonly NT_Handle Value;

        internal NT_Listener(NT_Handle value) => this.Value = value;
        internal static NT_Listener Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator NT_Handle(NT_Listener value) => value.Value;

        public static explicit operator NT_Listener(NT_Handle value) => new NT_Listener(value);

        public static bool operator ==(NT_Listener left, NT_Listener right) => left.Value == right.Value;

        public static bool operator !=(NT_Listener left, NT_Listener right) => !(left == right);

        public bool Equals(NT_Listener other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_Listener other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)(uint)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_ListenerPoller
            : IEquatable<NT_ListenerPoller>
    {
        internal readonly NT_Handle Value;

        internal NT_ListenerPoller(NT_Handle value) => this.Value = value;
        internal static NT_ListenerPoller Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator NT_Handle(NT_ListenerPoller value) => value.Value;

        public static explicit operator NT_ListenerPoller(NT_Handle value) => new NT_ListenerPoller(value);

        public static bool operator ==(NT_ListenerPoller left, NT_ListenerPoller right) => left.Value == right.Value;

        public static bool operator !=(NT_ListenerPoller left, NT_ListenerPoller right) => !(left == right);

        public bool Equals(NT_ListenerPoller other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_ListenerPoller other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)(uint)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_MultiSubscriber
            : IEquatable<NT_MultiSubscriber>
    {
        internal readonly NT_Handle Value;

        internal NT_MultiSubscriber(NT_Handle value) => this.Value = value;
        internal static NT_MultiSubscriber Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator NT_Handle(NT_MultiSubscriber value) => value.Value;

        public static explicit operator NT_MultiSubscriber(NT_Handle value) => new NT_MultiSubscriber(value);

        public static bool operator ==(NT_MultiSubscriber left, NT_MultiSubscriber right) => left.Value == right.Value;

        public static bool operator !=(NT_MultiSubscriber left, NT_MultiSubscriber right) => !(left == right);

        public bool Equals(NT_MultiSubscriber other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_MultiSubscriber other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)(uint)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_Topic
            : IEquatable<NT_Topic>
    {
        internal readonly NT_Handle Value;

        internal NT_Topic(NT_Handle value) => this.Value = value;
        internal static NT_Topic Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator NT_Handle(NT_Topic value) => value.Value;

        public static explicit operator NT_Topic(NT_Handle value) => new NT_Topic(value);

        public static bool operator ==(NT_Topic left, NT_Topic right) => left.Value == right.Value;

        public static bool operator !=(NT_Topic left, NT_Topic right) => !(left == right);

        public bool Equals(NT_Topic other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_Topic other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)(uint)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_Subscriber
            : IEquatable<NT_Subscriber>
    {
        internal readonly NT_Handle Value;

        internal NT_Subscriber(NT_Handle value) => this.Value = value;
        internal static NT_Subscriber Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator NT_Handle(NT_Subscriber value) => value.Value;

        public static explicit operator NT_Subscriber(NT_Handle value) => new NT_Subscriber(value);

        public static bool operator ==(NT_Subscriber left, NT_Subscriber right) => left.Value == right.Value;

        public static bool operator !=(NT_Subscriber left, NT_Subscriber right) => !(left == right);

        public bool Equals(NT_Subscriber other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_Subscriber other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)(uint)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    [DebuggerDisplay("{Value}")]
    internal readonly partial struct NT_Publisher
            : IEquatable<NT_Publisher>
    {
        internal readonly NT_Handle Value;

        internal NT_Publisher(NT_Handle value) => this.Value = value;
        internal static NT_Publisher Null => default;

        internal bool IsNull => Value == default;

        public static implicit operator NT_Handle(NT_Publisher value) => value.Value;

        public static explicit operator NT_Publisher(NT_Handle value) => new NT_Publisher(value);

        public static bool operator ==(NT_Publisher left, NT_Publisher right) => left.Value == right.Value;

        public static bool operator !=(NT_Publisher left, NT_Publisher right) => !(left == right);

        public bool Equals(NT_Publisher other) => this.Value == other.Value;

        public override bool Equals(object? obj) => obj is NT_Publisher other && this.Equals(other);

        public override int GetHashCode() => unchecked((int)(uint)this.Value);

        public override string ToString() => $"0x{(nuint)this.Value:x}";
    }

    #endregion

    #region Enums

    /** NetworkTables data types. */
    internal enum NT_Type
    {
        NT_UNASSIGNED = 0,
        NT_BOOLEAN = 0x01,
        NT_DOUBLE = 0x02,
        NT_STRING = 0x04,
        NT_RAW = 0x08,
        NT_BOOLEAN_ARRAY = 0x10,
        NT_DOUBLE_ARRAY = 0x20,
        NT_STRING_ARRAY = 0x40,
        NT_RPC = 0x80,
        NT_INTEGER = 0x100,
        NT_FLOAT = 0x200,
        NT_INTEGER_ARRAY = 0x400,
        NT_FLOAT_ARRAY = 0x800
    };

    /** NetworkTables entry flags. */
    enum NT_EntryFlags
    {
        NT_PERSISTENT = 0x01,
        NT_RETAINED = 0x02,
        NT_UNCACHED = 0x04
    };

    /** NetworkTables logging levels. */
    enum NT_LogLevel
    {
        NT_LOG_CRITICAL = 50,
        NT_LOG_ERROR = 40,
        NT_LOG_WARNING = 30,
        NT_LOG_INFO = 20,
        NT_LOG_DEBUG = 10,
        NT_LOG_DEBUG1 = 9,
        NT_LOG_DEBUG2 = 8,
        NT_LOG_DEBUG3 = 7,
        NT_LOG_DEBUG4 = 6
    };

    /** Client/server modes */
    enum NT_NetworkMode
    {
        NT_NET_MODE_NONE = 0x00,     /* not running */
        NT_NET_MODE_SERVER = 0x01,   /* running in server mode */
        NT_NET_MODE_CLIENT3 = 0x02,  /* running in NT3 client mode */
        NT_NET_MODE_CLIENT4 = 0x04,  /* running in NT4 client mode */
        NT_NET_MODE_STARTING = 0x08, /* flag for starting (either client or server) */
        NT_NET_MODE_LOCAL = 0x10,    /* running in local-only mode */
    };

    /** Event notification flags. */
    enum NT_EventFlags
    {
        NT_EVENT_NONE = 0,
        /** Initial listener addition. */
        NT_EVENT_IMMEDIATE = 0x01,
        /** Client connected (on server, any client connected). */
        NT_EVENT_CONNECTED = 0x02,
        /** Client disconnected (on server, any client disconnected). */
        NT_EVENT_DISCONNECTED = 0x04,
        /** Any connection event (connect or disconnect). */
        NT_EVENT_CONNECTION = NT_EVENT_CONNECTED | NT_EVENT_DISCONNECTED,
        /** New topic published. */
        NT_EVENT_PUBLISH = 0x08,
        /** Topic unpublished. */
        NT_EVENT_UNPUBLISH = 0x10,
        /** Topic properties changed. */
        NT_EVENT_PROPERTIES = 0x20,
        /** Any topic event (publish, unpublish, or properties changed). */
        NT_EVENT_TOPIC = NT_EVENT_PUBLISH | NT_EVENT_UNPUBLISH | NT_EVENT_PROPERTIES,
        /** Topic value updated (via network). */
        NT_EVENT_VALUE_REMOTE = 0x40,
        /** Topic value updated (local). */
        NT_EVENT_VALUE_LOCAL = 0x80,
        /** Topic value updated (network or local). */
        NT_EVENT_VALUE_ALL = NT_EVENT_VALUE_REMOTE | NT_EVENT_VALUE_LOCAL,
        /** Log message. */
        NT_EVENT_LOGMESSAGE = 0x100,
        /** Time synchronized with server. */
        NT_EVENT_TIMESYNC = 0x200,
    };

    #endregion

    #region Structs

    /**
     * A const UTF8 string.
    */
    [DebuggerDisplay("{ToString()}")]
    internal readonly struct WPI_String : IDisposable
    {
        /** Contents. */
        private readonly nint str;
        /** Length */
        private readonly uint len;

        public WPI_String(string str)
        {
            this.str = Marshal.StringToHGlobalAnsi(str);
            this.len = (uint)str.Length;
        }

        public readonly bool IsNull => str == 0;

        public static implicit operator WPI_String(string str) => new WPI_String(str);

        public static unsafe implicit operator ReadOnlySpan<char>(WPI_String str) => new ReadOnlySpan<char>(str.str.ToPointer(), (int)str.len);

        public static implicit operator string?(WPI_String str) => str.IsNull ? null : Marshal.PtrToStringAnsi(str.str, (int)str.len);

        public static implicit operator IntPtr(WPI_String str) => (IntPtr)str.str;

        public static unsafe implicit operator ReadOnlySpan<byte>(WPI_String str) => new ReadOnlySpan<byte>(str.str.ToPointer(), (int)str.len);

        public override string? ToString() => (string?)this;

        public void Dispose()
        {
            Marshal.FreeHGlobal((IntPtr)str);
        }
    };

    /** NetworkTables publish/subscribe options. */
    internal struct NT_PubSubOptions
    {
        /**
         * Structure size. Must be set to sizeof(NT_PubSubOptions).
         */
        public uint structSize;

        /**
         * Polling storage size for a subscription. Specifies the maximum number of
         * updates NetworkTables should store between calls to the subscriber's
         * ReadQueue() function. If zero, defaults to 1 if sendAll is false, 20 if
         * sendAll is true.
         */
        public uint pollStorage;

        /**
         * How frequently changes will be sent over the network, in seconds.
         * NetworkTables may send more frequently than this (e.g. use a combined
         * minimum period for all values) or apply a restricted range to this value.
         * The default is 100 ms.
         */
        public double periodic;

        /**
         * For subscriptions, if non-zero, value updates for ReadQueue() are not
         * queued for this publisher.
         */
        public NT_Publisher excludePublisher;

        /**
         * Send all value changes over the network.
         */
        public NT_Bool sendAll;

        /**
         * For subscriptions, don't ask for value changes (only topic announcements).
         */
        public NT_Bool topicsOnly;

        /**
         * Perform prefix match on subscriber topic names. Is ignored/overridden by
         * Subscribe() functions; only present in struct for the purposes of getting
         * information about subscriptions.
         */
        public NT_Bool prefixMatch;

        /**
         * Preserve duplicate value changes (rather than ignoring them).
         */
        public NT_Bool keepDuplicates;

        /**
         * For subscriptions, if remote value updates should not be queued for
         * ReadQueue(). See also disableLocal.
         */
        public NT_Bool disableRemote;

        /**
         * For subscriptions, if local value updates should not be queued for
         * ReadQueue(). See also disableRemote.
         */
        public NT_Bool disableLocal;

        /**
         * For entries, don't queue (for ReadQueue) value updates for the entry's
         * internal publisher.
         */
        public NT_Bool excludeSelf;

        /**
         * For subscriptions, don't share the existence of the subscription with the
         * network. Note this means updates will not be received from the network
         * unless another subscription overlaps with this one, and the subscription
         * will not appear in metatopics.
         */
        public NT_Bool hidden;

        public NT_PubSubOptions()
        {
            structSize = (uint)Marshal.SizeOf<NT_PubSubOptions>();
            pollStorage = 0;
            periodic = 0.1;
            excludePublisher = NT_Publisher.Null;
            sendAll = (NT_Bool)false;
            topicsOnly = (NT_Bool)false;
            prefixMatch = (NT_Bool)false;
            keepDuplicates = (NT_Bool)false;
            disableRemote = (NT_Bool)false;
            disableLocal = (NT_Bool)false;
            excludeSelf = (NT_Bool)false;
            hidden = (NT_Bool)false;
        }
    };

    /** NetworkTables Topic Information */
    internal struct NT_TopicInfo
    {
        /** Topic handle */
        public NT_Topic topic;

        /** Topic name */
        public WPI_String name;

        /** Topic type */
        public NT_Type type;

        /** Topic type string */
        public WPI_String type_str;

        /** Topic properties JSON string */
        public WPI_String properties;
    };

    #endregion
}
