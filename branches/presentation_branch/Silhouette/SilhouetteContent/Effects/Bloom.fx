float BlurDistance = 0.2f;
float oldSceneAverageBrightness = 0.3f;

sampler2D frameSampler: register(s0){
};


float4 PS_blur(float2 Tex: TEXCOORD0) : COLOR
{
	float4 Color;

	//Das hier kann verwendet werden, um nicht nur den Bloompass zu returnen, sondern ihn als gesamtbild,
	//mit dem ursprünglichen Buffer zu verrechnen, damit man nicht mit BlendState.Additive im Spritebatch malen muss
	//float4 regularColor = tex2D( frameSampler, float2(Tex.x, Tex.y));
 
// Get the texel from frameSampler using a modified texture coordinate. This
// gets the texels at the neighbour texels and adds it to Color.
// For less calculations: Delete the last 8 instructions in this block and divide with 8 insteat of 16

    Color  = tex2D( frameSampler, float2(Tex.x+BlurDistance, Tex.y+BlurDistance));
    Color += tex2D( frameSampler, float2(Tex.x-BlurDistance, Tex.y-BlurDistance));
    Color += tex2D( frameSampler, float2(Tex.x+BlurDistance, Tex.y-BlurDistance));
	Color += tex2D( frameSampler, float2(Tex.x-BlurDistance, Tex.y+BlurDistance));

	Color += tex2D( frameSampler, float2(Tex.x+BlurDistance, Tex.y));
	Color += tex2D( frameSampler, float2(Tex.x-BlurDistance, Tex.y));
	Color += tex2D( frameSampler, float2(Tex.x, Tex.y+BlurDistance));
	Color += tex2D( frameSampler, float2(Tex.x, Tex.y-BlurDistance));

	Color += tex2D( frameSampler, float2(Tex.x+BlurDistance*1.25f, Tex.y));
	Color += tex2D( frameSampler, float2(Tex.x-BlurDistance*1.25f, Tex.y));
	Color += tex2D( frameSampler, float2(Tex.x, Tex.y+BlurDistance*1.25f));
	Color += tex2D( frameSampler, float2(Tex.x, Tex.y-BlurDistance*1.25f));

	Color += tex2D( frameSampler, float2(Tex.x+BlurDistance*1.25f, Tex.y+BlurDistance));
	Color += tex2D( frameSampler, float2(Tex.x-BlurDistance*1.25f, Tex.y-BlurDistance));
	Color += tex2D( frameSampler, float2(Tex.x+BlurDistance, Tex.y+BlurDistance*1.25f));
	Color += tex2D( frameSampler, float2(Tex.x-BlurDistance, Tex.y-BlurDistance*1.25f));

// We need to devide the color with the amount of times we added
// a color to it, in this case 4, to get the avg. color

Color = Color / 16;

//Kontrast erhöhen
Color.rgb=pow(Color.rgb,2.0f)*oldSceneAverageBrightness;

//Siehe Kommentar oben
//Color = Color + regularColor;
//return Color/2;

return Color/2;
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
			PixelShader = compile ps_3_0 PS_blur();
	   }
}