/*
 * ==========
 * PARAMETERS
 * ==========
 */
#define MaxShadows 2 // Same in Terrain.cs
#define ShadowIntensity .5 // 0.0-1.0

float4x4 world;
float4x4 view;
float4x4 projection;

float3 Ambient = .3;
float3 Sun = float3(.5, -.5, 0);

int NumberOfShadows = 0;
float2 ShadowPositions[MaxShadows];
float ShadowRadii[MaxShadows];

texture Texture1;
float WeightTexture1;
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
float WeightTexture2;
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
	float2 hPos : TEXCOORD2;
};

VS_OUTPUT TerrainVertexShader(VS_INPUT input)
{
	VS_OUTPUT output;	
			
	output.Position = mul(mul(mul(input.Position, world), view), projection);
	
	float3 Normal = normalize(mul(normalize(input.Normal), world));
	
	output.SunFactor = saturate(dot(Normal, -Sun));
	
	output.TexCoords = input.TexCoords;

	output.hPos = input.Position.xz;
	
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
	 float2 hPos : TEXCOORD2;
};

float4 TerrainPixelShader(PS_INPUT input) : COLOR0
{
	float4 color;
	color = tex2D(Sampler1, input.TexCoords) * WeightTexture1;
	color += tex2D(Sampler2, input.TexCoords)  * WeightTexture2;
			
    color.rgb *= saturate(input.SunFactor + Ambient);
	
	for(int i=0; i < NumberOfShadows; i++)
	{
		float2 pos = ShadowPositions[i];
		float rad = ShadowRadii[i];
		
		if(abs(input.hPos.x - pos.x) < rad && 
			abs(input.hPos.y - pos.y) < rad)
		{
			float2 dist = input.hPos - pos;
			float lenSq = dist.x * dist.x + dist.y * dist.y;			
			float radSq = rad * rad;
			if(lenSq < radSq)
			{
				float weight = lenSq / radSq + (1 - ShadowIntensity);
				if(weight < 1)
					color *= weight;
			}
		}
		

	}

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