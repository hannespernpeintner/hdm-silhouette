sampler firstSampler : register(s0);

float amount = 0.5f;
bool bla;
float targetRed;
float targetGreen;
float targetBlue;

float4 PS_BLEACH(float2 texCoord: TEXCOORD0): COLOR
{
	float4 color = tex2D(firstSampler, texCoord);
	float4 targetColor = float4(targetRed, targetGreen, targetBlue, 1);
	float4 fAmount = float4(amount, amount, amount, 0.0f);

	if(bla)
	{
		if (color.a != 0)
		{
			color = fAmount*0.1f+targetColor;
		}
		return color;
	}
	if (!bla)
	{
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
}

technique BlendShader
{
	pass pass0
	{
		PixelShader = compile ps_2_0 PS_BLEACH();
	}
}