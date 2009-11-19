/*
 * ==========
 * PARAMETERS
 * ==========
 */
#define MaxBones 59

float4x4 world;
float4x4 view;
float4x4 projection;

float Ambient = .3;
bool IgnoreSun;
float3 Sun = float3(.5, -.5, 0);


bool Skinned;
float4x4 Bones[MaxBones];

texture Texture;
sampler Sampler = sampler_state
{
	Texture = (Texture);

	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};



/*
 * =========
 * FUNCTIONS 
 * =========
 */
float4 TransformPosition(float4 position, float4 indices, float4 weights ) 
{
	if(Skinned)
	{		
		float4x4 skinTransform = 0;    
		
		skinTransform += Bones[indices.x] * weights.x;
		skinTransform += Bones[indices.y] * weights.y;
		skinTransform += Bones[indices.z] * weights.z;
		skinTransform += Bones[indices.w] * weights.w;
		
		position = mul(position, skinTransform);
	}
	
	return mul(mul(mul(position, world), view), projection);
}
 


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
	float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};


struct VS_OUTPUT
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;
	float SunFactor : TEXCOORD1;
};

VS_OUTPUT DefaultVertexShader(VS_INPUT input)
{
	VS_OUTPUT output;
			
	output.Position = TransformPosition(input.Position, input.BoneIndices, input.BoneWeights); 
	
	float3 Normal = normalize(mul(normalize(input.Normal), world));
	
	if(IgnoreSun)
		output.SunFactor = 1;
	else
		output.SunFactor = saturate(dot(Normal, -Sun));
	
	output.TexCoords = input.TexCoords;
	
	return output;
}


struct NormalDepthVertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

NormalDepthVertexShaderOutput NormalDepthVertexShader(VS_INPUT input, uniform bool ignore)
{
    NormalDepthVertexShaderOutput output;
    
    output.Position = TransformPosition(input.Position, input.BoneIndices, input.BoneWeights); 	
	
	if(ignore)
	{
		output.Color = 0;
	}
	else 
	{
		float3 worldNormal = mul(input.Normal, world);
		output.Color.rgb = (worldNormal + 1) / 2;	
		output.Color.a = output.Position.z / output.Position.w;
	}	
	
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

};

float4 DefaultPixelShader(PS_INPUT input) : COLOR0
{
    float4 color = tex2D(Sampler, input.TexCoords);
    color.rgb *= saturate(input.SunFactor + Ambient);

    return color;
}


float4 NormalDepthPixelShader(float4 color : COLOR0) : COLOR0
{
    return color;
}




/*
 * ==========
 * TECHNIQUES
 * ==========
 */
technique Default
{
    pass P0
    {
        VertexShader = compile vs_2_0 DefaultVertexShader();
        PixelShader = compile ps_2_0 DefaultPixelShader();
    }
}

technique NormalDepth
{
	pass P0
	{
		VertexShader = compile vs_2_0 NormalDepthVertexShader(false);
        PixelShader = compile ps_2_0 NormalDepthPixelShader();
	}
}

technique IgnoreNormalDepth
{
	pass P0
	{
		VertexShader = compile vs_2_0 NormalDepthVertexShader(true);
        PixelShader = compile ps_2_0 NormalDepthPixelShader();
	}
}