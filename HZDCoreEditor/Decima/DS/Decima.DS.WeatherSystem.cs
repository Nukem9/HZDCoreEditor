namespace Decima.DS
{
    [RTTI.Serializable(0x8E92245843662D81, GameType.DS)]
    public class WeatherSystem : CoreObject
    {
        [RTTI.Member(3, 0x160, "General")] public Ref<RenderEffectResource> SimulationRenderEffectResource;
        [RTTI.Member(4, 0x1E0, "General")] public Ref<WorldDataType> PrecipitationOcclusionHeightData;
        [RTTI.Member(5, 0x380, "General")] public Array<WindSimulationForceField> WindSimulationForceFields;
        [RTTI.Member(6, 0x3D0, "General")] public BoundingBox3 WorldBounds;
        [RTTI.Member(7, 0x3F0, "General")] public FRange TemperatureRange;
        [RTTI.Member(8, 0x534, "General")] public float WetnessDryingTime;
        [RTTI.Member(9, 0x538, "General")] public float WetnessSaturationTime;
    }
}