sampler firstSampler : register(s0);

float targetRed;
float targetGreen;
float targetBlue;

float4 PS_CC(float2 texCoord: TEXCOORD0): COLOR
{
	float4 color = tex2D(firstSampler, texCoord);
	float4 targetColor = float4(targetRed, targetGreen, targetBlue, 1);
		if(targetRed == 0 && targetGreen == 0 && targetBlue == 0)
		{
			return color;
		}
		else if(color.a != 0 && color.r == 0)
		{
			return targetColor;
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
		PixelShader = compile ps_2_0 PS_CC();
	}
}