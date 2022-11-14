Shader "Creature Creator/Body"
{
    Properties
    {
		_TintColor ("Tint Color", Color) = (1,1,1,1)
		_PrimaryCol ("Primary Color", Color) = (1,1,1,1)
        _SecondaryCol ("Secondary Color", Color) = (0,0,0,1)
        _PatternTex ("Pattern Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.0
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _PatternTex;

        struct Input
        {
            float2 uv_PatternTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _PrimaryCol;
        fixed4 _SecondaryCol;
		fixed4 _TintColor;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color.
            fixed4 c = tex2D (_PatternTex, IN.uv_PatternTex);
			
			// Interpolate between primary and secondary colour using brightness).
			c.rgb = lerp(_SecondaryCol, _PrimaryCol, c.rgb) * _TintColor;
			
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
