float Wavelength = 0.01f;
float Time = 0;

sampler2D frameSampler: register(s0){
};


float distance1(float x1, float y1, float x2, float y2)
{
	return sqrt(pow((x1-x2),2) + pow((y1-y2),2));
}

float4 PS(float2 Tex: TEXCOORD0) : COLOR
{
	float4 Color = tex2D(frameSampler, float2( Tex.x+(cos(Tex.x*50))*(0.13)*(Time), Tex.y+(sin(Tex.y*20))*(0.13)*(Time)));
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