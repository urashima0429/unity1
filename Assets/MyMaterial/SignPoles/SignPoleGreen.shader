// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SignPole" {
	Properties
	{
		_Color("Base Color", Color) = (1,1,1,1)
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic",   Range(0,1)) = 0.0

		_Cutoff("Cutoff", Range(0,1)) = 0.5

		_Speed("Speed", Range(-10, 10)) = 8
		_Twist("Twist", Range(-10, 10)) = 1
		_Div("Div", Range(0, 10)) = 6
		_Rot("Rot", float) = 0
	}

		CGINCLUDE

#define PI 3.1415926535

		struct Input {
		float3 worldPos;
		float3 worldNormal;
	};

	half4 _Color;
	half _Glossiness;
	half _Metallic;

	float _Twist;
	float _Div;
	float _Speed;

	float mod(float a, float b)
	{
		return a - floor(a / b) * b;
	}

	float calcAlpha(float3 pos)
	{
		pos = mul(float4(pos, 1), unity_ObjectToWorld).xyz;
		float3 rot = float3(atan2(pos.x, pos.z), atan2(pos.y, length(pos.xz)), length(pos));
		rot.x += rot.y * _Twist + _Time.y * _Speed;
		rot.x = mod(rot.x * _Div, PI * 2);

		return (rot.x / PI) / 2;
	}

	void surf(Input IN, inout SurfaceOutputStandard o)
	{
		o.Albedo = _Color.rgb;
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = calcAlpha(IN.worldNormal);
	}
	ENDCG

		SubShader
	{
		Tags{ "RenderType" = "TransparentCutout" "Queue" = "AlphaTest" }

		// front-face

		Cull Back

		CGPROGRAM
#pragma surface surf Standard fullforwardshadows addshadow alphatest:_Cutoff
#pragma target 3.0
		ENDCG

		Cull Front
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows addshadow alphatest:_Cutoff
#pragma target 3.0
		ENDCG
	}
		FallBack "Diffuse"
}