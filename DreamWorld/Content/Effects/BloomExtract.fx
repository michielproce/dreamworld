
sampler TextureSampler : register(s0);

float BloomThreshold;


float4 PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(TextureSampler, texCoord);

    return saturate((c - BloomThreshold) / (1 - BloomThreshold));
}


technique BloomExtract
{
    pass P0
    {
        PixelShader = compile ps_2_0 PixelShader();
    }
}
