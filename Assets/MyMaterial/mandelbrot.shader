// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NewSurfaceShader" 
{
	Properties{
		_Exponent("Exponent", Range(1, 10)) = 2
		_MaxIteration("MaxIteration", Range(1, 256)) = 16
		_Threshold("Threshold", Range(1, 100)) = 2
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

		float _Exponent;
	float _Threshold;
	int _MaxIteration;

	// 複素数の積
	float2 cmul(float2 a, float2 b)
	{
		return float2(a.x * b.x - a.y * b.y, a.x * b.y + a.x * b.y);
	}

	// 複素数の累乗
	float2 cpow(float2 v, float p)
	{
		float a = v.x == 0 ? 0 : atan2(v.y, v.x) * p;
		return float2(cos(a), sin(a)) * pow(length(v), p);
	}

	// マンデルブロ集合を計算
	float mandelbrot(float2 c)
	{
		float2 z = float2(0, 0);
		for (int i = 0; i < _MaxIteration; i++)
		{
			z = cpow(z, _Exponent);
			z += c;
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
		float col = mandelbrot((i.uv - 0.5) * 3);
	return float4(col, col, col, 1);
	}
		ENDCG
	}
	}
		Fallback "Diffuse"
}
