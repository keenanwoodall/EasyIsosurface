Shader "Isosurface/Triplanar"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Bottom/Top", 2D) = "white" {}
		_MainTex1 ("Forward/Back", 2D) = "white" {}
		_MainTex2 ("Left/Right", 2D) = "white" {}
		_SizeX("SizeX", Float) = 20
		_SizeY("SizeY", Float) = 20
		_NX("X Blend", Range(0,1)) = 1
		_NY("Y Blend", Range(0,1)) = 1
		_NZ("Z Blend", Range(0,1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
 
		CGPROGRAM
		#pragma surface surf Lambert
 
		sampler2D _MainTex;
		sampler2D _MainTex1;
		sampler2D _MainTex2;
		fixed4 _Color;
		fixed _NX;
		fixed _NY;
		fixed _NZ;
		half _SizeX;
		half _SizeY;
 
		struct Input
		{
			half3 worldPos;
			fixed3 worldNormal;
		};
 
		void surf (Input IN, inout SurfaceOutput o)
		{
			half2 scale = half2(_SizeX, _SizeY);
 
			fixed4 c = tex2D(_MainTex, IN.worldPos.xz/scale);
			fixed4 c1 = tex2D(_MainTex1, IN.worldPos.xy/scale);
			fixed4 c2 = tex2D(_MainTex2, IN.worldPos.zy/scale);
 
			fixed3 nWNormal = normalize(IN.worldNormal*fixed3(_NX, _NY, _NZ));
			fixed3 projnormal = saturate(pow(nWNormal*1.5, 4));
 
			half4 result = lerp(c, c1, projnormal.z);
			result = lerp(result, c2, projnormal.x);
 
			o.Albedo = result.rgb * _Color.rgb;
			o.Alpha = result.a * _Color.a;
		}
		ENDCG
	}
 
	Fallback "VertexLit"
}
