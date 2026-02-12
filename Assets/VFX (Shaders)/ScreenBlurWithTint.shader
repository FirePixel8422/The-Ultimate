


Shader "ScreenBlurWithTint"
{
    Properties
    {
        _BlurStrength("Blur Strength", Float) = 1
        _TintAlpha("Tint Alpha", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "Queue"="Transparent" "IgnoreProjector"="True" }

        Pass
        {
            ZWrite Off
            Cull Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float _BlurStrength;
            float _TintAlpha;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.uv = input.uv;
                return output;
            }

            half4 SampleKawase(float2 uv, float2 offset)
            {
                // 4 offsets for smoother blur
                half4 col = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv) * 0.25;
                col += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv + float2(offset.x, 0)) * 0.1875;
                col += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv - float2(offset.x, 0)) * 0.1875;
                col += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv + float2(0, offset.y)) * 0.1875;
                col += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, uv - float2(0, offset.y)) * 0.1875;
                return col;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;

                float2 offset = _BlurStrength / _ScreenParams.xy;

                // First pass
                half4 col = SampleKawase(uv, offset);

                // Second pass
                col = SampleKawase(uv, offset * 0.5);

                // Apply black tint overlay
                col.rgb = lerp(col.rgb, float3(0,0,0), _TintAlpha);

                return col;
            }

            ENDHLSL
        }
    }
}
