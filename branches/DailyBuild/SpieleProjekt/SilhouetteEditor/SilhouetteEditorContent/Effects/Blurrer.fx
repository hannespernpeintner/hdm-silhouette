sampler sampler1;

float4 PS_BLUR(float2 texCoord: TEXCOORD0): Color
{
float4 color = 0;

color += tex2D(sampler1, texCoord + float2(0.006f, 0));
color += tex2D(sampler1, texCoord + float2(0.015f, 0));
color += tex2D(sampler1, texCoord + float2(-0.006f, 0));
color += tex2D(sampler1, texCoord + float2(-0.015f, 0));
color /= 4.0f;

return color;
}

technique Blurrer
{
	pass pass0
	{
		PixelShader = compile ps_2_0 PS_BLUR();
	}
}
