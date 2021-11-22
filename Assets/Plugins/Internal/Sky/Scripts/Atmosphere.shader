Shader "Custom/Atmosphere"
{
    Properties
    {
        _ZenithColour("Zenith", Color) = (1,1,1,1)
        _HorizonColour("Horizon", Color) = (1,1,1,1)
		_BlendHeightA("Height A", Range(0.0, 1.0)) = 0.5
		_BlendHeightB("Height B", Range(0.0, 1.0)) = 0.75
    }
    SubShader
    {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
		
		ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
		
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
            #include "UnityCG.cginc"

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

            float4 _ZenithColour;
            float4 _HorizonColour;
			float _BlendHeightA;
			float _BlendHeightB;
			
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float h = i.uv.y;
				float4 col;
				
				static const float pi = 3.141592653589793;
				h = 0.5 * sin(pi * (h - 0.5)) + 0.5; // converts uv.y values to "linear" values
				
				if (h < _BlendHeightA)
				{
					col = _HorizonColour;
				}
				else if (h < _BlendHeightB)
				{			
					float t = (h - _BlendHeightA) / (_BlendHeightB - _BlendHeightA);
					
					col = lerp(_HorizonColour, _ZenithColour, t);
				}
				else
				{
					col = _ZenithColour;
				}
				
				return col;		
            }
            ENDCG
        }
    }
}
