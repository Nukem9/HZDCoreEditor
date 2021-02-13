using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using Utility;

namespace Decima.HZD
{
    /// <summary>
    /// Typedef alias for BaseRef
    /// </summary>
    public class Ref<T> : BaseRef
    {
        public Ref() : base(typeof(T))
        {
        }
    }

    /// <summary>
    /// Typedef alias for BaseStreamingRef
    /// </summary>
    public class StreamingRef<T> : BaseStreamingRef<T>
    {
    }

    /// <summary>
    /// Typedef alias for BaseUUIDRef
    /// </summary>
    public class UUIDRef<T> : BaseUUIDRef<T>
    {
    }

    /// <summary>
    /// Typedef alias for BaseCPtr
    /// </summary>
    public class CPtr<T> : BaseCPtr<T>
    {
    }

    /// <summary>
    /// Typedef alias for BaseWeakPtr
    /// </summary>
    public class WeakPtr<T> : BaseWeakPtr<T>
    {
    }

    /// <summary>
    /// Typedef alias for BaseArray
    /// </summary>
    public class Array<T> : BaseArray<T>
    {
        public Array() 
            : base() { }

        public Array(int capacity) 
            : base(capacity) { }

        public Array(T[] source)
            : base(source.Length)
        {
            for (int i = 0; i < source.Length; i++)
                Add(source[i]);
        }
    }

    public class GlobalRenderVariableInfo_GLOBAL_RENDER_VAR_COUNT<T> : Array<T>
    {
    }

    public class float_GLOBAL_RENDER_VAR_COUNT<T> : Array<T>
    {
    }

    public class uint64_PLACEMENT_LAYER_MASK_SIZE<T> : Array<T>
    {
    }

    public class uint16_PBD_MAX_SKIN_WEIGHTS<T> : Array<T>
    {
    }

    public class uint8_PBD_MAX_SKIN_WEIGHTS<T> : Array<T>
    {
    }

    public class ShaderProgramResourceSet_36<T> : Array<T>
    {
    }

    public class float_GLOBAL_APP_RENDER_VAR_COUNT<T> : Array<T>
    {
    }

    public class GlobalAppRenderVariableInfo_GLOBAL_APP_RENDER_VAR_COUNT<T> : Array<T>
    {
    }

    public class EnvelopeSegment_MAX_ENVELOPE_SEGMENTS<T> : Array<T>
    {
    }

    /// <summary>
    /// Typedef alias for BaseHashMap
    /// </summary>
    public class HashMap<T> : BaseHashMap<T>
    {
    }

    /// <summary>
    /// Typedef alias for BaseHashSet
    /// </summary>
    public class HashSet<T> : BaseHashSet<T>
    {
    }

    /// <summary>
    /// Typedef alias for BaseString
    /// </summary>
    public class String : BaseString
    {
        public String() : this("")
        {
        }

        public String(string value) : base(value)
        {
        }

        public static implicit operator string(String value)
        {
            return value.Value;
        }

        public static implicit operator String(string value)
        {
            return new String(value);
        }
    }

    /// <summary>
    /// Alias for a regular String
    /// </summary>
    public class Filename : String
    {
    }

    /// <summary>
    /// Typedef alias for BasedWString
    /// </summary>
    public class WString : BaseWString
    {
    }

    [DebuggerDisplay("{Value}")]
    public class uint128 : RTTI.ISerializable, RTTI.ISaveSerializable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BigInteger Value;

        public void Deserialize(BinaryReader reader)
        {
            Value = new BigInteger(reader.ReadBytesStrict(16));
        }
        public void Serialize(BinaryWriter writer) => throw new NotImplementedException();
        public void DeserializeStateObject(SaveState state) => throw new NotImplementedException();
        public void SerializeStateObject(SaveState state) => throw new NotImplementedException();
    }
}
