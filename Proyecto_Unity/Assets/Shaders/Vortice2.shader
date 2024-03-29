Shader "Planet/Vortice"
{
	Properties 
	{
_RimColor("_RimColor", Color) = (0,0.4965034,1,1)
_RimPower("_RimPower", Range(0.1,3) ) = 1.707772
_efecto("_efecto", 2D) = "black" {}
_velocidad("_velocidad", Float) = 0
_luminosidad("_luminosidad", Float) = 0

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


float4 _RimColor;
float _RimPower;
sampler2D _efecto;
float _velocidad;
float _luminosidad;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_efecto;
float3 viewDir;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Multiply6=_velocidad.xxxx * _Time;
float4 UV_Pan1=float4((IN.uv_efecto.xyxy).x + Multiply6.x,(IN.uv_efecto.xyxy).y + Multiply6.x,(IN.uv_efecto.xyxy).z,(IN.uv_efecto.xyxy).w);
float4 Tex2D0=tex2D(_efecto,UV_Pan1.xy);
float4 Multiply1=_velocidad.xxxx * _Time;
float4 UV_Pan0=float4(Tex2D0.x + Multiply1.y,Tex2D0.y + Multiply1.y,Tex2D0.z,Tex2D0.w);
float4 Tex2D1=tex2D(_efecto,UV_Pan0.xy);
float4 Abs0=abs(_SinTime);
float4 Splat0=Abs0.x;
float4 Add0=float4( 1.0, 1.0, 1.0, 1.0 ) + Splat0;
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
float4 Pow0=pow(Fresnel0,_RimPower.xxxx);
float4 Multiply0=_RimColor * Pow0;
float4 Multiply2=Add0 * Multiply0;
float4 Multiply4=Tex2D1 * Multiply2;
float4 Multiply5=Multiply4 * _luminosidad.xxxx;
float4 Master0_0_NoInput = float4(0,0,0,0);
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Emission = Multiply5;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}