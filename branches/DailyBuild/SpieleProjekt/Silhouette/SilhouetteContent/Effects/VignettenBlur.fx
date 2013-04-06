// The blur amount( how far away from our texel will we look up neighbour texels? )
// float BlurDistance = 0.002f;
 
// This will use the texture bound to the object( like from the sprite batch ).
sampler ColorMapSampler : register(s0);
sampler MaskSampler : register(s1);
//sampler noiseSampler: register(s3);

bool bBlur;
bool bVignette;

float bN = 0;
float bE = 0;
float bS = 0;
float bW = 0;
 
float4 PS(float2 Tex: TEXCOORD0) : COLOR
{

	//float4 noise = tex2D(noiseSampler, Tex);

	float4 Color;
	float4 Vignette = tex2D(MaskSampler, Tex);
	// Vignette ist schwarz-weiﬂ. Abh. vom blau-Wert wird Blurst‰rke ausgew‰hlt
	float BlurDistance = 0.005 * (1-Vignette.b);
	float mbv = (1-Vignette.b); // MotionBlurVignette

if (bVignette)
{

 
	// Get the texel from ColorMapSampler using a modified texture coordinate. This
	// gets the texels at the neighbour texels and adds it to Color.
	Color = tex2D( ColorMapSampler, float2(Tex.x, Tex.y));
	if (bBlur)
	{
       Color  += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance+bW*mbv, Tex.y-BlurDistance+bS*mbv));
       Color  += tex2D( ColorMapSampler, float2(Tex.x, Tex.y-BlurDistance));
       Color  += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance+bE*mbv, Tex.y-BlurDistance+bS*mbv));
	   
       Color  += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance+bW*mbv, Tex.y));
       Color  += tex2D( ColorMapSampler, float2(Tex.x, Tex.y));
       Color  += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance+bE*mbv, Tex.y));
	   
       Color  += tex2D( ColorMapSampler, float2(Tex.x-BlurDistance+bW*mbv, Tex.y+BlurDistance+bN*mbv));
       Color  += tex2D( ColorMapSampler, float2(Tex.x, Tex.y+BlurDistance+bN*mbv));
       Color  += tex2D( ColorMapSampler, float2(Tex.x+BlurDistance+bE*mbv, Tex.y+BlurDistance+bN*mbv));

	   
       Color  += tex2D( ColorMapSampler, float2(Tex.x, Tex.y+2*BlurDistance+bS*mbv));
       Color  += tex2D( ColorMapSampler, float2(Tex.x, Tex.y-2*BlurDistance+bN*mbv));
       Color  += tex2D( ColorMapSampler, float2(Tex.x+2*BlurDistance+bW*mbv, Tex.y));
       Color  += tex2D( ColorMapSampler, float2(Tex.x-2*BlurDistance+bE*mbv, Tex.y));

		Color = Color / 14;
	}

Color *= Vignette;
//Color += float4(noise.r,noise.r,noise.r,noise.r)/50 * Vignette;

}

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