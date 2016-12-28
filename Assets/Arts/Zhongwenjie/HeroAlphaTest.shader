
Shader "Zhongwenjie/Hero AlphaTest" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (0.26, 0.19, 0.16, 0.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 2.0
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}

	SubShader 
	{
		Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout" }
		LOD 150
	
		CGPROGRAM
		#pragma surface surf Lambert noforwardadd alphatest:_Cutoff

		fixed4 _Color;
		sampler2D _MainTex;
		float4 _RimColor;
		float _RimPower;

		struct Input 
		{
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldNormal; 
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = tex.rgb;
			o.Gloss = tex.a;
			o.Alpha = tex.a;
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir), IN.worldNormal));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);
		}
		ENDCG
	}

	Fallback "Transparent/Cutout/Diffuse"
}
