Shader "Unlit/NewUnlitShader Test"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_RippleTex("Ripple (S/W)", 2D) = "white" {}
		_Amount("Extrusion Amount", Range(-10,10)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };


			sampler2D _MainTex;
			sampler2D _RippleTex;
            float4 _MainTex_ST;
			float _Amount;

            v2f vert (appdata v)
            {
                v2f o;

				fixed4 rip = tex2Dlod(_RippleTex, v.vertex/*uv*/);
				v.vertex.xyz += v.normal *  _Amount * rip;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
