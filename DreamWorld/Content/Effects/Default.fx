#define MaxBones 59

float4x4 world;
float4x4 view;
float4x4 projection;

texture Texture;

bool Skinned;
float4x4 Bones[MaxBones];



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
	float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};


struct VS_OUTPUT
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;

};

VS_OUTPUT DefaultVertexShader(VS_INPUT input)
{
	VS_OUTPUT output;
			
	float4 position = input.Position;
	
	if(Skinned) {
		float4x4 skinTransform = 0;    
		skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x;
		skinTransform += Bones[input.BoneIndices.y] * input.BoneWeights.y;
		skinTransform += Bones[input.BoneIndices.z] * input.BoneWeights.z;
		skinTransform += Bones[input.BoneIndices.w] * input.BoneWeights.w;
		position = mul(position, skinTransform);
	}
	
	output.Position = mul(mul(mul(position, world), view), projection);	
	
	output.TexCoords = input.TexCoords;
	
	return output;
}


struct PS_INPUT
{
     float2 TexCoords : TEXCOORD0;
};

float4 DefaultPixelShader(PS_INPUT input) : COLOR0
{
    float4 color = tex2D(Sampler, input.TexCoords);
    
    return color;
}


technique Default
{
    pass P0
    {
        VertexShader = compile vs_2_0 DefaultVertexShader();
        PixelShader = compile ps_2_0 DefaultPixelShader();
    }
}