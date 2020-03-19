// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Bright" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Hold("hold",Range(0,1)) = 0.5
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200		
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			
			sampler2D _MainTex;
			float _Hold;
			
			struct Out
			{
				float4 pos : POSITION;
				float2 uv : Texcoord0;
			};
			
			Out vert(appdata_img i)
			{
				Out o;
				o.pos = UnityObjectToClipPos(i.vertex);
				o.uv = i.texcoord;
				
				return o;
			}
			
			half4 frag(Out i) : COLOR
			{
				half4 o;
				half4 texColor = tex2D(_MainTex,i.uv);
				o.rgb = max(half3(0,0,0),texColor.rgb - _Hold);
				o.a = 1;
				
				return o;
			}
			ENDCG
			
		}
	}
	
	FallBack "Diffuse"
}
