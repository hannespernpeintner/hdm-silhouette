// The blur amount( how far away from our texel will we look up neighbour texels? )
float BlurDistance = 0.002f;
 
// This will use the texture bound to the object( like from the sprite batch ).
sampler ColorMapSampler : register(s0);
 
float4 PS(float2 Tex: TEXCOORD0) : COLOR
{
float4 Color;
 
// Get the texel from ColorMapSampler using a modified texture coordinate. This
// gets the texels at the neighbour texels and adds it to Color.
// For less calculations: Delete the last 8 instructions in this block and divide with 8 insteat of 16
    Color  = tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y+BlurDistance));
    Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y-BlurDistance));
    Color += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y-BlurDistance));
	Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y+BlurDistance));

	Color += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y));
	Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y));
	Color += tex2D( ColorMapSampler, float2(Tex.x, Tex.y+BlurDistance));
	Color += tex2D( ColorMapSampler, float2(Tex.x, Tex.y-BlurDistance));

	Color += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance*1.25f, Tex.y));
	Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance*1.25f, Tex.y));
	Color += tex2D( ColorMapSampler, float2(Tex.x, Tex.y+BlurDistance*1.25f));
	Color += tex2D( ColorMapSampler, float2(Tex.x, Tex.y-BlurDistance*1.25f));

	Color += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance*1.25f, Tex.y+BlurDistance));
	Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance*1.25f, Tex.y-BlurDistance));
	Color += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y+BlurDistance*1.25f));
	Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y-BlurDistance*1.25f));

// We need to devide the color with the amount of times we added
// a color to it, in this case 4, to get the avg. color
Color = Color / 16;
 
// returned the blurred color
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