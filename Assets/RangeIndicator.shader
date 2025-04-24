Shader "Custom/RangeIndicator"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _RangeMin ("Range Min", Float) = 0.5
        _RangeMax ("Range Max", Float) = 1.0
        _Angle ("Angle", Range(0, 180)) = 90
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalRenderPipeline"
        }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RangeMin;
            float _RangeMax;
            float _Angle;
            float4 _Color;
            float _Glossiness;
            float _Metallic;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float distanceFromCenter = length(IN.worldPos.xz);
                float angle = atan2(IN.worldPos.z, IN.worldPos.x) * 180 / 3.14159265;

                if (distanceFromCenter >= _RangeMin && distanceFromCenter <= _RangeMax && abs(angle) <= _Angle)
                {
                    half4 c = tex2D(_MainTex, IN.uv) * _Color;
                    return c;
                }
                else
                {
                    return half4(0, 0, 0, 0); // 保持透明或黑色
                }
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}