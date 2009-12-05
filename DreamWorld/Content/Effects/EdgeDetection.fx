float EdgeWidth = 0.4;
float EdgeIntensity = 1;

float NormalThreshold = 0.1;
float DepthThreshold = 0.02;

float NormalSensitivity = 1;
float DepthSensitivity = 10;

float2 ScreenResolution;


texture SceneTexture;

sampler SceneSampler : register(s0) = sampler_state
{
    Texture = (SceneTexture);
    
    MinFilter = Linear;
    MagFilter = Linear;
    
    AddressU = Clamp;
    AddressV = Clamp;
};


texture NormalDepthTexture;

sampler NormalDepthSampler : register(s1) = sampler_state
{
    Texture = (NormalDepthTexture);
    
    MinFilter = Linear;
    MagFilter = Linear;
    
    AddressU = Clamp;
    AddressV = Clamp;
};


float4 PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
    float3 scene = tex2D(SceneSampler, texCoord);

    float2 edgeOffset = EdgeWidth / ScreenResolution;
    
    float4 n1 = tex2D(NormalDepthSampler, texCoord + float2(-1, -1) * edgeOffset);
    float4 n2 = tex2D(NormalDepthSampler, texCoord + float2( 1,  1) * edgeOffset);
    float4 n3 = tex2D(NormalDepthSampler, texCoord + float2(-1,  1) * edgeOffset);
    float4 n4 = tex2D(NormalDepthSampler, texCoord + float2( 1, -1) * edgeOffset);

    float4 diagonalDelta = abs(n1 - n2) + abs(n3 - n4);

    float normalDelta = dot(diagonalDelta.xyz, 1);
    float depthDelta = diagonalDelta.w;
    
    normalDelta = saturate((normalDelta - NormalThreshold) * NormalSensitivity);
    depthDelta = saturate((depthDelta - DepthThreshold) * DepthSensitivity);

    float edgeAmount = saturate(normalDelta + depthDelta) * EdgeIntensity;
    
    scene *= (1 - edgeAmount);

    return float4(scene, 1);
}


// Compile the pixel shader for doing edge detection without any sketch effect.
technique EdgeDetect
{
    pass P0
    {
        PixelShader = compile ps_2_0 PixelShader();
    }
}