// The blur amount( how far away from our texel will we look up neighbour texels? )
// float BlurDistance = 0.002f;
 
// This will use the texture bound to the object( like from the sprite batch ).
sampler ColorMapSampler : register(s0);
sampler MaskSampler : register(s1);
//sampler noiseSampler: register(s3);

bool bBlur;
bool bVignette;
 
float4 PS(float2 Tex: TEXCOORD0) : COLOR
{

	//float4 noise = tex2D(noiseSampler, Tex);

	float4 Color;
	float4 Vignette = tex2D(MaskSampler, Tex);
	// Vignette ist schwarz-weiﬂ. Abh. vom blau-Wert wird Blurst‰rke ausgew‰hlt
	float BlurDistance = 0.005 * (1-Vignette.b);

if (bVignette)
{

 
	// Get the texel from ColorMapSampler using a modified texture coordinate. This
	// gets the texels at the neighbour texels and adds it to Color.

	if (bBlur)
	{
       Color  = tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y+BlurDistance));
       Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y-BlurDistance));
       Color += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y-BlurDistance));
		Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y+BlurDistance));
		Color = Color / 4;
	}

Color *= Vignette;
//Color += float4(noise.r,noise.r,noise.r,noise.r)/50 * Vignette;

}

return Color;
}
 
technique PostProcess
{
       pass P0
       {
             // A post process shader only needs a pixel shader.
             PixelShader = compile ps_2_0 PS();
       }
}