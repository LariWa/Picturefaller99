Shader "Custom/WobbleShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_RippleTex("Ripple (S/W)", 2D) = "white" {}
		//_Glossiness("Smoothness", Range(0,1)) = 0.5
		//_Metallic("Metallic", Range(0,1)) = 0.0
		_Amount("Extrusion Amount", Range(-10,10)) = 0.5
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			//LOD 200

			//Lighting Off

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows vertex:vert addshadow
			//#pragma surface surf NoLighting noforwardadd vertex:vert addshadow
			//#pragma surface surf NoLighting
			//#pragma surface surf NoLighting noambient
			//#pragma vertex vert
			//#pragma fragment frag

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			/*fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
			 {
				 fixed4 c;
				 c.rgb = s.Albedo;
				 c.a = s.Alpha;
				 return c;
			 }*/


			sampler2D _MainTex;
			sampler2D _RippleTex;


			struct Input //https://docs.unity3d.com/Manual/SL-SurfaceShaders.html
			{
				float2 uv_MainTex;
				//float2 uv_RippleTex;
				//float3 worldNormal;
				//float3 objectNormal;
				//float3 worldPos;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			float _Amount;

			void vert(inout appdata_full v)
			{
				fixed4 rip = tex2Dlod(_RippleTex, v.texcoord);
				v.vertex.xyz += v.normal * _Amount * rip; // can also access rip.r / g / b
			}

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);// *_Color;
				//fixed4 c = tex2D(_RippleTex, IN.uv_RippleTex);

				//c = (IN.objectNormal * (c * 10)) * IN.objectNormal + worldPos;
				//o.Normal = IN.objectNormal * (c * 10);
				//o.Normal *= c;
				//o.Normal = UnpackNormal(tex2D(_RippleTex, IN.uv_RippleTex))*6;


				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				//o.Metallic = _Metallic;
				//o.Smoothness = _Glossiness;
				//o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
