float BlurDistance = 0.002f;
float oldSceneAverageBrightness = 0.3f;

sampler2D ColorMapSampler: register(s0){
};


float4 blurgauss(float2 Tex: TEXCOORD0) : COLOR
{

	float4 Color;
	float4 test = tex2D( ColorMapSampler, float2(Tex.x, Tex.y));
	if (test.r*test.g*test.b < 0.65)
	{
		return test;
	}

	{
		float bd = BlurDistance*0.5;
		
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
			PixelShader = compile ps_3_0 blurgauss();
	   }
}