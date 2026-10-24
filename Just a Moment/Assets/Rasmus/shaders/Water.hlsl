#ifndef WATER_FUNCTIONS_INCLUDED
#define WATER_FUNCTIONS_INCLUDED

#define PIX2 6.2831853 // pi * 2

inline void WaveOffSet(float4 w, float3 p, float t, float noiseAmount, float pNoise,
                inout float3 offset, inout float3 tangent, inout float3 binormal)
{
    float wavelength = max(w.w, 0.001);
    float k = PIX2 / wavelength;
    float c = sqrt(9.8 / k);
    float2 d = normalize(w.xy + 1e-5);
    float f = k * (dot(d, p.xz) - c * t);

    float amp = lerp(1.0, pNoise, noiseAmount);
    float a = (w.z / k) * amp;
    float s = w.z * amp;

    float sinF = sin(f), cosF = cos(f);

    offset += float3(d.x * a * cosF, a * sinF, d.y * a * cosF);
    tangent += float3(-d.x * d.x * s * sinF, d.x * s * cosF, -d.x * d.y * s * sinF);
    binormal += float3(-d.x * d.y * s * sinF, d.y * s * cosF, -d.y * d.y * s * sinF);
}



uint PFNV1(uint key)
{
    const uint prime = 16777619u;
    const uint offset = 2166136261u;
    uint h = offset;
    h *= prime;
    h ^= key;
    h *= prime;
    h ^= key;
    return h;
    
}
float WhiteHash21(float2 p)
{
    int2 c = (int2) round(p);
    uint h = PFNV1((uint) c.x);
    h = PFNV1(h + (uint) c.y);
    return (h >> 8) * (1.0 / 16777215.0);
}

inline void RasHash_Tchou_2_1(uint2 v, out uint o)
{
    // ~6 alu (2 mul)
    v.y ^= 1103515245U;
    v.x += v.y;
    v.x *= v.y;
    v.x ^= v.x >> 5u;
    v.x *= 0x27d4eb2du;
    o = v.x;
}
inline void Hash_Tchou_2_1(float2 p, out float o)
{
    uint r;
    uint2 v = (uint2) (int2) round(p);
    RasHash_Tchou_2_1(v, r);
    o = (r >> 8) * (1.0 / float(0x00ffffff));
}


inline float Noise2D(float2 p)
{
    float2 i = floor(p);
    float2 f = frac(p);

                //float a = Hash_Tchou_2_1_float(i);
                //corners
    float a = 0.0;
    float b = 0.0;
    float c = 0.0;
    float d = 0.0;
    Hash_Tchou_2_1(i, a);
    Hash_Tchou_2_1(i + float2(1.0, 0.0), b);
    Hash_Tchou_2_1(i + float2(0.0, 1.0), c);
    Hash_Tchou_2_1(i + float2(1.0, 1.0), d);
    
    float2 u = f * f * (3.0 - 2.0 * f);

    return lerp(a, b, u.x) +
                       (c - a) * u.y * (1.0 - u.x) +
                       (d - b) * u.x * u.y;
}

inline float Fbm2D(float2 p, int octives)
{
    float value = 0.0;
    float amplitude = 0.5;
    float frequency = 1.0;
    
    for (int i = 0; i < octives; i++)
    {
        value += amplitude * Noise2D(p * frequency);
        p *= 2.0;
        frequency *= 0.2f;
        amplitude *= 0.5f;
    }
    return 0;
}

float WHash21(float2 p)
{
    p = frac(p * float2(123.34, 456.21));
    p += dot(p, p + 45.32);
    return frac(p.x * p.y);
}

float WNoise(float2 p)
{
    float2 i = floor(p), f = frac(p);
    float a = WHash21(i), b = WHash21(i + float2(1, 0));
    float c = WHash21(i + float2(0, 1)), d = WHash21(i + float2(1, 1));
    float2 u = f * f * (3.0 - 2.0 * f);
    return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

float WFbm(float2 p, int octaves)
{
    float v = 0.0, amp = 0.5, freq = 1.0;
    for (int i = 0; i < octaves; i++)
    {
        v += amp * WNoise(p * freq);
        freq *= 2.0;
        amp *= 0.5;
    }
    return v;
}

inline void TchouNoise_float(float2 p, out float n)
{
    n = Noise2D(p);
}
inline void Whash21_float(float2 p, out float n)
{
    n = WFbm(p, 2);
}

inline void FmNoise_float(float2 p, out float n)
{
    n = WhiteHash21(p);

}

inline void Foam_float(float noiseFoamBase,float depthFade, float foamDepth, float foamSharppness, float noiseAmmount, float foamAmmount, out float foam)
{
    float foam_blur = 1.0 - foamSharppness + 1e-6;
    float shore_fade = saturate(depthFade / foamDepth);
    float hard_foam_end = 0.1;
    float soft_foam_end = hard_foam_end + foam_blur * 0.3;
    float foam_shore = smoothstep(0.5 - foam_blur * 0.5, 0.5 + foam_blur * 0.5, noiseFoamBase);
    foam_shore = saturate(smoothstep(soft_foam_end, hard_foam_end, shore_fade) +
                     smoothstep(1, soft_foam_end, shore_fade) * foam_shore * noiseAmmount);
    float foam_surface = smoothstep(noiseFoamBase, noiseFoamBase + foam_blur, foamAmmount);
    foam_surface = smoothstep(0.5 - foam_blur * 0.5, 0.5 + foam_blur * 0.5, foam_surface);
    
    foam = saturate(shore_fade + foam_surface);
}

inline void GerstnerWave_float(float3 p,float waveOctives, float4 A, float4 B, float4 C, float4 D, float t, float speed,float noiseAmmplitude ,
out float3 offset, out float height, out float3 normals)
{
    int oct = max(1, (int) round(waveOctives));
    float noise = WFbm(p.xz * 0.1, oct);
    float time = t * speed;
    float3 tanget = float3(1, 0, 0);
    float3 binormal = float3(0, 0, 1);
    
    offset = 0;
    WaveOffSet(A, p, time, noiseAmmplitude, noise, offset, tanget, binormal);
    WaveOffSet(B, p, time, noiseAmmplitude, noise, offset, tanget, binormal);
   // WaveOffSet(C, p, time, noiseAmmplitude, noise, offset, tanget, binormal);
   // WaveOffSet(D, p, time, noiseAmmplitude, noise, offset, tanget, binormal);
    
    height = offset.y;
    normals = normalize(cross(binormal, tanget));
}

#endif