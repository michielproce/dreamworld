/*
 * ==========
 * PARAMETERS
 * ==========
 */
float4x4 world;
float4x4 view;
float4x4 projection;

float3 Ambient = .3;
float3 Sun = float3(.5, -.5, 0);

float TransitionHeight;
float TransitionSmudge;

texture Texture1;
sampler Sampler1 = sampler_state
{
	Texture = (Texture1);
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = wrap; 
	AddressV = wrap;
};

texture Texture2;
sampler Sampler2 = sampler_state
{
	Texture = (Texture2);
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = wrap; 
	AddressV = wrap;
};

/*
 * ==============
 * VERTEX SHADERS 
 * ==============
 */
struct VS_INPUT
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 TexCoords : TEXCOORD0;
};


struct VS_OUTPUT
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;
	float SunFactor : TEXCOORD1;
	float4 vPos : TEXCOORD2;
};

VS_OUTPUT TerrainVertexShader(VS_INPUT input)
{
	VS_OUTPUT output;	
			
	output.Position = mul(mul(mul(input.Position, world), view), projection);
	
	float3 Normal = normalize(mul(normalize(input.Normal), world));
	
	output.SunFactor = saturate(dot(Normal, -Sun));
	
	output.TexCoords = input.TexCoords;

	output.vPos = input.Position;
	
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
     float SunFactor: TEXCOORD1;
	 float4 vPos : TEXCOORD2;
};

float4 TerrainPixelShader(PS_INPUT input) : COLOR0
{
	float4 color;

	float bottom = TransitionHeight - TransitionSmudge * .5;
	float top = TransitionHeight + TransitionSmudge * .5;

	if(input.vPos.y < bottom) 			
		color = tex2D(Sampler1, input.TexCoords);	
	else if(input.vPos.y > top) 		
		color = tex2D(Sampler2, input.TexCoords);
	else {
		float weight = (input.vPos.y - bottom) / TransitionSmudge;
		color = tex2D(Sampler1, input.TexCoords) * (1-weight);
		color += tex2D(Sampler2, input.TexCoords) * weight; 
	}
			
    color.rgb *= saturate(input.SunFactor + Ambient);

    return color;
}

float4 IgnoreNormalDepthPixelShader() : COLOR0
{
    return 1;
}


/*
 * ==========
 * TECHNIQUES
 * ==========
 */
technique Terrain
{
    pass P0
    {
        VertexShader = compile vs_2_0 TerrainVertexShader();
        PixelShader = compile ps_2_0 TerrainPixelShader();
    }
}

technique IgnoreNormalDepth
{
	pass P0
	{
		VertexShader = compile vs_2_0 TerrainVertexShader();
        PixelShader = compile ps_2_0 IgnoreNormalDepthPixelShader();
	}
}