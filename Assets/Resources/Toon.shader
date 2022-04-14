Shader "Hidden/Custom/Toon"
{
    HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
//#include "UnityCG.cginc"
//#include "AutoLight.cginc"
//#include "Lighting.cginc"


        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        //TEXTURE2D_SAMPLER2D(_ToonLut, sampler_ToonLut);
        float3 _RimColor;
        float _RimPower;
        float4 _Color;
   

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            return 0;

        //float3 normal = normalize(i.normal);
        //float ndotl = dot(normal, _WorldSpaceLightPos0);
        //float ndotv = saturate(dot(normal, i.viewDir));
        //
        //float3 lut = tex2D(_ToonLut, float2(ndotl, 0));
        //float3 rim = _RimColor * pow(1 - ndotv, _RimPower) * ndotl;
        //
        //float3 directDiffuse = lut * _LightColor0;
        //float3 indirectDiffuse = unity_AmbientSky;
        //
        //float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord) * _Color;
        //col.rgb *= directDiffuse + indirectDiffuse;
        //col.rgb += rim;
        //col.a = 1.0;
        //
        //return col;

       // float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
       // float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
       // color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);
       // return color;
    }

        ENDHLSL

        SubShader
    {
        Cull Off ZWrite Off ZTest Always

            Pass
        {
            HLSLPROGRAM

            #pragma vertex VertDefault
            #pragma fragment Frag
                

            ENDHLSL
        }
    }
}