// The blur amount( how far away from our texel will we look up neighbour texels? )
float BlurDistance = 0.002f;
int Samples = 4;
 
// This will use the texture bound to the object( like from the sprite batch ).
sampler ColorMapSampler : register(s0);
 
float4 PS(float2 Tex: TEXCOORD0) : COLOR
{
float4 Color = tex2D( ColorMapSampler, float2(Tex.x, Tex.y));

[unroll(100)]
for (int i = 1; i <= 30; i++)
{
	float bd = BlurDistance / i;

	Color += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y+BlurDistance));
    Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y-BlurDistance));
    Color += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance, Tex.y-BlurDistance));
	Color += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance, Tex.y+BlurDistance));
}
    

// We need to devide the color with the amount of times we added
// a color to it, in this case 4, to get the avg. color
Color = Color / (30+1);
 
// returned the blurred color
return Color;
}

float4x4 MatrixTransform : register(vs, c0);

void SpriteVertexShader(inout float4 color    : COLOR0, 
                        inout float2 texCoord : TEXCOORD0, 
                        inout float4 position : SV_Position) 
{
	{
		position = mul(position, MatrixTransform); 
	} 
}
 
technique PostProcess
{
       pass P0
       {
             VertexShader = compile vs_3_0 SpriteVertexShader();
             PixelShader = compile ps_3_0 PS();
       }
}