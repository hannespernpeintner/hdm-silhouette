
float2 camMovement;
sampler ColorMapSampler : register(s0);
sampler MaskSampler : register(s1);
 
float4 PS(float2 Tex: TEXCOORD0) : COLOR
{
	float4 Color;
	float4 Vignette;
	Vignette = tex2D(MaskSampler, Tex);
	float2 BlurDistance = (clamp(camMovement.y,-1,1), clamp(camMovement.x,-1,1))/40;
	//BlurDistance -= 1-Vignette.rg/2;
	BlurDistance.y = 0;
	if (Color.r > 0.9) {

		float4 sample1 = tex2D( ColorMapSampler, Tex);
		float4 sample2 = tex2D( ColorMapSampler, Tex + BlurDistance);
       Color  = tex2D( ColorMapSampler, Tex) * 5;
	   	BlurDistance *=0.1f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance) * 3;
	   	BlurDistance *=1.1f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance) * 2;
	   	BlurDistance *=1.1f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance);
	   	BlurDistance *=1.1f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance);
	   	BlurDistance *=1.1f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance);
	   	BlurDistance *=1.1f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance);
		BlurDistance *=1.05f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance);
		BlurDistance *=1.05f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance);
		BlurDistance *=1.05f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance);
		BlurDistance *=1.02f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance);
		BlurDistance *=1.02f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance);
		BlurDistance *=1.02f;
       Color  += tex2D( ColorMapSampler, Tex + BlurDistance);
	}
		Color.rgba = Color.rgba / 20;

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