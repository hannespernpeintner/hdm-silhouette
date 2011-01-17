sampler firstSampler : register(s0);

// amount ist fürs bleachen, targetXXX zielfarbe, für den colorchange
float amount = 0.5f;
float targetRed;
float targetGreen;
float targetBlue;

float4 PS_BLEACH(float2 texCoord: TEXCOORD0): COLOR
{
	float4 color = tex2D(firstSampler, texCoord);
	float4 targetColor = float4(targetRed* color.r, targetGreen*color.g, targetBlue*color.b, 0);
	float4 fAmount = float4(amount, amount, amount, 0);

	{
			color += (fAmount/2);
			color = color + targetColor;

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