Shader "Hidden/SlitsFx"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        [HDR] _Color("", Color) = (1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"
    #include "SimplexNoise2D.hlsl"

    sampler2D _MainTex;
    half3 _Color;
    half _Frequency;
    half _Amplitude;
    float _LocalTime;

    void Vertex(
        float4 position : POSITION,
        float2 uv : TEXCOORD0,
        out float4 outPosition : SV_Position,
        out float2 outUV : TEXCOORD0
    )
    {
        outPosition = UnityObjectToClipPos(position);
        outUV = uv;
    }

    half4 Fragment(
        float4 sv_position : SV_Position,
        float2 uv : TEXCOORD0
    ) : SV_Target
    {
        float width = 0.2 * _Amplitude;
        float t = _LocalTime;

        half2 p1 = half2(uv.x * _Frequency * 2, t);
        half2 p2 = half2(uv.x * _Frequency * 1, t);
        half n = snoise(p1) + snoise(p2) / 2;

        half c1 = 1 - smoothstep(width * 0.99, width,  n);
        half c2 = 1 - smoothstep(width * 0.99, width, -n);
        half slit = c1 * c2;

        half4 src = tex2D(_MainTex, uv);
        return half4(lerp(src.rgb, _Color, slit), src.a);
    }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDCG
        }
    }
}
