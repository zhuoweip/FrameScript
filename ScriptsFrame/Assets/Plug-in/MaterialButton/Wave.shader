Shader "Custom/Wave"   
{
    Properties
    {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1, 0, 0, 1)
        _Point1("Point1", vector) = (100, 100, 0, 0)
		_Radius("Radius", range(0, 50000)) = 0
    }
    SubShader
    {
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
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
            };
            struct v2f
            {    
                float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
            };
			
            float4 _Color;
            float4 _Point1;
			float _Radius;
			sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
				fixed4  color = tex2D(_MainTex, i.texcoord) * i.color;
                if(pow((i.vertex.x - _Point1.x), 2) + pow((i.vertex.y - _Point1.y), 2) < _Radius) //set area to draw wave
                {
					return color * _Color; 
                }
				return color;
            }
            ENDCG
        }
    }
}
