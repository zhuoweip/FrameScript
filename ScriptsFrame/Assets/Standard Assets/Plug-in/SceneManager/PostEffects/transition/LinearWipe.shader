Shader "Scene Manager/LinearWipe"
{
	Properties
	{
		_MainTex("MainTex",2d)="white"{}
		_Angle("_Angle",float)=0
		_Value("Value",Range(0,1))=1
		_Color("Color",Color) = (0,0,0,1)
	}

	SubShader
	{
		Tags 
		{
            "IgnoreProjector"="True"
            "Queue"="Overlay"
            "RenderType"="Overlay"
        }
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

//				#if UNITY_UV_STARTS_AT_TOP
//				o.uv.y=1-o.uv.y;
//				#endif

				return o;
			}

			sampler2D _MainTex;
			uniform float _Value;
			uniform float _Angle;
			float4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 c;

				fixed4 screen=tex2D(_MainTex,i.uv);

				fixed4 x=fixed4(i.uv.x,i.uv.x,i.uv.x,1);
				fixed4 y=fixed4(i.uv.y,i.uv.y,i.uv.y,1);

				fixed4 mask=lerp(x,y,_Angle);
				mask=step(1-_Value,mask);
				c=screen*mask;
				c.a = cos(c.r + c.g + c.b);
				return c;
			}
			ENDCG
		}
	}
}
