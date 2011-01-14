sampler firstSampler : register(s0);

bool bla;
float targetRed;
float targetGreen;
float targetBlue;

float4 PS_CC(float2 texCoord: TEXCOORD0): COLOR
{
	float4 color = tex2D(firstSampler, texCoord);
	float4 targetColor = float4(targetRed, targetGreen, targetBlue, 1);

	if (bla)
	{
		color *= targetColor;
	}

	return color;
}

technique BlendShader
{
	pass pass0
	{
		PixelShader = compile ps_2_0 PS_CC();
	}
}