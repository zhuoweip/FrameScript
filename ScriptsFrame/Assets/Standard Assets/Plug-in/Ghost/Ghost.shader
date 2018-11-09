// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Ghost" {
	Properties{
		[PerRendererData]_MainTex("Main Tex", 2D) = "white" {}
		_MainTexAlpha("MainTexAlpha",Range(0,1)) = 1
		_GhostTexAlpha("GhostTexAlpha", Range(0, 1)) = 0.5
		[HideInInspector]_Offset0("Offset 0", vector) = (0, 0, 0, 0) // 这里只显示4个残影，所以传入4个偏移值
		[HideInInspector]_Offset1("Offset 1", vector) = (0, 0, 0, 0)
		[HideInInspector]_Offset2("Offset 2", vector) = (0, 0, 0, 0)
		[HideInInspector]_Offset3("Offset 3", vector) = (0, 0, 0, 0)

		[HideInInspector]_Color("Tint", Color) = (1,1,1,1)
		[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
		[HideInInspector]_ColorMask("Color Mask", Float) = 15
	}

		CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float _MainTexAlpha;
		float _GhostTexAlpha;
		float4 _Offset0;
		float4 _Offset1;
		float4 _Offset2;
		float4 _Offset3;

	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};

	// 在shader中要渲染自身，以及4个残影，所以要定义5个不同的vert函数
	v2f vert_normal(appdata_base v) { // 渲染自身的vert函数
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;
		return o;
	}
	// 渲染4个残影的vert函数
	v2f vert_offset_1(appdata_base v) {
		v2f o;
		float4 pos = mul(unity_ObjectToWorld, v.vertex);
		o.pos = mul(UNITY_MATRIX_VP, pos + _Offset0);
		o.uv = v.texcoord;
		return o;
	}


	v2f vert_offset_2(appdata_base v) {
		v2f o;
		float4 pos = mul(unity_ObjectToWorld, v.vertex);
		o.pos = mul(UNITY_MATRIX_VP, pos + _Offset1);
		o.uv = v.texcoord;
		return o;
	}

	v2f vert_offset_3(appdata_base v) {
		v2f o;
		float4 pos = mul(unity_ObjectToWorld, v.vertex);
		o.pos = mul(UNITY_MATRIX_VP, pos + _Offset2);
		o.uv = v.texcoord;
		return o;
	}

	v2f vert_offset_4(appdata_base v) {
		v2f o;
		float4 pos = mul(unity_ObjectToWorld, v.vertex);
		o.pos = mul(UNITY_MATRIX_VP, pos + _Offset3);
		o.uv = v.texcoord;
		return o;
	}


	// 这里只定义了两个frag函数，分别是渲染自身，以及残影的
	float4 frag_normal(v2f i) : COLOR{
		float4 c;
		c = tex2D(_MainTex, i.uv);
		c.a *= _MainTexAlpha;
		return c;
	}

		float4 frag_color(v2f i) : COLOR{ // 将残影的alpha值设为0.5
		float4 c;
		c = tex2D(_MainTex, i.uv);
		c.a *= _GhostTexAlpha;
		return c;
	}
		ENDCG

		SubShader {
		Tags{ "RenderType" = "ShadowCaster" }

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			// 这里用4个pass来渲染残影，第5个pass渲染自身
			Pass{ // 从最远的开始渲染
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
#pragma vertex vert_offset_4
#pragma fragment frag_color
			ENDCG
		}

			Pass{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
#pragma vertex vert_offset_3
#pragma fragment frag_color
			ENDCG
		}

			Pass{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
#pragma vertex vert_offset_2
#pragma fragment frag_color
			ENDCG
		}

			Pass{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
#pragma vertex vert_offset_1
#pragma fragment frag_color
			ENDCG
		}

			Pass{ // 渲染自身，这时要开启 ZWrite
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
#pragma vertex vert_normal
#pragma fragment frag_normal
			ENDCG
		}
	}
	FallBack "Diffuse"
}