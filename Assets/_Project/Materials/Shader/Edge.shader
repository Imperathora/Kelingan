Shader "Hidden/Edge"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Intensity("Intensity", Float) = 1
	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
		{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag

			//#include "UnityCG.cginc"

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

		v2f vert(appdata v)
		{
			v2f o;
			//o.vertex = UnityObjectToClipPos(v.vertex);
			o.vertex.xy = 2.0 * v.vertex - 1.0;
			o.vertex.z = 0.0;
			o.vertex.w = 1.0;
			o.uv = v.uv;
			return o;
		}

		sampler2D _MainTex;
		float _Intensity;

	#define PI 3.1415926

		float4 frag(v2f i) : SV_Target
		{
			float2 center = i.uv;
			float2 left = (i.uv - float2(1, 0) / _ScreenParams.xy);
			float2 right = (i.uv + float2(1, 0) / _ScreenParams.xy);
			float2 below = (i.uv - float2(0, 1) / _ScreenParams.xy);
			float2 above = (i.uv + float2(0, 1) / _ScreenParams.xy);

			float4 col = tex2D(_MainTex, center);

			float4 h_edge = tex2D(_MainTex, left) - tex2D(_MainTex, right);
			float4 v_edge = tex2D(_MainTex, below) - tex2D(_MainTex, above);

			float4 edge4 = sqrt(h_edge * h_edge + v_edge * v_edge) / sqrt(2.0);
			float edge = (edge4.x + edge4.y + edge4.z) / _Intensity;

			//return 5.0 * float4(edge, edge, edge, 1.0) + col;
			return lerp(col, float4(0, 0, 0, 1), saturate(5.0 * edge));
		}
			ENDCG
		}
		}

}
