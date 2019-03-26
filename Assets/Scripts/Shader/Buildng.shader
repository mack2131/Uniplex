Shader "Custom/Buildng" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_ConstructY("ConstructY", Float) = 1
		_ConstructGap("ConstructGap", Float) = 1
		_ConstructColor ("ConstructColor", Color) = (0,0,0,0)
		//_building("building", Int) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque"
			   "ForceNoShadowCasting" = "False" }
		Cull Off
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		
		struct Input 
		{
			float2 uv_MainTex;
			float3 worldPos;
			float3 viewDir;
		};
		
		float _ConstructY;
		float _ConstructGap;
		fixed4 _ConstructColor;
		int building;
		
		#pragma surface surf Standard fullforwardshadows
		
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float3 viewDir;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)
		
		
		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			viewDir = IN.viewDir;
				
			float s = +sin((IN.worldPos.x * IN.worldPos.z) * 2 + _Time[3] + o.Normal) / 120;
			if (IN.worldPos.y > _ConstructY + s + _ConstructGap)
				discard;	
				
			if (IN.worldPos.y < _ConstructY)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				building = 0;
			}
			else
			{
				o.Albedo = _ConstructColor.rgb;
				o.Alpha = _ConstructColor.a;
				building = 1;
			}
			
			if (dot(o.Normal, viewDir) < 0)
			{
				o.Albedo = _ConstructColor.rgb;
				o.Alpha = _ConstructColor.a;
			}
			
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}	
		
		ENDCG
	}
	//FallBack "Diffuse"
}
