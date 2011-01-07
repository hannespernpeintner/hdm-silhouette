// The blur amount( how far away from our texel will we look up neighbour texels? )
// float BlurDistance = 0.002f;
 
// This will use the texture bound to the object( like from the sprite batch ).
sampler ColorMapSampler : register(s0);
sampler MaskSampler : register(s1);
sampler MaskSampler2 : register(s2);
 
float4 PS(float2 Tex: TEXCOORD0) : COLOR
{
float4 Color;
float4 Vignette = tex2D(MaskSampler, Tex);
float BlurDistance = 0.005 * (1-Vignette.b);
 
// Get the texel from ColorMapSampler using a modified texture coordinate. This
// gets the texels at the neighbour texels and adds it to Color.
       Color  = tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y+BlurDistance));
       Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y-BlurDistance));
       Color += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y-BlurDistance));
Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y+BlurDistance));
// We need to devide the color with the amount of times we added
// a color to it, in this case 4, to get the avg. color
Color = Color / 4;

 
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