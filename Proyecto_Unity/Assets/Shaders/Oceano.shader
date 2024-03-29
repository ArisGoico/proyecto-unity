Shader "Planet/Oceano"
{
	Properties 
	{
_MainTex("_MainTex", 2D) = "black" {}
_Ilum("textura de luz", 2D) = "black" {}
_ColorProfundo("agua profunda", Color) = (0,0.2666667,0.1647059,1)
_ColorMedio("agua cercana a la superficie", Color) = (0.3411765,0.7215686,0.1764706,1)
_ColorSuperficie("agua superficial", Color) = (0.7568628,0.8078431,0.3294118,1)
_ColorOlas("_ColorOlas", Color) = (1,1,1,1)
_ColorPlaya("playa", Color) = (0.8039216,0.7450981,0.6156863,1)
_Mar("Textura Olas", 2D) = "black" {}
_velVer("velocidad", Float) = 0
_costa("Textura de la costa", 2D) = "black" {}
_Olas("_Olas", Range(0.01,0.05) ) = 0.01
_VelocidadOlas("_VelocidadOlas", Float) = 0
_nivelMar("Nivel del Agua", Float) = 0
_tamPlaya("Tamaño Playa", Float) = 0

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="True"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
Mode Linear
Range 3,6
Color (0.0093562,0.04580892,0.08955222,1)
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 3.0


sampler2D _MainTex;
sampler2D _Ilum;
float4 _ColorProfundo;
float4 _ColorMedio;
float4 _ColorSuperficie;
float4 _ColorOlas;
float4 _ColorPlaya;
sampler2D _Mar;
float _velVer;
sampler2D _costa;
float _Olas;
float _VelocidadOlas;
float _nivelMar;
float _tamPlaya;

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
float4 Luminance0= Luminance( light.xyz ).xxxx;
float4 Assemble0=float4(Luminance0.x, float4( 0.0, 0.0, 0.0, 0.0 ).y, float4( 0.0, 0.0, 0.0, 0.0 ).z, float4( 0.0, 0.0, 0.0, 0.0 ).w);
float4 Tex2D0=tex2D(_Ilum,Assemble0.xy);
float4 Multiply1=float4( s.Albedo.x, s.Albedo.y, s.Albedo.z, 1.0 ) * Tex2D0;
return Multiply1;

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
				float2 uv_Mar;
float2 uv_MainTex;
float2 uv_costa;

			};

			void vert (inout appdata_full v, out Input o) {
float4 Add3_1_NoInput = float4(0,0,0,0);
float4 Add3=_nivelMar.xxxx + Add3_1_NoInput;
float4 Clamp0_0_NoInput = float4(0,0,0,0);
float4 Clamp0=clamp(Clamp0_0_NoInput,_nivelMar.xxxx,Add3);
float4 Step0=step(Add3,Clamp0);
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Step0;
float4 Subtract2=_nivelMar.xxxx - _tamPlaya.xxxx;
float4 Clamp1_0_NoInput = float4(0,0,0,0);
float4 Clamp1=clamp(Clamp1_0_NoInput,Subtract2,_nivelMar.xxxx);
float4 Step1=step(_nivelMar.xxxx,Clamp1);
float4 Invert1= float4(1.0, 1.0, 1.0, 1.0) - Step1;
float4 Subtract0=Invert0 - Invert1;
float4 Clamp2_0_NoInput = float4(0,0,0,0);
float4 Clamp2=clamp(Clamp2_0_NoInput,float4( 0.0, 0.0, 0.0, 0.0 ),Subtract2);
float4 Step2=step(Subtract2,Clamp2);
float4 Invert2= float4(1.0, 1.0, 1.0, 1.0) - Step2;
float4 Subtract1=Invert1 - Invert2;
float4 Add2=Subtract0 + Subtract1;
float4 Add1=Add2 + Invert2;
float4 Multiply0=float4( v.normal.x, v.normal.y, v.normal.z, 1.0 ) * Add1;
float4 Add0=v.vertex + Multiply0;
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);
v.vertex = Add0;


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D1=tex2D(_Mar,(IN.uv_Mar.xyxy).xy);
float4 Multiply6=_velVer.xxxx * _Time;
float4 UV_Pan2=float4(Tex2D1.x + Multiply6.x,Tex2D1.y + Multiply6.x,Tex2D1.z,Tex2D1.w);
float4 Tex2D0=tex2D(_Mar,UV_Pan2.xy);
float4 Multiply4=_ColorOlas * Tex2D0;
float4 Multiply0=_ColorProfundo * Tex2D0;
float4 Multiply1=Tex2D0 * _ColorMedio;
float4 Multiply9=float4( 0.0005,0.0005,0.0005,0.0005 ) * _SinTime;
float4 UV_Pan0=float4((IN.uv_MainTex.xyxy).x + Multiply9.x,(IN.uv_MainTex.xyxy).y + Multiply9.x,(IN.uv_MainTex.xyxy).z + Multiply9.x,(IN.uv_MainTex.xyxy).w);
float4 Tex2D2=tex2D(_MainTex,UV_Pan0.xy);
float4 Add5=float4( 0.01,0.01,0.01,0.01 ) + Tex2D2;
float4 Multiply7=Add5 * float4( 2,2,2,2 );
float4 Lerp1=lerp(Multiply0,Multiply1,Multiply7);
float4 Multiply2=Tex2D0 * _ColorSuperficie;
float4 Multiply8=_Time * _VelocidadOlas.xxxx;
float4 Sin0=sin(Multiply8);
float4 Splat0=Sin0.x;
float4 Abs0=abs(Splat0);
float4 Add2=float4( 0.1,0.1,0.1,0.1 ) + Abs0;
float4 Multiply5=_Olas.xxxx * Add2;
float4 Add4=Multiply5 + _nivelMar.xxxx;
float4 Add6=_tamPlaya.xxxx + Add4;
float4 Sampled2D2=tex2D(_MainTex,IN.uv_MainTex.xy);
float4 Clamp4=clamp(Sampled2D2,Add4,Add6);
float4 Step5=step(Add6,Clamp4);
float4 Invert5= float4(1.0, 1.0, 1.0, 1.0) - Step5;
float4 Subtract3=_nivelMar.xxxx - _tamPlaya.xxxx;
float4 Clamp5=clamp(Sampled2D2,Subtract3,Add4);
float4 Step4=step(Add4,Clamp5);
float4 Invert4= float4(1.0, 1.0, 1.0, 1.0) - Step4;
float4 Subtract5=Invert5 - Invert4;
float4 Clamp6=clamp(Sampled2D2,float4( 0.0, 0.0, 0.0, 0.0 ),Subtract3);
float4 Step3=step(Subtract3,Clamp6);
float4 Invert3= float4(1.0, 1.0, 1.0, 1.0) - Step3;
float4 Subtract6=Invert4 - Invert3;
float4 Assemble0_3_NoInput = float4(0,0,0,0);
float4 Assemble0=float4(Subtract5.x, Subtract6.y, Invert3.z, Assemble0_3_NoInput.w);
float4 Split0=Assemble0;
float4 Multiply10=Multiply7 * float4( Split0.y, Split0.y, Split0.y, Split0.y);
float4 Lerp7=lerp(Lerp1,Multiply2,Multiply10);
float4 Add0=float4( Split0.y, Split0.y, Split0.y, Split0.y) + float4( Split0.z, Split0.z, Split0.z, Split0.z);
float4 Multiply11=Lerp7 * Add0;
float4 Sampled2D0=tex2D(_costa,IN.uv_costa.xy);
float4 Multiply3=Sampled2D0 * _ColorPlaya;
float4 Clamp3_1_NoInput = float4(0,0,0,0);
float4 Clamp3_2_NoInput = float4(0,0,0,0);
float4 Clamp3=clamp(Sampled2D2,Clamp3_1_NoInput,Clamp3_2_NoInput);
float4 Step6_0_NoInput = float4(0,0,0,0);
float4 Step6=step(Step6_0_NoInput,Clamp3);
float4 Subtract4=Step6 - Invert5;
float4 Lerp0_0_NoInput = float4(0,0,0,0);
float4 Lerp0=lerp(Lerp0_0_NoInput,Multiply3,Subtract4);
float4 Add3=Multiply11 + Lerp0;
float4 Step0=step(Add3,float4( 0.0, 0.0, 0.0, 0.0 ));
float4 Multiply12=Multiply4 * Step0;
float4 Add1=Multiply12 + Add3;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_2_NoInput = float4(0,0,0,0);
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Add1;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}