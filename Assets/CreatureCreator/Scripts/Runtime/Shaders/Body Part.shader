Shader "Creature Creator/Body Part" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _Color;

struct Input {
    float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = _Color;
    o.Albedo = c.rgb;
    o.Alpha = c.a;
}
ENDCG
}

Fallback "Legacy Shaders/VertexLit"
}