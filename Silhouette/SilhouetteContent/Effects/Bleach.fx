sampler firstSampler : register(s0);

// amount ist fürs bleachen, targetXXX zielfarbe, für den colorchange
            float orangeTargetRed = 1.0f;
            float orangeTargetGreen = 0.68f;
            float orangeTargetBlue = 0.55f;
            float blueTargetRed = 0.53f;
            float blueTargetGreen = 0.59f;
            float blueTargetBlue = 0.69f;

			float fadeOrange;
			float fadeBlue;

float amount = 0.5f;


float4 PS_BLEACH(float2 texCoord: TEXCOORD0): COLOR
{
	float4 color = tex2D(firstSampler, texCoord);
	float3 blueTarget = float3(blueTargetRed, blueTargetGreen, blueTargetBlue);
	float3 orangeTarget = float3(orangeTargetRed, orangeTargetGreen, orangeTargetBlue);

	float4 fAmount = float4(amount, amount, amount, 0);

	{
			color += (fAmount);
			color.rgb = (1- 0.5f * fadeBlue - 0.5f * fadeOrange) * color + (fadeBlue * blueTarget)/2 + (fadeOrange * orangeTarget)/2;

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