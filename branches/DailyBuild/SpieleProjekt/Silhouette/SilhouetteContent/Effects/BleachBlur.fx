float BlurDistance = 0.003f;
float amount = 0.6f;
float targetRed;
float targetGreen;
float targetBlue;
 
sampler ColorMapSampler : register(s0);
 
float4 PS(float2 Tex: TEXCOORD0) : COLOR
{
float4 Color;
 
    Color  = tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y+BlurDistance));
    Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y-BlurDistance));
    Color += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y-BlurDistance));
	Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y+BlurDistance));

Color = Color / 4;

	float4 targetColor = float4(targetRed* Color.r, targetGreen*Color.g, targetBlue*Color.b, 0);
	float4 fAmount = float4(amount, amount, amount, 0);

	Color += (fAmount/2);
	Color = Color + targetColor;

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