// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Nicrom/LPW/ASE/Low Poly Vegetation"
{
	Properties
	{
		[NoScaleOffset][Header(Surface)][Space]_MainTex("Main Texture", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[Header(Main Bending)][Space]_MBDefaultBending("MB Default Bending", Float) = 0
		[Space]_MBAmplitude("MB Amplitude", Float) = 1.5
		_MBAmplitudeOffset("MB Amplitude Offset", Float) = 2
		[Space]_MBFrequency("MB Frequency", Float) = 1.11
		_MBFrequencyOffset("MB Frequency Offset", Float) = 0
		[Space]_MBPhase("MB Phase", Float) = 1
		[Space]_MBWindDir("MB Wind Dir", Range( 0 , 360)) = 0
		_MBWindDirOffset("MB Wind Dir Offset", Range( 0 , 180)) = 20
		[Space]_MBMaxHeight("MB Max Height", Float) = 10
		[NoScaleOffset][Header(World Space Noise)][Space]_NoiseTexture("Noise Texture", 2D) = "bump" {}
		_NoiseTextureTilling("Noise Tilling - Static (XY), Animated (ZW)", Vector) = (1,1,1,1)
		_NoisePannerSpeed("Noise Panner Speed", Vector) = (0.05,0.03,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _MBWindDir;
		uniform float _MBWindDirOffset;
		uniform sampler2D _NoiseTexture;
		uniform float4 _NoiseTextureTilling;
		uniform float2 _NoisePannerSpeed;
		uniform float _MBAmplitude;
		uniform float _MBAmplitudeOffset;
		uniform float _MBFrequency;
		uniform float _MBFrequencyOffset;
		uniform float _MBPhase;
		uniform float _MBDefaultBending;
		uniform float _MBMaxHeight;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _Metallic;
		uniform float _Smoothness;


		float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
		{
			original -= center;
			float C = cos( angle );
			float S = sin( angle );
			float t = 1 - C;
			float m00 = t * u.x * u.x + C;
			float m01 = t * u.x * u.y - S * u.z;
			float m02 = t * u.x * u.z + S * u.y;
			float m10 = t * u.x * u.y + S * u.z;
			float m11 = t * u.y * u.y + C;
			float m12 = t * u.y * u.z - S * u.x;
			float m20 = t * u.x * u.z - S * u.y;
			float m21 = t * u.y * u.z + S * u.x;
			float m22 = t * u.z * u.z + C;
			float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
			return mul( finalMatrix, original ) + center;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float MB_WindDirection870 = _MBWindDir;
			float MB_WindDirectionOffset1373 = _MBWindDirOffset;
			float3 objToWorld1645 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float2 appendResult1506 = (float2(objToWorld1645.x , objToWorld1645.z));
			float2 WorldSpaceUVs1638 = appendResult1506;
			float2 AnimatedNoiseTilling1639 = (_NoiseTextureTilling).zw;
			float2 panner1643 = ( 0.1 * _Time.y * _NoisePannerSpeed + float2( 0,0 ));
			float4 AnimatedWorldNoise1344 = tex2Dlod( _NoiseTexture, float4( ( ( WorldSpaceUVs1638 * AnimatedNoiseTilling1639 ) + panner1643 ), 0, 0.0) );
			float temp_output_1584_0 = radians( ( ( MB_WindDirection870 + ( MB_WindDirectionOffset1373 * (-1.0 + ((AnimatedWorldNoise1344).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) ) * -1.0 ) );
			float3 appendResult1587 = (float3(cos( temp_output_1584_0 ) , 0.0 , sin( temp_output_1584_0 )));
			float3 worldToObj1646 = mul( unity_WorldToObject, float4( appendResult1587, 1 ) ).xyz;
			float3 worldToObj1647 = mul( unity_WorldToObject, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float3 normalizeResult1581 = normalize( ( worldToObj1646 - worldToObj1647 ) );
			float3 MB_RotationAxis1420 = normalizeResult1581;
			float MB_Amplitude880 = _MBAmplitude;
			float MB_AmplitudeOffset1356 = _MBAmplitudeOffset;
			float2 StaticNoileTilling1640 = (_NoiseTextureTilling).xy;
			float4 StaticWorldNoise1340 = tex2Dlod( _NoiseTexture, float4( ( WorldSpaceUVs1638 * StaticNoileTilling1640 ), 0, 0.0) );
			float3 objToWorld1649 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float MB_Frequency873 = _MBFrequency;
			float MB_FrequencyOffset1474 = _MBFrequencyOffset;
			float MB_Phase1360 = _MBPhase;
			float MB_DefaultBending877 = _MBDefaultBending;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float MB_MaxHeight1335 = _MBMaxHeight;
			float MB_RotationAngle97 = radians( ( ( ( ( MB_Amplitude880 + ( MB_AmplitudeOffset1356 * (StaticWorldNoise1340).r ) ) * sin( ( ( ( objToWorld1649.x + objToWorld1649.z ) + ( _Time.y * ( MB_Frequency873 + ( MB_FrequencyOffset1474 * (StaticWorldNoise1340).r ) ) ) ) * MB_Phase1360 ) ) ) + MB_DefaultBending877 ) * ( ase_vertex3Pos.y / MB_MaxHeight1335 ) ) );
			float3 appendResult1558 = (float3(0.0 , ase_vertex3Pos.y , 0.0));
			float3 rotatedValue1567 = RotateAroundAxis( appendResult1558, ase_vertex3Pos, MB_RotationAxis1420, MB_RotationAngle97 );
			float3 rotatedValue1565 = RotateAroundAxis( float3( 0,0,0 ), rotatedValue1567, MB_RotationAxis1420, MB_RotationAngle97 );
			float3 LocalVertexOffset1045 = ( ( rotatedValue1565 - ase_vertex3Pos ) * step( 0.01 , ase_vertex3Pos.y ) );
			v.vertex.xyz += LocalVertexOffset1045;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 Albedo292 = tex2D( _MainTex, uv_MainTex );
			o.Albedo = Albedo292.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "LPVegetation_MaterialInspector"
}
/*ASEBEGIN
Version=18703
2194.286;690.1429;1314;653;5770.465;-419.133;5.160324;True;False
Node;AmplifyShaderEditor.CommentaryNode;1629;-4479.612,1022.172;Inherit;False;2302.227;639.3289;;21;1457;1340;1344;1635;1636;1633;1644;1643;1630;1632;1634;1631;1640;1637;1642;1639;1638;1641;1506;1528;1645;World Space Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.TransformPositionNode;1645;-4453.878,1097.39;Inherit;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector4Node;1528;-4426.581,1450.637;Inherit;False;Property;_NoiseTextureTilling;Noise Tilling - Static (XY), Animated (ZW);13;0;Create;False;0;0;False;0;False;1,1,1,1;1,1,1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;1506;-4220.553,1136.429;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SwizzleNode;1641;-4075.456,1526.016;Inherit;False;FLOAT2;2;3;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1638;-4061.73,1131.755;Inherit;False;WorldSpaceUVs;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1639;-3897.271,1524.578;Inherit;False;AnimatedNoiseTilling;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SwizzleNode;1642;-4077.456,1398.017;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;1631;-3445.494,1293.164;Inherit;False;1638;WorldSpaceUVs;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;1457;-3466.515,1504.173;Float;False;Property;_NoisePannerSpeed;Noise Panner Speed;14;0;Create;True;0;0;False;0;False;0.05,0.03;0.08,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;1640;-3907.271,1397.578;Inherit;False;StaticNoileTilling;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;1637;-3493.495,1397.163;Inherit;False;1639;AnimatedNoiseTilling;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;1643;-3217.487,1485.275;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;0.1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;1634;-3209.326,1079.317;Inherit;False;1638;WorldSpaceUVs;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;1632;-3226.581,1180.781;Inherit;False;1640;StaticNoileTilling;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1630;-3182.494,1335.164;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;1644;-2992.637,1402.254;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1633;-2973.236,1128.544;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1636;-2788.065,1099.612;Inherit;True;Property;_NoiseTexture;Noise Texture;12;1;[NoScaleOffset];Create;True;0;0;False;2;Header(World Space Noise);Space;False;-1;None;None;True;0;True;bump;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;866;-6014.641,1535.244;Inherit;False;763.7373;893.087;;18;1335;1334;1373;952;1356;873;1474;1360;870;877;687;880;300;1262;1286;480;850;1473;Material Properties;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;1635;-2809.382,1373.937;Inherit;True;Property;_NoiseTex1;NoiseTex;12;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;1636;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;3;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;1344;-2483.04,1374.635;Inherit;False;AnimatedWorldNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1473;-5978.357,1987.908;Inherit;False;Property;_MBFrequencyOffset;MB Frequency Offset;7;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1340;-2461.857,1099.066;Inherit;False;StaticWorldNoise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;1595;-4989.096,2690.196;Inherit;False;2812.515;1025.047;;29;1620;1619;1618;1617;1616;1615;1613;1612;1611;1609;1608;1607;1606;1604;1603;1602;1599;1598;1597;1596;1621;1622;1623;1624;1626;1627;1628;97;1649;Rotation Angle;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1578;-4988.315,1923.213;Inherit;False;2813.804;508.2881;;18;1420;1581;1594;1593;1592;1591;1589;1588;1587;1586;1585;1584;1582;1580;1579;1646;1647;1648;Rotation Axis;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;1592;-4946.373,2192.131;Inherit;False;1344;AnimatedWorldNoise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;1611;-4937.316,3582.835;Inherit;False;1340;StaticWorldNoise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;480;-5981.74,1896.297;Float;False;Property;_MBFrequency;MB Frequency;6;0;Create;True;0;0;False;1;Space;False;1.11;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1474;-5517.464,1987.066;Inherit;False;MB_FrequencyOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;952;-5985.979,2255.598;Float;False;Property;_MBWindDirOffset;MB Wind Dir Offset;10;0;Create;True;0;0;False;0;False;20;0;0;180;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;1607;-4681.315,3582.835;Inherit;False;FLOAT;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;1580;-4683.199,2191.074;Inherit;False;FLOAT;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1623;-4781.316,3486.835;Inherit;False;1474;MB_FrequencyOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;873;-5478.74,1893.297;Float;False;MB_Frequency;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;850;-5985.822,2170.913;Float;False;Property;_MBWindDir;MB Wind Dir;9;0;Create;True;0;0;False;1;Space;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1373;-5540.689,2254.407;Inherit;False;MB_WindDirectionOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1593;-4540.371,2089.131;Inherit;False;1373;MB_WindDirectionOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;1579;-4453.354,2195.904;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1612;-4458.315,3528.835;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;870;-5504.822,2167.363;Float;False;MB_WindDirection;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1624;-4527.315,3430.835;Inherit;False;873;MB_Frequency;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;1609;-4274.316,3476.835;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1262;-5981.74,1799.297;Float;False;Property;_MBAmplitudeOffset;MB Amplitude Offset;5;0;Create;True;0;0;False;0;False;2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1286;-5978.44,2076.997;Float;False;Property;_MBPhase;MB Phase;8;0;Create;True;0;0;False;1;Space;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;1649;-4335.244,3062.619;Inherit;False;Object;World;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TimeNode;1599;-4351.316,3270.835;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;1594;-4309.372,2006.13;Inherit;False;870;MB_WindDirection;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1586;-4215.322,2136.357;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1360;-5473.44,2074.997;Inherit;False;MB_Phase;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1356;-5519.74,1798.297;Inherit;False;MB_AmplitudeOffset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;1588;-4009.299,2075.358;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1589;-4034.266,2193.785;Float;False;Constant;_Float1;Float 0;23;0;Create;True;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1616;-4111.319,3382.835;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;1608;-4101.565,3099.724;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;300;-5980.74,1704.297;Float;False;Property;_MBAmplitude;MB Amplitude;4;0;Create;True;0;0;False;1;Space;False;1.5;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1619;-4166.774,2959.997;Inherit;False;1340;StaticWorldNoise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;1621;-4015.865,2858.071;Inherit;False;1356;MB_AmplitudeOffset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;880;-5472.74,1702.297;Float;False;MB_Amplitude;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1626;-3931.207,3417.319;Inherit;False;1360;MB_Phase;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;1602;-3912.11,2959.945;Inherit;False;FLOAT;0;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;1597;-3866.34,3232.471;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1585;-3824.674,2122.438;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1617;-3697.167,2910.991;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;1584;-3646.905,2122.163;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1596;-3696.259,3310.515;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1334;-5982.689,2339.403;Inherit;False;Property;_MBMaxHeight;MB Max Height;11;0;Create;True;0;0;False;1;Space;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;687;-5982.74,1608.298;Float;False;Property;_MBDefaultBending;MB Default Bending;3;0;Create;True;0;0;False;2;Header(Main Bending);Space;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1622;-3767.865,2797.071;Inherit;False;880;MB_Amplitude;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;1606;-3518.27,3310.742;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;1582;-3429.154,2069.441;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;1591;-3424.924,2179.104;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1335;-5483.689,2339.407;Inherit;False;MB_MaxHeight;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;1603;-3515.129,2848.45;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;877;-5504.74,1606.298;Float;False;MB_DefaultBending;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1618;-3283.988,3056.016;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1628;-3311.461,3485.809;Inherit;False;1335;MB_MaxHeight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;1613;-3297.425,3320.841;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;1627;-3327.61,3210.122;Inherit;False;877;MB_DefaultBending;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;1587;-3236.932,2098.254;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TransformPositionNode;1647;-3077.972,2260.107;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;1598;-3053.578,3412.028;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;1604;-3047.562,3107.039;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;1646;-3080.972,2091.107;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;1648;-2825.972,2179.107;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1620;-2842.802,3255.511;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;1581;-2654.541,2178.19;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RadiansOpNode;1615;-2673.039,3255.875;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;1555;-1920.194,2177.758;Inherit;False;1920.748;759.7495;;16;1567;1559;1560;1569;1568;1572;1558;1570;1045;1575;1566;1565;1564;1563;1562;1561;Main Bending Vertex Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1420;-2476.181,2172.153;Inherit;False;MB_RotationAxis;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;1572;-1852.515,2465.83;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;97;-2495.582,3251.19;Float;False;MB_RotationAngle;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;1570;-1702.208,2621.549;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;1569;-1729.594,2296.877;Inherit;False;1420;MB_RotationAxis;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;1558;-1646.677,2488.308;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;1568;-1745.021,2386.908;Inherit;False;97;MB_RotationAngle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1559;-1344.971,2258.219;Inherit;False;1420;MB_RotationAxis;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;1567;-1416.309,2441.592;Inherit;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;489;-1283.31,1537.797;Inherit;False;1281.093;382.0935;;4;292;295;294;515;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;1560;-1354.37,2349.501;Inherit;False;97;MB_RotationAngle;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;515;-1183.34,1630.506;Float;True;Property;_MainTex;Main Texture;0;1;[NoScaleOffset];Create;False;0;0;False;2;Header(Surface);Space;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;1563;-877.0338,2651.509;Float;False;Constant;_Float2;Float 0;8;0;Create;True;0;0;False;0;False;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;1564;-917.6754,2476.195;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotateAboutAxisNode;1565;-1028.694,2328.387;Inherit;False;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;1562;-907.9887,2736.414;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;294;-906.694,1712.875;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;1566;-648.8582,2711.857;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;1561;-659.0073,2409.004;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;295;-617.595,1631.625;Inherit;True;Property;_MainTexture;Main Texture;0;1;[NoScaleOffset];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1575;-433.1784,2561.454;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;292;-274.6025,1632.662;Float;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1045;-239.1429,2555.471;Float;False;LocalVertexOffset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;1265;386.895,1920.538;Inherit;False;634.495;508.0168;;5;0;1495;1061;1496;296;Master Node;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;1061;454.7657,2294.361;Inherit;False;1045;LocalVertexOffset;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;1328;-19355.95,10790.4;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1495;420.2959,2070.739;Inherit;False;Property;_Metallic;Metallic;1;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1496;420.2959,2157.739;Inherit;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;296;518.4048,1984.629;Inherit;False;292;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;1330;-19002.03,10716.32;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;773.2129,1989.286;Float;False;True;-1;2;LPVegetation_MaterialInspector;0;0;Standard;Nicrom/LPW/ASE/Low Poly Vegetation;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;1;Pragma;multi_compile_instancing;False;;Custom;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1506;0;1645;1
WireConnection;1506;1;1645;3
WireConnection;1641;0;1528;0
WireConnection;1638;0;1506;0
WireConnection;1639;0;1641;0
WireConnection;1642;0;1528;0
WireConnection;1640;0;1642;0
WireConnection;1643;2;1457;0
WireConnection;1630;0;1631;0
WireConnection;1630;1;1637;0
WireConnection;1644;0;1630;0
WireConnection;1644;1;1643;0
WireConnection;1633;0;1634;0
WireConnection;1633;1;1632;0
WireConnection;1636;1;1633;0
WireConnection;1635;1;1644;0
WireConnection;1344;0;1635;0
WireConnection;1340;0;1636;0
WireConnection;1474;0;1473;0
WireConnection;1607;0;1611;0
WireConnection;1580;0;1592;0
WireConnection;873;0;480;0
WireConnection;1373;0;952;0
WireConnection;1579;0;1580;0
WireConnection;1612;0;1623;0
WireConnection;1612;1;1607;0
WireConnection;870;0;850;0
WireConnection;1609;0;1624;0
WireConnection;1609;1;1612;0
WireConnection;1586;0;1593;0
WireConnection;1586;1;1579;0
WireConnection;1360;0;1286;0
WireConnection;1356;0;1262;0
WireConnection;1588;0;1594;0
WireConnection;1588;1;1586;0
WireConnection;1616;0;1599;2
WireConnection;1616;1;1609;0
WireConnection;1608;0;1649;1
WireConnection;1608;1;1649;3
WireConnection;880;0;300;0
WireConnection;1602;0;1619;0
WireConnection;1597;0;1608;0
WireConnection;1597;1;1616;0
WireConnection;1585;0;1588;0
WireConnection;1585;1;1589;0
WireConnection;1617;0;1621;0
WireConnection;1617;1;1602;0
WireConnection;1584;0;1585;0
WireConnection;1596;0;1597;0
WireConnection;1596;1;1626;0
WireConnection;1606;0;1596;0
WireConnection;1582;0;1584;0
WireConnection;1591;0;1584;0
WireConnection;1335;0;1334;0
WireConnection;1603;0;1622;0
WireConnection;1603;1;1617;0
WireConnection;877;0;687;0
WireConnection;1618;0;1603;0
WireConnection;1618;1;1606;0
WireConnection;1587;0;1582;0
WireConnection;1587;2;1591;0
WireConnection;1598;0;1613;2
WireConnection;1598;1;1628;0
WireConnection;1604;0;1618;0
WireConnection;1604;1;1627;0
WireConnection;1646;0;1587;0
WireConnection;1648;0;1646;0
WireConnection;1648;1;1647;0
WireConnection;1620;0;1604;0
WireConnection;1620;1;1598;0
WireConnection;1581;0;1648;0
WireConnection;1615;0;1620;0
WireConnection;1420;0;1581;0
WireConnection;97;0;1615;0
WireConnection;1558;1;1572;2
WireConnection;1567;0;1569;0
WireConnection;1567;1;1568;0
WireConnection;1567;2;1558;0
WireConnection;1567;3;1570;0
WireConnection;1565;0;1559;0
WireConnection;1565;1;1560;0
WireConnection;1565;3;1567;0
WireConnection;294;2;515;0
WireConnection;1566;0;1563;0
WireConnection;1566;1;1562;2
WireConnection;1561;0;1565;0
WireConnection;1561;1;1564;0
WireConnection;295;0;515;0
WireConnection;295;1;294;0
WireConnection;1575;0;1561;0
WireConnection;1575;1;1566;0
WireConnection;292;0;295;0
WireConnection;1045;0;1575;0
WireConnection;0;0;296;0
WireConnection;0;3;1495;0
WireConnection;0;4;1496;0
WireConnection;0;11;1061;0
ASEEND*/
//CHKSM=1C086FFAAE5CA2A534C86A26338EF9F99A7D7558