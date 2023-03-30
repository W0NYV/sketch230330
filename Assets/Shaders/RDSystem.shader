Shader "Hidden/RDSystem"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _CardinalityTexture;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_CardinalityTexture, i.uv);

				float d = col.g;
				d *= 3.0;

				float d2 = max(d - 0.85, 0.0);
				d2 *= 8.5;
				d = d-d2;
				d -= 0.3;
				d *= 3.0;

				return d * float4(0.95, 1.0, 0.85, 1.0);
			}
			ENDCG
		}
	}
}