sampler firstSampler : register(s0);
sampler maskSampler : register(s1);

float4 PS_BLEND(float2 texCoord: TEXCOORD0): COLOR
{
	float4 color = tex2D(firstSampler, texCoord);
   
   // Farbe der ersten Textur mit der der 2. multiplizieren
   color.rgb *= tex2D(maskSampler, texCoord);
   
   return color;
}

technique BlendShader
{
	pass pass0
	{
		PixelShader = compile ps_2_0 PS_BLEND();
	}
}