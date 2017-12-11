// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ForBox" {

	SubShader{
		Pass{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		float4 vert(float4 v:POSITION) : SV_POSITION{
		return UnityObjectToClipPos(v);
	}

		fixed4 frag() : SV_Target{
		return fixed4(0.2, 0.2, 0.2, 0.3);
	}
		ENDCG
	}
	}
}