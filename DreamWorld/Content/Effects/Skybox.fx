/*
 * ==========
 * PARAMETERS
 * ==========
 */
float4x4 world;
float4x4 view;
float4x4 projection;

float3 Ambient = .3;

texture Texture;
sampler Sampler = sampler_state
{
	Texture = (Texture);
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

/*
 * ==============
 * VERTEX SHADERS 
 * ==============
 */
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

VS_OUTPUT SkyboxVertexShader(VS_INPUT input)
{
	VS_OUTPUT output;	
			
	output.Position = mul(mul(mul(input.Position, world), view), projection);	
	
	output.TexCoords = input.TexCoords;
	
	return output;
}


/*
 * =============
 * PIXEL SHADERS 
 * =============
 */
 
struct PS_INPUT
{
     float2 TexCoords : TEXCOORD0;
};

float4 SkyboxPixelShader(PS_INPUT input) : COLOR0
{
	float4 color;

	color = tex2D(Sampler, input.TexCoords);
			
    //color.rgb *= saturate(Ambient);

    return color;
}

float4 IgnoreNormalDepthPixelShader(float4 color : COLOR0) : COLOR0
{
    return 1;
}


/*
 * ==========
 * TECHNIQUES
 * ==========
 */
technique Skybox
{
    pass P0
    {
        VertexShader = compile vs_2_0 SkyboxVertexShader();
        PixelShader = compile ps_2_0 SkyboxPixelShader();
    }
}

technique IgnoreNormalDepth
{
	pass P0
	{
		VertexShader = compile vs_2_0 SkyboxVertexShader();
        PixelShader = compile ps_2_0 IgnoreNormalDepthPixelShader();
	}
}