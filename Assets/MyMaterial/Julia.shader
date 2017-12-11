// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Julia"
{
	Properties{
		_MaxIteration("MaxIteration", Range(1, 256)) = 16
		_Threshold("Threshold", Range(1, 100)) = 2
		_Cx("Cx", Range(-1, 1)) = 0
		_Cy("Cy", Range(-1, 1)) = 0
		_Scale("Scale", Range(1, 5)) = 3

		_Exponent("Exponent", Range(0, 3)) = 2

		_ExpExponent("ExpExponent", Range(0, 3)) = 2
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		float _Threshold;
	int _MaxIteration;
	float _Cx;
	float _Cy;
	float _Scale;

	float _Exponent;
	float _ExpExponent;

	// 複素数の指数関数
	float2 cexp(float2 c)
	{
		return exp(c.x) * float2(cos(c.y), sin(c.y));
	}

	// 複素数の積
	float2 cmul(float2 a, float2 b)
	{
		return float2(a.x * b.x - a.y * b.y, a.x * b.y + b.x * a.y);
	}

	// 複素数の累乗
	float2 cpow(float2 v, float p)
	{
		float a = v.x == 0 ? 0 : atan2(v.y, v.x) * p;
		return float2(cos(a), sin(a)) * pow(length(v), p);
	}

	// ジュリア集合を計算
	float julia(float2 z)
	{
		for (int i = 0; i < _MaxIteration; i++)
		{
			z = cmul(cpow(z, _Exponent), cexp(cpow(z, _ExpExponent)));
			z += float2(_Cx, _Cy);
			if (length(z) > _Threshold) return (float)i / _MaxIteration;
		}
		return 1.0;
	}

	// 頂点/フラグメントシェーダー
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
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		float col = julia((i.uv - 0.5) * _Scale);
	return float4(col, col, col, 1);
	}
		ENDCG
	}
	}
		Fallback "Diffuse"
}