sampler firstSampler : register(s0);

float amount = 0.5f;

float4 PS_BLEACH(float2 texCoord: TEXCOORD0): COLOR
{
	float4 color = tex2D(firstSampler, texCoord);
	float4 fAmount = float4(amount, amount, amount, 0.0f);

		if (color.a != 0)
		{
			color += fAmount;
			return color;
		}
		else
		{
			return color;
		}
}

technique BlendShader
{
	pass pass0
	{
		PixelShader = compile ps_2_0 PS_BLEACH();
	}
}