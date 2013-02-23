// The blur amount( how far away from our texel will we look up neighbour texels? )
float BlurDistance = 0.002f;
 
// This will use the texture bound to the object( like from the sprite batch ).
sampler ColorMapSampler : register(s0);

int gauss[5][5] = 
{
	{1,4,6,4,1},
	{4,16,24,16,4},
	{6,24,36,24,6},
	{4,16,24,16,4},
	{1,4,6,4,1}
};
 
float4 PS(float2 Tex: TEXCOORD0) : COLOR
{

	float4 Color;
	{
		float bd = BlurDistance*2;
		
		Color = tex2D( ColorMapSampler, float2(Tex.x-bd, Tex.y-bd));
		Color += 2*tex2D( ColorMapSampler, float2(Tex.x-bd, Tex.y));
		Color += tex2D( ColorMapSampler, float2(Tex.x-bd, Tex.y+bd));

		Color += 2*tex2D( ColorMapSampler, float2(Tex.x, Tex.y-bd));
		Color += 4*tex2D( ColorMapSampler, float2(Tex.x, Tex.y));
		Color += 2*tex2D( ColorMapSampler, float2(Tex.x, Tex.y+bd));

		Color += tex2D( ColorMapSampler, float2(Tex.x+bd, Tex.y-bd));
		Color += 2*tex2D( ColorMapSampler, float2(Tex.x+bd, Tex.y));
		Color += tex2D( ColorMapSampler, float2(Tex.x+bd, Tex.y+bd));
	}
    

	// We need to devide the color with the amount of times we added
	// a color to it, in this case 4, to get the avg. color
	Color = Color / (16);
 
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