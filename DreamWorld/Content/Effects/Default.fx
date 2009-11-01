float4x4 world;
float4x4 view;
float4x4 projection;

texture Texture;

sampler Sampler = sampler_state
{
	Texture = (Texture);

	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};


struct VS_INPUT
{
	float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
};


struct VS_OUTPUT
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;

};

VS_OUTPUT VertexShader(VS_INPUT input)
{
	VS_OUTPUT output;
	
	float4x4 wvp = mul(mul(world, view), projection);
	output.Position = mul(input.Position, wvp);

	output.TexCoords = input.TexCoords;
	
	return output;
}


struct PS_INPUT
{
     float2 TexCoords : TEXCOORD0;
};

float4 PixelShader(PS_INPUT input) : COLOR0
{
    float4 color = tex2D(Sampler, input.TexCoords);
    
    return color;
}


technique Default
{
    pass P0
    {
        VertexShader = compile vs_1_0 VertexShader();
        PixelShader = compile ps_1_0 PixelShader();
    }
}