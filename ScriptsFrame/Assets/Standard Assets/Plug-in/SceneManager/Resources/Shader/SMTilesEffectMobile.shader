// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Scene Manager/Tiles Effect Mobile" {
	Properties {
		_Backface ("Backface", 2D) = "black" {}
		_ScreenContent ("Screen Content", 2D) = "black" {}
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	sampler2D _ScreenContent;
	half4 _ScreenContent_ST;
	sampler2D _Backface;
	half4 _Backface_ST;	
			
	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half2 uvBackface : TEXCOORD2;
	};
	
	v2f vert(appdata_full v) {
		v2f o;
		o.pos = UnityObjectToClipPos (v.vertex);	
		o.uv = TRANSFORM_TEX(v.texcoord, _ScreenContent); 
		o.uvBackface.xy = TRANSFORM_TEX(v.texcoord, _Backface);
		#if UNITY_UV_STARTS_AT_TOP
		o.uv.y = 1 - o.uv.y;
		#endif		
		return o; 
	}
	
	fixed4 frag(v2f i) : COLOR {	
		return tex2D(_ScreenContent, i.uv.xy);
	}
	
	ENDCG        
	
	SubShader {
		Tags { "RenderType"="Opaque" }
		Lighting Off
		LOD 200
	
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	
		Pass { 
			Cull Front 
			SetTexture [_Backface] {
                Combine texture
            }
		}
	}
	
	FallBack "Diffuse"
} 