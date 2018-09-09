// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PengLu/Unlit/MosaicVF" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_MosaicSize("MosaicSize", int) = 100
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass{
		CGPROGRAM
#pragma vertex vert  
#pragma fragment frag  


#include "UnityCG.cginc"  

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;

	};

	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;


	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	half4 _MainTex_TexelSize;
	int _MosaicSize;
	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		//float2 uv = (i.texcoord*_MainTex_TexelSize.zw) ;//将纹理坐标映射到分辨率的大小  
		//         uv = floor(uv/_MosaicSize)*_MosaicSize;//根据马赛克块大小进行取整  
		//         i.texcoord =uv*_MainTex_TexelSize.xy;//把坐标重新映射回0,1的范围之内  
		//         fixed4 col = tex2D(_MainTex, i.texcoord);  

		float2 uv = i.texcoord * float2(1000,1000);
		uv = floor(uv / _MosaicSize)*_MosaicSize;
		i.texcoord = uv* float2(0.001,0.001);
		fixed4 col = tex2D(_MainTex, i.texcoord);

		//UNITY_OPAQUE_ALPHA(col.a);  

		return col;
	}
		ENDCG
	}
	}

}