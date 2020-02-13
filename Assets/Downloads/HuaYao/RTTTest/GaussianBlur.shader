// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GaussianBlur" 
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass
		{
		
			ZTest Always Cull Off ZWrite Off
	  		Fog { Mode off }  
	  		
	  		
			CGPROGRAM
			//#pragma fragmentoption ARB_precision_hint_fastest
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f
			{
				float4 pos : POSITION;
				half2 uv0 : Texcoord0;
				half4 uv1 : Texcoord1;
				half4 uv2 : Texcoord2;
				half4 uv3 : Texcoord3;
				half4 uv4 : Texcoord4;
			};
	
			sampler2D 	_MainTex;
			half4 		offsets;
			
			v2f vert(appdata_img i)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(i.vertex);
				
				//o.uv0 = i.texcoord.yx;
				o.uv0 = i.texcoord.xy;
				//o.uv0 = MultiplyUV( UNITY_MATRIX_TEXTURE0, i.texcoord );
				
				o.uv1.xyzw = o.uv0.xyxy + half4(-1,1,1,1) * offsets;
				o.uv2.xyzw = o.uv0.xyxy + half4(-1,0,1,0) * offsets;
				o.uv3.xyzw = o.uv0.xyxy + half4(-1,-1,1,-1) * offsets;
				o.uv4.xyzw = o.uv0.xyxy + half4(0,1,0,-1) * offsets;
				return o;
			}
			
			half4 frag(v2f i) : COLOR
			{
				half4 color = half4 (0,0,0,0);
				color += 0.147761 * tex2D(_MainTex,i.uv0);
				
				color += 0.0947416 * tex2D(_MainTex,i.uv1.xy);
				color += 0.0947416 * tex2D(_MainTex,i.uv1.zw);
				
				color += 0.118318 * tex2D(_MainTex,i.uv2.xy);
				color += 0.118318 * tex2D(_MainTex,i.uv2.zw);
				
				color += 0.0947416 * tex2D(_MainTex,i.uv3.xy);  
				color += 0.0947416 * tex2D(_MainTex,i.uv3.zw);
				
				color += 0.118318 * tex2D(_MainTex,i.uv4.xy);
				color += 0.118318 * tex2D(_MainTex,i.uv4.zw);
				
				return color;
			}
			
			ENDCG
		}
	} 
	
	FallBack "Diffuse"
}
