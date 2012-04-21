#define NUM_SAMPLES 180

float3 ScreenLightPos = (0,1,0.5);

float Density = .95f;
float Decay = .98f;
float Weight = 0.2f;
float Exposure = .15f;

sampler2D frameSampler: register(s0){
};
sampler2D clouds: register(s3){
};

float4 lightRayPS( float2 texCoord : TEXCOORD0 ) : COLOR0
{

  // Calculate vector from pixel to light source in screen space.  
   float2 deltaTexCoord = (texCoord - ScreenLightPos.xy);  
  // Divide by number of samples and scale by control factor.  
  deltaTexCoord *= 1.0f / NUM_SAMPLES * Density;  
  // Store initial sample.  
   float3 color = tex2D(frameSampler, texCoord);  
  // Set up illumination decay factor.  
   float illuminationDecay = 1.0f;  
  // Evaluate summation from Equation 3 NUM_SAMPLES iterations.  
   for (int i = 0; i < NUM_SAMPLES; i++)  
  {  
    // Step sample location along ray.  
    texCoord -= deltaTexCoord;  
    // Retrieve sample at new location.  
   float3 sample = tex2D(frameSampler, texCoord);  
    // Apply sample attenuation scale/decay factors.  
    sample *= illuminationDecay * Weight;  
    // Accumulate combined color.  
    color += sample;  
    // Update exponential decay factor.  
    illuminationDecay *= Decay;  
  }  
  // Output final color with a further scale control factor.  
  float4 temp = float4( color * Exposure, 0.1);
  temp.a = 0.6;
  return temp;
   //return float4( color * Exposure, 0.1);  
    
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
			PixelShader = compile ps_3_0 lightRayPS();
       }
}