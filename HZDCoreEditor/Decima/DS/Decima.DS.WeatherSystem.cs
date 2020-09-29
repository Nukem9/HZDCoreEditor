#pragma warning disable CS0649 // warning CS0649: 'member' is never assigned to, and will always have its default value 'value'.
#pragma warning disable CS0108 // warning CS0108: 'class' hides inherited member 'member'. Use the new keyword if hiding was intended.

namespace Decima.DS
{
    using int8 = System.SByte;
    using uint8 = System.Byte;
    using int16 = System.Int16;
    using uint16 = System.UInt16;
    using int32 = System.Int32;
    using uint32 = System.UInt32;
    using int64 = System.Int64;
    using uint64 = System.UInt64;

    using wchar = System.Int16;
    using ucs4 = System.Int32;

    using HalfFloat = System.UInt16;
    using LinearGainFloat = System.Single;
    using MusicTime = System.UInt64;

    using MaterialType = System.UInt16;
    using AnimationNodeID = System.UInt16;
    using AnimationTagID = System.UInt32;
    using AnimationSet = System.UInt32;
    using AnimationStateID = System.UInt32;
    using AnimationEventID = System.UInt32;
    using PhysicsCollisionFilterInfo = System.UInt32;

[RTTI.Serializable(0x8E92245843662D81, GameType.DS)]
public class WeatherSystem : CoreObject
{
	[RTTI.Member(0, 0x160, "General")] public Ref<RenderEffectResource> SimulationRenderEffectResource;
	[RTTI.Member(1, 0x1E0, "General")] public Ref<WorldDataType> PrecipitationOcclusionHeightData;
	[RTTI.Member(2, 0x380, "General")] public Array<WindSimulationForceField> WindSimulationForceFields;
	[RTTI.Member(3, 0x3D0, "General")] public BoundingBox3 WorldBounds;
	[RTTI.Member(4, 0x3F0, "General")] public FRange TemperatureRange;
	[RTTI.Member(5, 0x534, "General")] public float WetnessDryingTime;
	[RTTI.Member(6, 0x538, "General")] public float WetnessSaturationTime;
}

}
