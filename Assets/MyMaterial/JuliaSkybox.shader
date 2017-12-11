// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/JuliaSkybox" 
	{
		Properties
		{
			_MaxIteration("MaxIteration", Range(1, 256)) = 16
			_Threshold("Threshold", Range(1, 100)) = 2
			_Cx("Cx", Range(-1, 1)) = 0
			_Cy("Cy", Range(-1, 1)) = 0
			_Scale("Scale", Range(1, 5)) = 3

			_Exponent("Exponent", Range(0, 3)) = 2

			_ExpExponent("ExpExponent", Range(0, 3)) = 2

			_Color1("Color1", Color) = (1, 1, 1, 1)
			_Color2("Color2", Color) = (0, 0, 0, 1)
		}

			SubShader
		{
			Tags{ "RenderType" = "Background" "Queue" = "Background" }
			Pass
		{
			ZWrite Off
			Cull Off
			Fog{ Mode Off }

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

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
			float a = atan2(v.y, v.x) * p;
			return float2(cos(a), sin(a)) * pow(length(v), p);
		}

		float _Threshold;
		int _MaxIteration;
		float _Cx;
		float _Cy;
		float _Scale;

		float _Exponent;
		float _ExpExponent;

		float3 _Color1;
		float3 _Color2;

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

		struct appdata
		{
			float4 position : POSITION;
			float3 uv : TEXCOORD;
		};

		struct v2f
		{
			float4 position : SV_POSITION;
			float3 uv : TEXCOORD;
		};

		v2f vert(appdata v)
		{
			v2f o;
			o.position = UnityObjectToClipPos(v.position);
			o.uv = v.uv;
			return o;
		}

		fixed4 frag(v2f i) : COLOR
		{
			float j = julia((i.uv.xy - 0.5) * _Scale);
		float3 col = _Color1 * j + _Color2 * (1 - j);
		return float4(col, 1);
		}

			ENDCG
		}
		}
	}