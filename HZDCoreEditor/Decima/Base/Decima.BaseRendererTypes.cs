namespace Decima
{
    /// <summary>
    /// Assumes that the TypeId is 0xDD1CF5F847F285DC for DS/HZD.
    /// </summary>
    public enum BaseDataBufferFormat : int
    {
        Invalid = 0,
        R_FLOAT_16 = 1,
        R_FLOAT_32 = 2,
        RG_FLOAT_32 = 3,
        RGB_FLOAT_32 = 4,
        RGBA_FLOAT_32 = 5,
        R_UINT_8 = 6,
        R_UINT_16 = 7,
        R_UINT_32 = 8,
        RG_UINT_32 = 9,
        RGB_UINT_32 = 10,
        RGBA_UINT_32 = 11,
        R_INT_32 = 12,
        RG_INT_32 = 13,
        RGB_INT_32 = 14,
        RGBA_INT_32 = 15,
        R_UNORM_8 = 16,
        R_UNORM_16 = 17,
        RGBA_UNORM_8 = 18,
        RGBA_UINT_8 = 19,
        RG_UINT_16 = 20,
        RGBA_UINT_16 = 21,
        RGBA_INT_8 = 22,
        Structured = 23,
    }

    /// <summary>
    /// Assumes that the TypeId is 0x76B127086C01CEF9 for DS/HZD.
    /// </summary>
    public enum BaseIndexFormat : int
    {
        Index16 = 0,
        Index32 = 1,
    }

    /// <summary>
    /// Assumes that the TypeId is 0x82B0917D65F57B50 for DS and 0x5AE55465BEED6FAE for HZD.
    /// </summary>
    public enum BaseVertexElementStorageType : byte
    {
        Undefined = 0,
        SignedShortNormalized = 1,
        Float = 2,
        _HalfFloat = 3,
        UnsignedByteNormalized = 4,
        SignedShort = 5,
        X10Y10Z10W2Normalized = 6,
        UnsignedByte = 7,
        UnsignedShort = 8,
        UnsignedShortNormalized = 9,
        UNorm8sRGB = 10,
        X10Y10Z10W2UNorm = 11,
    }

    /// <summary>
    /// Assumes that the TypeId is 0xDF41686E0FC76D2B for DS and 0xF3AF7D47725EF5B4 for HZD. Left empty because the enum values diverge.
    /// </summary>
    public enum BaseVertexElement : byte
    {
    }

    /// <summary>
    /// Assumes that the TypeId is 0xCDC39E10FC548E63 for DS/HZD.
    /// </summary>
    public enum BaseTextureType : int
    {
        _2D = 0,
        _3D = 1,
        CubeMap = 2,
        _2DArray = 3,
    }

    /// <summary>
    /// Assumes that the TypeId is 0x211FDC8FD3395464 for DS/HZD.
    /// </summary>
    public enum BasePixelFormat : int
    {
        INVALID = 76,
        RGBA_5551 = 0,
        RGBA_5551_REV = 1,
        RGBA_4444 = 2,
        RGBA_4444_REV = 3,
        RGB_888_32 = 4,
        RGB_888_32_REV = 5,
        RGB_888 = 6,
        RGB_888_REV = 7,
        RGB_565 = 8,
        RGB_565_REV = 9,
        RGB_555 = 10,
        RGB_555_REV = 11,
        RGBA_8888 = 12,
        RGBA_8888_REV = 13,
        RGBE_REV = 14,
        RGBA_FLOAT_32 = 15,
        RGB_FLOAT_32 = 16,
        RG_FLOAT_32 = 17,
        R_FLOAT_32 = 18,
        RGBA_FLOAT_16 = 19,
        RGB_FLOAT_16 = 20,
        RG_FLOAT_16 = 21,
        R_FLOAT_16 = 22,
        RGBA_UNORM_32 = 23,
        RG_UNORM_32 = 24,
        R_UNORM_32 = 25,
        RGBA_UNORM_16 = 26,
        RG_UNORM_16 = 27,
        R_UNORM_16 = 28,
        RGBA_UNORM_8 = 29,
        RG_UNORM_8 = 30,
        R_UNORM_8 = 31,
        RGBA_NORM_32 = 32,
        RG_NORM_32 = 33,
        R_NORM_32 = 34,
        RGBA_NORM_16 = 35,
        RG_NORM_16 = 36,
        R_NORM_16 = 37,
        RGBA_NORM_8 = 38,
        RG_NORM_8 = 39,
        R_NORM_8 = 40,
        RGBA_UINT_32 = 41,
        RG_UINT_32 = 42,
        R_UINT_32 = 43,
        RGBA_UINT_16 = 44,
        RG_UINT_16 = 45,
        R_UINT_16 = 46,
        RGBA_UINT_8 = 47,
        RG_UINT_8 = 48,
        R_UINT_8 = 49,
        RGBA_INT_32 = 50,
        RG_INT_32 = 51,
        R_INT_32 = 52,
        RGBA_INT_16 = 53,
        RG_INT_16 = 54,
        R_INT_16 = 55,
        RGBA_INT_8 = 56,
        RG_INT_8 = 57,
        R_INT_8 = 58,
        RGB_FLOAT_11_11_10 = 59,
        RGBA_UNORM_10_10_10_2 = 60,
        RGB_UNORM_11_11_10 = 61,
        DEPTH_FLOAT_32_STENCIL_8 = 62,
        DEPTH_FLOAT_32_STENCIL_0 = 63,
        DEPTH_24_STENCIL_8 = 64,
        DEPTH_16_STENCIL_0 = 65,
        BC1 = 66,
        BC2 = 67,
        BC3 = 68,
        BC4U = 69,
        BC4S = 70,
        BC5U = 71,
        BC5S = 72,
        BC6U = 73,
        BC6S = 74,
        BC7 = 75,
    }

    /// <summary>
    /// Assumes that the TypeId is 0x8E32C40558E36E3E for DS/HZD.
    /// </summary>
    public enum BaseProgramType : int
    {
        ComputeProgram = 0,
        GeometryProgram = 1,
        VertexProgram = 2,
        PixelProgram = 3,
    }

    /// <summary>
    /// Assumes that the TypeId is 0x2D12E73211551897 for DS/HZD.
    /// </summary>
    public enum BaseProgramTypeMask : int
    {
        None = 0,

        CP = 1,
        GP = 2,
        VP = 4,
        FP = 8,
        All = 15,
        AllGraphics = 14,

        VP_GP_FP = 14,
        VP_GP = 6,
        VP_FP = 12,
        FP_CP = 9,
    }

    /// <summary>
    /// Assumes that the TypeId is 0x255B21D736738454 for DS/HZD.
    /// </summary>
    public enum BaseRenderDataStreamingMode : byte
    {
        NotStreaming = 0,
        Streaming = 1,
    };
}