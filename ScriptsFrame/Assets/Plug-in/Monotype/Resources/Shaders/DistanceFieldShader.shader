Shader "Monotype/DistanceField" {
    Properties {
        _MainTex ("Texture", 2D) = "black" { }
        _InsideCut("Inside cut off", Float) = 0.029
        _OutsideCut("Outside cut off", Float) = -0.0108
        _TextureSize("Texture Size", Float) = 2048.0
        _RenderSize("RenderSize", Int) = 40
        _ZTest("ZTest", Int) = 4 //ZTest LessEqual, for UI Text
    }

    CGINCLUDE
    #include "UnityCG.cginc"
    sampler2D _MainTex;
    float     _InsideCut;
    float     _OutsideCut;
    float     _TextureSize;
    int       _RenderSize;

    struct appdata
    {
        float4 vertex : POSITION;   // vertex position
        float2 uv :     TEXCOORD0;  // texture coordinate
        float4 color:   COLOR;      // vertex color
    };

    struct v2f {
        float4 pos :  SV_POSITION;
        float2 uv :   TEXCOORD0;
        float4 color: COLOR;
    };
    ENDCG

    SubShader
    {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }

        Pass {
            Name "Monotype"

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZTest [_ZTest]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = v.uv;
                o.color = v.color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float texture_size = _TextureSize;
                float4 duvdxy = float4(ddx(i.uv), ddy(i.uv));
                float4 dist = tex2D(_MainTex, i.uv);

                float pixSize = length(texture_size*duvdxy);
                float2 cutoff = float2(0.5f + (_OutsideCut * pixSize), 0.5f + (_InsideCut * pixSize));
                float alpha = (clamp(dist.a, cutoff.x, cutoff.y) - cutoff.x) / (cutoff.y - cutoff.x);

                return float4(i.color.rgb, alpha * i.color.a);
            }
        ENDCG
        }
    }
}
