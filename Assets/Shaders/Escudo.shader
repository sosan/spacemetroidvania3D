Shader "Custom/Escudo"
{
	Properties
	{
		_Globalopacity("Global opacity", Range( 0 , 1)) = 1
		_Maintexture("Main texture", 2D) = "black" {}
		_Maintextureintensity("Main texture intensity", Float) = 1
		_Mainpanningspeed("Main panning speed", Vector) = (0,0,0,0)
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 1
		_TessMin( "Tess Min Distance", Float ) = 10
		_TessMax( "Tess Max Distance", Float ) = 25
		_TessPhongStrength( "Phong Tess Strength", Range( 0, 1 ) ) = 0.5
		[Toggle]_Invertmaintexture("Invert main texture", Range( 0 , 1)) = 0
		[HDR]_Maincolor("Main color", Color) = (0.7941176,0.1284602,0.1284602,0.666)
		[HDR]_Edgecolor("Edge color", Color) = (0.7941176,0.1284602,0.1284602,0.666)
		_Bias("Bias", Float) = 0
		_Scale("Scale", Float) = 1
		_Power("Power", Range( 0 , 5)) = 2
		_Innerfresnelintensity("Inner fresnel intensity", Range( 0 , 1)) = 0
		_Outerfresnelintensity("Outer fresnel intensity", Range( 0 , 1)) = 1
		_Secondarytexture("Secondary texture", 2D) = "black" {}
		_Secondarytextureintensity("Secondary texture intensity", Float) = 1
		_Secondarypanningspeed("Secondary panning speed", Vector) = (0,0,0,0)
		[Toggle]_Invertsecondarytexture("Invert secondary texture", Range( 0 , 1)) = 0
		[HDR]_Secondarycolor("Secondary color", Color) = (0,0,0,0)
		[Toggle]_Enabledistortion("Enable distortion", Range( 0 , 1)) = 0
		_Distortionscale("Distortion scale", Range( 0 , 10)) = 1
		_Distortionspeed("Distortion speed", Range( 0 , 5)) = 1
		_Extraroughness("Extra roughness", Range( 0 , 10)) = 0
		[Toggle]_Enablepulsation("Enable pulsation", Range( 0 , 1)) = 0
		_Pulsephase("Pulse phase", Float) = 0
		_Pulsefrequency("Pulse frequency", Float) = 3
		_Pulseamplitude("Pulse amplitude", Float) = 1
		_Pulseoffset("Pulse offset", Float) = 0
		[Toggle]_Enablenoise("Enable noise", Range( 0 , 1)) = 0
		_Sharpennoise("Sharpen noise", Range( 0 , 1)) = 0
		_Noisescale("Noise scale", Float) = 50
		_Noisespeed("Noise speed", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float _Enabledistortion;
		uniform float _Extraroughness;
		uniform float _Distortionspeed;
		uniform float _Distortionscale;
		uniform float _Enablepulsation;
		uniform float _Pulsefrequency;
		uniform float _Pulsephase;
		uniform float _Pulseamplitude;
		uniform float _Pulseoffset;
		uniform float _Globalopacity;
		uniform float _Outerfresnelintensity;
		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;
		uniform float _Innerfresnelintensity;
		uniform float4 _Edgecolor;
		uniform float3 _Noisespeed;
		uniform float _Noisescale;
		uniform float _Enablenoise;
		uniform float _Sharpennoise;
		uniform float _Secondarytextureintensity;
		uniform float _Invertsecondarytexture;
		uniform sampler2D _Secondarytexture;
		uniform float2 _Secondarypanningspeed;
		uniform float4 _Secondarytexture_ST;
		uniform float4 _Secondarycolor;
		uniform float _Maintextureintensity;
		uniform float _Invertmaintexture;
		uniform sampler2D _Maintexture;
		uniform float2 _Mainpanningspeed;
		uniform float4 _Maintexture_ST;
		uniform float4 _Maincolor;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;
		uniform float _TessPhongStrength;


		float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod3D289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float simplePerlin3D52 = snoise( ( ( _Extraroughness * ase_vertex3Pos ) + ase_vertexNormal + ( _Distortionspeed * _Time.y ) ) );
			float temp_output_76_0 = ( _Distortionscale / 100.0 );
			float3 VertexOut74 = ( ( _Enabledistortion * ( ase_vertexNormal * (( temp_output_76_0 * -1.0 ) + (simplePerlin3D52 - 0.0) * (temp_output_76_0 - ( temp_output_76_0 * -1.0 )) / (1.0 - 0.0)) ) ) + ( _Enablepulsation * ( ase_vertexNormal * (sin( (_Time.y*_Pulsefrequency + _Pulsephase) )*( _Pulseamplitude / 10.0 ) + _Pulseoffset) ) ) );
			v.vertex.xyz += VertexOut74;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( _Bias + _Scale * pow( 1.0 - fresnelNdotV1, _Power ) );
			float clampResult170 = clamp( ( ( _Outerfresnelintensity * fresnelNode1 ) + ( ( 1.0 - fresnelNode1 ) * _Innerfresnelintensity ) ) , 0.0 , 1.0 );
			float FresnelMask68 = clampResult170;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float simplePerlin3D153 = snoise( ( ( _Noisespeed * _Time.y ) + ( _Noisescale * ase_vertex3Pos ) ) );
			float temp_output_157_0 = ( ( ( simplePerlin3D153 + _Enablenoise ) * _Enablenoise ) + ( 1.0 - _Enablenoise ) );
			float temp_output_3_0_g1 = ( 1.0 - temp_output_157_0 );
			float Noise160 = ( ( temp_output_157_0 * ( 1.0 - _Sharpennoise ) ) + ( _Sharpennoise * ( 1.0 - saturate( ( temp_output_3_0_g1 / fwidth( temp_output_3_0_g1 ) ) ) ) ) );
			float4 FresnelOut66 = ( FresnelMask68 * _Edgecolor * Noise160 );
			float2 uv_Secondarytexture = i.uv_texcoord * _Secondarytexture_ST.xy + _Secondarytexture_ST.zw;
			float2 panner37 = ( _Time.y * _Secondarypanningspeed + uv_Secondarytexture);
			float3 desaturateInitialColor171 = tex2D( _Secondarytexture, panner37 ).rgb;
			float desaturateDot171 = dot( desaturateInitialColor171, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar171 = lerp( desaturateInitialColor171, desaturateDot171.xxx, 1.0 );
			float4 SecondaryTexOut72 = ( _Secondarytextureintensity * float4( ( ( _Invertsecondarytexture * ( 1.0 - desaturateVar171 ) ) + ( ( 1.0 - _Invertsecondarytexture ) * desaturateVar171 ) ) , 0.0 ) * _Secondarycolor );
			float2 uv_Maintexture = i.uv_texcoord * _Maintexture_ST.xy + _Maintexture_ST.zw;
			float2 panner26 = ( _Time.y * _Mainpanningspeed + uv_Maintexture);
			float3 desaturateInitialColor56 = tex2D( _Maintexture, panner26 ).rgb;
			float desaturateDot56 = dot( desaturateInitialColor56, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar56 = lerp( desaturateInitialColor56, desaturateDot56.xxx, 1.0 );
			float4 MainTexOut64 = ( _Maintextureintensity * float4( ( ( _Invertmaintexture * ( 1.0 - desaturateVar56 ) ) + ( ( 1.0 - _Invertmaintexture ) * desaturateVar56 ) ) , 0.0 ) * _Maincolor );
			float4 EmissionOut70 = ( FresnelOut66 + SecondaryTexOut72 + MainTexOut64 );
			o.Emission = ( _Globalopacity * EmissionOut70 ).rgb;
			o.Alpha = ( _Globalopacity * FresnelMask68 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction tessphong:_TessPhongStrength 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	
}
