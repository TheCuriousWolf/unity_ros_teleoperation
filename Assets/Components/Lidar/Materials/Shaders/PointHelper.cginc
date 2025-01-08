// Helper functions for point cloud shaders

// Unpacks a 32-bit RGBA color into a float4
float4 UnpackRGBA(int rgba)
{
    int4 unpackedColor;
    int unpacked = rgba;

    unpackedColor.r = unpacked >> 16 & 0xFF;
    unpackedColor.g = unpacked >> 8 & 0xFF;
    unpackedColor.b = unpacked & 0xFF;
    unpackedColor.a = 255;

    return unpackedColor / 255.0f;
}