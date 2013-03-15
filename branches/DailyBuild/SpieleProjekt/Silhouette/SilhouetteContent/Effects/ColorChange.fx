sampler firstSampler : register(s0);

bool bla;
float targetRed;
float targetGreen;
float targetBlue;
float alpha;

float4 PS_CC(float2 texCoord: TEXCOORD0): COLOR
{
	float4 color = tex2D(firstSampler, texCoord);
	// Hier kann ich targetColor von color abhängig machen!!
	float4 targetColor = float4(targetRed* color.r, targetGreen*color.g, targetBlue*color.b, alpha);

		color.rgb = color.rgb + targetColor.rgb;

	return color;
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
		PixelShader = compile ps_3_0 PS_CC();
	}
}