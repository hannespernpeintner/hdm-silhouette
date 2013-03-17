sampler firstSampler : register(s0);

float BleachAdditionAmount = 0.5f;


float4 PS_BLEACH(float2 texCoord: TEXCOORD0): COLOR
{
	float4 color = tex2D(firstSampler, texCoord);
	float4 fAmount = float4(BleachAdditionAmount, BleachAdditionAmount, BleachAdditionAmount, 0);

	{
		color.rgb += (fAmount).rgb;
		return color;
	}
}
float4x4 MatrixTransform : register(vs, c0);

void SpriteVertexShader(inout float4 color    : COLOR0, 
                        inout float2 texCoord : TEXCOORD0, 
                        inout float4 position : SV_Position) 
{
	{
		position = mul(position, MatrixTransform); 
	} 
}
technique BlendShader
{
	pass pass0
	{
		VertexShader = compile vs_3_0 SpriteVertexShader();
		PixelShader = compile ps_3_0 PS_BLEACH();
	}
}