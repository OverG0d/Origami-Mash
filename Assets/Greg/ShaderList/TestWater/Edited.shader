Shader "Custom/Edited"
{
    Properties
    {
        [NoScaleOffset] Texture2D_29578A37("NormalMap", 2D) = "white" {}
        Vector1_9E1333AC("RefractStrength", Float) = 0
        Vector1_E0C9B14B("DepthDistance", Float) = 0
        Vector1_B9D5A880("DepthStrength", Float) = 0
        Vector1_FE9E9249("NormalStrengthVector", Range(0, 1)) = 0
        Vector1_C8C7ABCA("PanSpeed", Float) = 0
        Vector1_6E50E8E7("NormalTiling", Float) = 0
        [NoScaleOffset]Texture2D_17583793("Displacement", 2D) = "white" {}
        Vector1_1AB3DB99("DisplacementStrength", Float) = 0
        Color_A19D7DB2("TopColour", Color) = (0.03301889, 0.9484667, 1, 0)
        Color_17013314("BottomColour", Color) = (0, 0.2251563, 1, 0)
    }
        SubShader
        {
            Tags
            {
                "RenderPipeline" = "UniversalPipeline"
                "RenderType" = "Transparent"
                "Queue" = "Transparent+0"
            }

            Pass
            {
                Name "Universal Forward"
                Tags
                {
                    "LightMode" = "UniversalForward"
                }

            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _AlphaClip 1
            #define _NORMALMAP 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS 
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS_FORWARD
            #define REQUIRE_DEPTH_TEXTURE
            #define REQUIRE_OPAQUE_TEXTURE

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float Vector1_9E1333AC;
            float Vector1_E0C9B14B;
            float Vector1_B9D5A880;
            float Vector1_FE9E9249;
            float Vector1_C8C7ABCA;
            float Vector1_6E50E8E7;
            float Vector1_1AB3DB99;
            float4 Color_A19D7DB2;
            float4 Color_17013314;
            CBUFFER_END
            TEXTURE2D(Texture2D_29578A37); SAMPLER(samplerTexture2D_29578A37); float4 Texture2D_29578A37_TexelSize;
            TEXTURE2D(Texture2D_17583793); SAMPLER(samplerTexture2D_17583793); float4 Texture2D_17583793_TexelSize;
            SAMPLER(_SampleTexture2DLOD_90AE17E8_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2DLOD_CB399CA6_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_AD662BE1_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_4505C472_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_700E9753_Sampler_3_Linear_Repeat);

            // Graph Functions

            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }

            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }

            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }

            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }

            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
            {
                Out = lerp(A, B, T);
            }

            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }

            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A + B;
            }

            void Unity_SceneColor_float(float4 UV, out float3 Out)
            {
                Out = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV.xy);
            }

            void Unity_SceneDepth_Linear01_float(float4 UV, out float Out)
            {
                Out = Linear01Depth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }

            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }

            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }

            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
            {
                Out = lerp(A, B, T);
            }

            void Unity_Saturate_float3(float3 In, out float3 Out)
            {
                Out = saturate(In);
            }

            void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
            {
                Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
            }

            void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
            {
                Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
            }

            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
                float4 uv0;
                float3 TimeParameters;
            };

            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };

            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Split_5C99FBDE_R_1 = IN.ObjectSpacePosition[0];
                float _Split_5C99FBDE_G_2 = IN.ObjectSpacePosition[1];
                float _Split_5C99FBDE_B_3 = IN.ObjectSpacePosition[2];
                float _Split_5C99FBDE_A_4 = 0;
                float _Property_15BA5427_Out_0 = Vector1_6E50E8E7;
                float _Multiply_84A22B8D_Out_2;
                Unity_Multiply_float(_Property_15BA5427_Out_0, 1.5, _Multiply_84A22B8D_Out_2);
                float2 _Vector2_49268348_Out_0 = float2(0.1, 1);
                float2 _Multiply_DEC6DFE8_Out_2;
                Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_49268348_Out_0, _Multiply_DEC6DFE8_Out_2);
                float _Property_DD8A2173_Out_0 = Vector1_C8C7ABCA;
                float2 _Multiply_7CA558EB_Out_2;
                Unity_Multiply_float(_Multiply_DEC6DFE8_Out_2, (_Property_DD8A2173_Out_0.xx), _Multiply_7CA558EB_Out_2);
                float2 _TilingAndOffset_52970F6_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, (_Multiply_84A22B8D_Out_2.xx), _Multiply_7CA558EB_Out_2, _TilingAndOffset_52970F6_Out_3);
                float4 _SampleTexture2DLOD_90AE17E8_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_52970F6_Out_3, 0);
                float _SampleTexture2DLOD_90AE17E8_R_5 = _SampleTexture2DLOD_90AE17E8_RGBA_0.r;
                float _SampleTexture2DLOD_90AE17E8_G_6 = _SampleTexture2DLOD_90AE17E8_RGBA_0.g;
                float _SampleTexture2DLOD_90AE17E8_B_7 = _SampleTexture2DLOD_90AE17E8_RGBA_0.b;
                float _SampleTexture2DLOD_90AE17E8_A_8 = _SampleTexture2DLOD_90AE17E8_RGBA_0.a;
                float _Property_55FEC0A0_Out_0 = Vector1_6E50E8E7;
                float2 _Vector2_719B031B_Out_0 = float2(0.3, -1);
                float2 _Multiply_B9F078AF_Out_2;
                Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_719B031B_Out_0, _Multiply_B9F078AF_Out_2);
                float _Property_7A36E455_Out_0 = Vector1_C8C7ABCA;
                float2 _Multiply_A3BEABAC_Out_2;
                Unity_Multiply_float(_Multiply_B9F078AF_Out_2, (_Property_7A36E455_Out_0.xx), _Multiply_A3BEABAC_Out_2);
                float2 _TilingAndOffset_3F74A7B2_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, (_Property_55FEC0A0_Out_0.xx), _Multiply_A3BEABAC_Out_2, _TilingAndOffset_3F74A7B2_Out_3);
                float4 _SampleTexture2DLOD_CB399CA6_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_3F74A7B2_Out_3, 0);
                float _SampleTexture2DLOD_CB399CA6_R_5 = _SampleTexture2DLOD_CB399CA6_RGBA_0.r;
                float _SampleTexture2DLOD_CB399CA6_G_6 = _SampleTexture2DLOD_CB399CA6_RGBA_0.g;
                float _SampleTexture2DLOD_CB399CA6_B_7 = _SampleTexture2DLOD_CB399CA6_RGBA_0.b;
                float _SampleTexture2DLOD_CB399CA6_A_8 = _SampleTexture2DLOD_CB399CA6_RGBA_0.a;
                float _Add_77F26DA0_Out_2;
                Unity_Add_float(_SampleTexture2DLOD_90AE17E8_R_5, _SampleTexture2DLOD_CB399CA6_R_5, _Add_77F26DA0_Out_2);
                float _Saturate_E41E12E3_Out_1;
                Unity_Saturate_float(_Add_77F26DA0_Out_2, _Saturate_E41E12E3_Out_1);
                float _Property_6421F4FD_Out_0 = Vector1_1AB3DB99;
                float _Multiply_553C6DE2_Out_2;
                Unity_Multiply_float(_Saturate_E41E12E3_Out_1, _Property_6421F4FD_Out_0, _Multiply_553C6DE2_Out_2);
                float3 _Vector3_74CFAD86_Out_0 = float3(_Split_5C99FBDE_R_1, _Multiply_553C6DE2_Out_2, _Split_5C99FBDE_B_3);
                description.VertexPosition = _Vector3_74CFAD86_Out_0;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }

            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 WorldSpacePosition;
                float4 ScreenPosition;
                float4 uv0;
                float3 TimeParameters;
            };

            struct SurfaceDescription
            {
                float3 Albedo;
                float3 Normal;
                float3 Emission;
                float Metallic;
                float Smoothness;
                float Occlusion;
                float Alpha;
                float AlphaClipThreshold;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _Property_A04E65CB_Out_0 = Color_17013314;
                float4 _Property_532E50A6_Out_0 = Color_A19D7DB2;
                float _Property_15BA5427_Out_0 = Vector1_6E50E8E7;
                float _Multiply_84A22B8D_Out_2;
                Unity_Multiply_float(_Property_15BA5427_Out_0, 1.5, _Multiply_84A22B8D_Out_2);
                float2 _Vector2_49268348_Out_0 = float2(0.1, 1);
                float2 _Multiply_DEC6DFE8_Out_2;
                Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_49268348_Out_0, _Multiply_DEC6DFE8_Out_2);
                float _Property_DD8A2173_Out_0 = Vector1_C8C7ABCA;
                float2 _Multiply_7CA558EB_Out_2;
                Unity_Multiply_float(_Multiply_DEC6DFE8_Out_2, (_Property_DD8A2173_Out_0.xx), _Multiply_7CA558EB_Out_2);
                float2 _TilingAndOffset_52970F6_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, (_Multiply_84A22B8D_Out_2.xx), _Multiply_7CA558EB_Out_2, _TilingAndOffset_52970F6_Out_3);
                float4 _SampleTexture2DLOD_90AE17E8_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_52970F6_Out_3, 0);
                float _SampleTexture2DLOD_90AE17E8_R_5 = _SampleTexture2DLOD_90AE17E8_RGBA_0.r;
                float _SampleTexture2DLOD_90AE17E8_G_6 = _SampleTexture2DLOD_90AE17E8_RGBA_0.g;
                float _SampleTexture2DLOD_90AE17E8_B_7 = _SampleTexture2DLOD_90AE17E8_RGBA_0.b;
                float _SampleTexture2DLOD_90AE17E8_A_8 = _SampleTexture2DLOD_90AE17E8_RGBA_0.a;
                float _Property_55FEC0A0_Out_0 = Vector1_6E50E8E7;
                float2 _Vector2_719B031B_Out_0 = float2(0.3, -1);
                float2 _Multiply_B9F078AF_Out_2;
                Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_719B031B_Out_0, _Multiply_B9F078AF_Out_2);
                float _Property_7A36E455_Out_0 = Vector1_C8C7ABCA;
                float2 _Multiply_A3BEABAC_Out_2;
                Unity_Multiply_float(_Multiply_B9F078AF_Out_2, (_Property_7A36E455_Out_0.xx), _Multiply_A3BEABAC_Out_2);
                float2 _TilingAndOffset_3F74A7B2_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, (_Property_55FEC0A0_Out_0.xx), _Multiply_A3BEABAC_Out_2, _TilingAndOffset_3F74A7B2_Out_3);
                float4 _SampleTexture2DLOD_CB399CA6_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_3F74A7B2_Out_3, 0);
                float _SampleTexture2DLOD_CB399CA6_R_5 = _SampleTexture2DLOD_CB399CA6_RGBA_0.r;
                float _SampleTexture2DLOD_CB399CA6_G_6 = _SampleTexture2DLOD_CB399CA6_RGBA_0.g;
                float _SampleTexture2DLOD_CB399CA6_B_7 = _SampleTexture2DLOD_CB399CA6_RGBA_0.b;
                float _SampleTexture2DLOD_CB399CA6_A_8 = _SampleTexture2DLOD_CB399CA6_RGBA_0.a;
                float _Add_77F26DA0_Out_2;
                Unity_Add_float(_SampleTexture2DLOD_90AE17E8_R_5, _SampleTexture2DLOD_CB399CA6_R_5, _Add_77F26DA0_Out_2);
                float _Saturate_E41E12E3_Out_1;
                Unity_Saturate_float(_Add_77F26DA0_Out_2, _Saturate_E41E12E3_Out_1);
                float4 _Lerp_EAAAE123_Out_3;
                Unity_Lerp_float4(_Property_A04E65CB_Out_0, _Property_532E50A6_Out_0, (_Saturate_E41E12E3_Out_1.xxxx), _Lerp_EAAAE123_Out_3);
                float4 _ScreenPosition_2A4C382C_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                float _Split_F5815293_R_1 = _ScreenPosition_2A4C382C_Out_0[0];
                float _Split_F5815293_G_2 = _ScreenPosition_2A4C382C_Out_0[1];
                float _Split_F5815293_B_3 = _ScreenPosition_2A4C382C_Out_0[2];
                float _Split_F5815293_A_4 = _ScreenPosition_2A4C382C_Out_0[3];
                float4 _Combine_390BEBAA_RGBA_4;
                float3 _Combine_390BEBAA_RGB_5;
                float2 _Combine_390BEBAA_RG_6;
                Unity_Combine_float(_Split_F5815293_R_1, _Split_F5815293_G_2, 0, 0, _Combine_390BEBAA_RGBA_4, _Combine_390BEBAA_RGB_5, _Combine_390BEBAA_RG_6);
                float4 _SampleTexture2D_AD662BE1_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_29578A37, samplerTexture2D_29578A37, _TilingAndOffset_3F74A7B2_Out_3);
                _SampleTexture2D_AD662BE1_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_AD662BE1_RGBA_0);
                float _SampleTexture2D_AD662BE1_R_4 = _SampleTexture2D_AD662BE1_RGBA_0.r;
                float _SampleTexture2D_AD662BE1_G_5 = _SampleTexture2D_AD662BE1_RGBA_0.g;
                float _SampleTexture2D_AD662BE1_B_6 = _SampleTexture2D_AD662BE1_RGBA_0.b;
                float _SampleTexture2D_AD662BE1_A_7 = _SampleTexture2D_AD662BE1_RGBA_0.a;
                float4 _Combine_D817EB00_RGBA_4;
                float3 _Combine_D817EB00_RGB_5;
                float2 _Combine_D817EB00_RG_6;
                Unity_Combine_float(_SampleTexture2D_AD662BE1_R_4, _SampleTexture2D_AD662BE1_G_5, 0, 0, _Combine_D817EB00_RGBA_4, _Combine_D817EB00_RGB_5, _Combine_D817EB00_RG_6);
                float _Property_161D095F_Out_0 = Vector1_9E1333AC;
                float2 _Multiply_E7BAE2BC_Out_2;
                Unity_Multiply_float(_Combine_D817EB00_RG_6, (_Property_161D095F_Out_0.xx), _Multiply_E7BAE2BC_Out_2);
                float2 _Add_4BBA4B9E_Out_2;
                Unity_Add_float2(_Combine_390BEBAA_RG_6, _Multiply_E7BAE2BC_Out_2, _Add_4BBA4B9E_Out_2);
                float3 _SceneColor_8E29EF8E_Out_1;
                Unity_SceneColor_float((float4(_Add_4BBA4B9E_Out_2, 0.0, 1.0)), _SceneColor_8E29EF8E_Out_1);
                float _SceneDepth_85022D96_Out_1;
                Unity_SceneDepth_Linear01_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_85022D96_Out_1);
                float _Multiply_B09530DE_Out_2;
                Unity_Multiply_float(_SceneDepth_85022D96_Out_1, _ProjectionParams.z, _Multiply_B09530DE_Out_2);
                float4 _ScreenPosition_5BB9DC9_Out_0 = IN.ScreenPosition;
                float _Split_44E4B6AD_R_1 = _ScreenPosition_5BB9DC9_Out_0[0];
                float _Split_44E4B6AD_G_2 = _ScreenPosition_5BB9DC9_Out_0[1];
                float _Split_44E4B6AD_B_3 = _ScreenPosition_5BB9DC9_Out_0[2];
                float _Split_44E4B6AD_A_4 = _ScreenPosition_5BB9DC9_Out_0[3];
                float _Property_983ECE44_Out_0 = Vector1_E0C9B14B;
                float _Multiply_ACECC948_Out_2;
                Unity_Multiply_float(_Split_44E4B6AD_A_4, _Property_983ECE44_Out_0, _Multiply_ACECC948_Out_2);
                float _Subtract_C1760102_Out_2;
                Unity_Subtract_float(_Multiply_B09530DE_Out_2, _Multiply_ACECC948_Out_2, _Subtract_C1760102_Out_2);
                float _OneMinus_490CA95D_Out_1;
                Unity_OneMinus_float(_Subtract_C1760102_Out_2, _OneMinus_490CA95D_Out_1);
                float _Property_97100872_Out_0 = Vector1_B9D5A880;
                float _Multiply_D97971B7_Out_2;
                Unity_Multiply_float(_OneMinus_490CA95D_Out_1, _Property_97100872_Out_0, _Multiply_D97971B7_Out_2);
                float _Saturate_D8E9DE67_Out_1;
                Unity_Saturate_float(_Multiply_D97971B7_Out_2, _Saturate_D8E9DE67_Out_1);
                float3 _Lerp_8566F9F4_Out_3;
                Unity_Lerp_float3((_Lerp_EAAAE123_Out_3.xyz), _SceneColor_8E29EF8E_Out_1, (_Saturate_D8E9DE67_Out_1.xxx), _Lerp_8566F9F4_Out_3);
                float3 _Saturate_C4759D20_Out_1;
                Unity_Saturate_float3(_Lerp_8566F9F4_Out_3, _Saturate_C4759D20_Out_1);
                float4 _SampleTexture2D_4505C472_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_29578A37, samplerTexture2D_29578A37, _TilingAndOffset_52970F6_Out_3);
                _SampleTexture2D_4505C472_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_4505C472_RGBA_0);
                float _SampleTexture2D_4505C472_R_4 = _SampleTexture2D_4505C472_RGBA_0.r;
                float _SampleTexture2D_4505C472_G_5 = _SampleTexture2D_4505C472_RGBA_0.g;
                float _SampleTexture2D_4505C472_B_6 = _SampleTexture2D_4505C472_RGBA_0.b;
                float _SampleTexture2D_4505C472_A_7 = _SampleTexture2D_4505C472_RGBA_0.a;
                float _Property_3AFA4D1D_Out_0 = Vector1_FE9E9249;
                float3 _NormalStrength_A52AA518_Out_2;
                Unity_NormalStrength_float((_SampleTexture2D_4505C472_RGBA_0.xyz), _Property_3AFA4D1D_Out_0, _NormalStrength_A52AA518_Out_2);
                float4 _SampleTexture2D_700E9753_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_29578A37, samplerTexture2D_29578A37, _TilingAndOffset_3F74A7B2_Out_3);
                _SampleTexture2D_700E9753_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_700E9753_RGBA_0);
                float _SampleTexture2D_700E9753_R_4 = _SampleTexture2D_700E9753_RGBA_0.r;
                float _SampleTexture2D_700E9753_G_5 = _SampleTexture2D_700E9753_RGBA_0.g;
                float _SampleTexture2D_700E9753_B_6 = _SampleTexture2D_700E9753_RGBA_0.b;
                float _SampleTexture2D_700E9753_A_7 = _SampleTexture2D_700E9753_RGBA_0.a;
                float3 _NormalStrength_8908D548_Out_2;
                Unity_NormalStrength_float((_SampleTexture2D_700E9753_RGBA_0.xyz), _Property_3AFA4D1D_Out_0, _NormalStrength_8908D548_Out_2);
                float3 _NormalBlend_D2B57619_Out_2;
                Unity_NormalBlend_float(_NormalStrength_A52AA518_Out_2, _NormalStrength_8908D548_Out_2, _NormalBlend_D2B57619_Out_2);
                surface.Albedo = _Saturate_C4759D20_Out_1;
                surface.Normal = _NormalBlend_D2B57619_Out_2;
                surface.Emission = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                surface.Metallic = 0;
                surface.Smoothness = 0.5;
                surface.Occlusion = 1;
                surface.Alpha = 1;
                surface.AlphaClipThreshold = 0.5;
                return surface;
            }

            // --------------------------------------------------
            // Structs and Packing

            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };

            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float3 normalWS;
                float4 tangentWS;
                float4 texCoord0;
                float3 viewDirectionWS;
                #if defined(LIGHTMAP_ON)
                float2 lightmapUV;
                #endif
                #if !defined(LIGHTMAP_ON)
                float3 sh;
                #endif
                float4 fogFactorAndVertexLight;
                float4 shadowCoord;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if defined(LIGHTMAP_ON)
                #endif
                #if !defined(LIGHTMAP_ON)
                #endif
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                float3 interp01 : TEXCOORD1;
                float4 interp02 : TEXCOORD2;
                float4 interp03 : TEXCOORD3;
                float3 interp04 : TEXCOORD4;
                float2 interp05 : TEXCOORD5;
                float3 interp06 : TEXCOORD6;
                float4 interp07 : TEXCOORD7;
                float4 interp08 : TEXCOORD8;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyzw = input.tangentWS;
                output.interp03.xyzw = input.texCoord0;
                output.interp04.xyz = input.viewDirectionWS;
                #if defined(LIGHTMAP_ON)
                output.interp05.xy = input.lightmapUV;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.interp06.xyz = input.sh;
                #endif
                output.interp07.xyzw = input.fogFactorAndVertexLight;
                output.interp08.xyzw = input.shadowCoord;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.tangentWS = input.interp02.xyzw;
                output.texCoord0 = input.interp03.xyzw;
                output.viewDirectionWS = input.interp04.xyz;
                #if defined(LIGHTMAP_ON)
                output.lightmapUV = input.interp05.xy;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.sh = input.interp06.xyz;
                #endif
                output.fogFactorAndVertexLight = input.interp07.xyzw;
                output.shadowCoord = input.interp08.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                output.ObjectSpaceNormal = input.normalOS;
                output.ObjectSpaceTangent = input.tangentOS;
                output.ObjectSpacePosition = input.positionOS;
                output.uv0 = input.uv0;
                output.TimeParameters = _TimeParameters.xyz;

                return output;
            }

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                output.WorldSpacePosition = input.positionWS;
                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                output.uv0 = input.texCoord0;
                output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }


            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

                // Render State
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                Cull Back
                ZTest LEqual
                ZWrite On
                // ColorMask: <None>


                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                // Pragmas
                #pragma prefer_hlslcc gles
                #pragma exclude_renderers d3d11_9x
                #pragma target 2.0
                #pragma multi_compile_instancing

                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>

                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _AlphaClip 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define FEATURES_GRAPH_VERTEX
                #define SHADERPASS_SHADOWCASTER

                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float Vector1_9E1333AC;
                float Vector1_E0C9B14B;
                float Vector1_B9D5A880;
                float Vector1_FE9E9249;
                float Vector1_C8C7ABCA;
                float Vector1_6E50E8E7;
                float Vector1_1AB3DB99;
                float4 Color_A19D7DB2;
                float4 Color_17013314;
                CBUFFER_END
                TEXTURE2D(Texture2D_29578A37); SAMPLER(samplerTexture2D_29578A37); float4 Texture2D_29578A37_TexelSize;
                TEXTURE2D(Texture2D_17583793); SAMPLER(samplerTexture2D_17583793); float4 Texture2D_17583793_TexelSize;
                SAMPLER(_SampleTexture2DLOD_90AE17E8_Sampler_3_Linear_Repeat);
                SAMPLER(_SampleTexture2DLOD_CB399CA6_Sampler_3_Linear_Repeat);

                // Graph Functions

                void Unity_Multiply_float(float A, float B, out float Out)
                {
                    Out = A * B;
                }

                void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                {
                    Out = A * B;
                }

                void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                {
                    Out = UV * Tiling + Offset;
                }

                void Unity_Add_float(float A, float B, out float Out)
                {
                    Out = A + B;
                }

                void Unity_Saturate_float(float In, out float Out)
                {
                    Out = saturate(In);
                }

                // Graph Vertex
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                    float4 uv0;
                    float3 TimeParameters;
                };

                struct VertexDescription
                {
                    float3 VertexPosition;
                    float3 VertexNormal;
                    float3 VertexTangent;
                };

                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    float _Split_5C99FBDE_R_1 = IN.ObjectSpacePosition[0];
                    float _Split_5C99FBDE_G_2 = IN.ObjectSpacePosition[1];
                    float _Split_5C99FBDE_B_3 = IN.ObjectSpacePosition[2];
                    float _Split_5C99FBDE_A_4 = 0;
                    float _Property_15BA5427_Out_0 = Vector1_6E50E8E7;
                    float _Multiply_84A22B8D_Out_2;
                    Unity_Multiply_float(_Property_15BA5427_Out_0, 1.5, _Multiply_84A22B8D_Out_2);
                    float2 _Vector2_49268348_Out_0 = float2(0.1, 1);
                    float2 _Multiply_DEC6DFE8_Out_2;
                    Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_49268348_Out_0, _Multiply_DEC6DFE8_Out_2);
                    float _Property_DD8A2173_Out_0 = Vector1_C8C7ABCA;
                    float2 _Multiply_7CA558EB_Out_2;
                    Unity_Multiply_float(_Multiply_DEC6DFE8_Out_2, (_Property_DD8A2173_Out_0.xx), _Multiply_7CA558EB_Out_2);
                    float2 _TilingAndOffset_52970F6_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, (_Multiply_84A22B8D_Out_2.xx), _Multiply_7CA558EB_Out_2, _TilingAndOffset_52970F6_Out_3);
                    float4 _SampleTexture2DLOD_90AE17E8_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_52970F6_Out_3, 0);
                    float _SampleTexture2DLOD_90AE17E8_R_5 = _SampleTexture2DLOD_90AE17E8_RGBA_0.r;
                    float _SampleTexture2DLOD_90AE17E8_G_6 = _SampleTexture2DLOD_90AE17E8_RGBA_0.g;
                    float _SampleTexture2DLOD_90AE17E8_B_7 = _SampleTexture2DLOD_90AE17E8_RGBA_0.b;
                    float _SampleTexture2DLOD_90AE17E8_A_8 = _SampleTexture2DLOD_90AE17E8_RGBA_0.a;
                    float _Property_55FEC0A0_Out_0 = Vector1_6E50E8E7;
                    float2 _Vector2_719B031B_Out_0 = float2(0.3, -1);
                    float2 _Multiply_B9F078AF_Out_2;
                    Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_719B031B_Out_0, _Multiply_B9F078AF_Out_2);
                    float _Property_7A36E455_Out_0 = Vector1_C8C7ABCA;
                    float2 _Multiply_A3BEABAC_Out_2;
                    Unity_Multiply_float(_Multiply_B9F078AF_Out_2, (_Property_7A36E455_Out_0.xx), _Multiply_A3BEABAC_Out_2);
                    float2 _TilingAndOffset_3F74A7B2_Out_3;
                    Unity_TilingAndOffset_float(IN.uv0.xy, (_Property_55FEC0A0_Out_0.xx), _Multiply_A3BEABAC_Out_2, _TilingAndOffset_3F74A7B2_Out_3);
                    float4 _SampleTexture2DLOD_CB399CA6_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_3F74A7B2_Out_3, 0);
                    float _SampleTexture2DLOD_CB399CA6_R_5 = _SampleTexture2DLOD_CB399CA6_RGBA_0.r;
                    float _SampleTexture2DLOD_CB399CA6_G_6 = _SampleTexture2DLOD_CB399CA6_RGBA_0.g;
                    float _SampleTexture2DLOD_CB399CA6_B_7 = _SampleTexture2DLOD_CB399CA6_RGBA_0.b;
                    float _SampleTexture2DLOD_CB399CA6_A_8 = _SampleTexture2DLOD_CB399CA6_RGBA_0.a;
                    float _Add_77F26DA0_Out_2;
                    Unity_Add_float(_SampleTexture2DLOD_90AE17E8_R_5, _SampleTexture2DLOD_CB399CA6_R_5, _Add_77F26DA0_Out_2);
                    float _Saturate_E41E12E3_Out_1;
                    Unity_Saturate_float(_Add_77F26DA0_Out_2, _Saturate_E41E12E3_Out_1);
                    float _Property_6421F4FD_Out_0 = Vector1_1AB3DB99;
                    float _Multiply_553C6DE2_Out_2;
                    Unity_Multiply_float(_Saturate_E41E12E3_Out_1, _Property_6421F4FD_Out_0, _Multiply_553C6DE2_Out_2);
                    float3 _Vector3_74CFAD86_Out_0 = float3(_Split_5C99FBDE_R_1, _Multiply_553C6DE2_Out_2, _Split_5C99FBDE_B_3);
                    description.VertexPosition = _Vector3_74CFAD86_Out_0;
                    description.VertexNormal = IN.ObjectSpaceNormal;
                    description.VertexTangent = IN.ObjectSpaceTangent;
                    return description;
                }

                // Graph Pixel
                struct SurfaceDescriptionInputs
                {
                };

                struct SurfaceDescription
                {
                    float Alpha;
                    float AlphaClipThreshold;
                };

                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    surface.Alpha = 1;
                    surface.AlphaClipThreshold = 0.5;
                    return surface;
                }

                // --------------------------------------------------
                // Structs and Packing

                // Generated Type: Attributes
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };

                // Generated Type: Varyings
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Generated Type: PackedVaryings
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                // Packed Type: Varyings
                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output = (PackedVaryings)0;
                    output.positionCS = input.positionCS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // Unpacked Type: Varyings
                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output = (Varyings)0;
                    output.positionCS = input.positionCS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // --------------------------------------------------
                // Build Graph Inputs

                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);

                    output.ObjectSpaceNormal = input.normalOS;
                    output.ObjectSpaceTangent = input.tangentOS;
                    output.ObjectSpacePosition = input.positionOS;
                    output.uv0 = input.uv0;
                    output.TimeParameters = _TimeParameters.xyz;

                    return output;
                }

                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
                }


                // --------------------------------------------------
                // Main

                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

                ENDHLSL
            }

            Pass
            {
                Name "DepthOnly"
                Tags
                {
                    "LightMode" = "DepthOnly"
                }

                    // Render State
                    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                    Cull Back
                    ZTest LEqual
                    ZWrite On
                    ColorMask 0


                    HLSLPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag

                    // Debug
                    // <None>

                    // --------------------------------------------------
                    // Pass

                    // Pragmas
                    #pragma prefer_hlslcc gles
                    #pragma exclude_renderers d3d11_9x
                    #pragma target 2.0
                    #pragma multi_compile_instancing

                    // Keywords
                    // PassKeywords: <None>
                    // GraphKeywords: <None>

                    // Defines
                    #define _SURFACE_TYPE_TRANSPARENT 1
                    #define _AlphaClip 1
                    #define _NORMALMAP 1
                    #define _NORMAL_DROPOFF_TS 1
                    #define ATTRIBUTES_NEED_NORMAL
                    #define ATTRIBUTES_NEED_TANGENT
                    #define ATTRIBUTES_NEED_TEXCOORD0
                    #define FEATURES_GRAPH_VERTEX
                    #define SHADERPASS_DEPTHONLY

                    // Includes
                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                    #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                    // --------------------------------------------------
                    // Graph

                    // Graph Properties
                    CBUFFER_START(UnityPerMaterial)
                    float Vector1_9E1333AC;
                    float Vector1_E0C9B14B;
                    float Vector1_B9D5A880;
                    float Vector1_FE9E9249;
                    float Vector1_C8C7ABCA;
                    float Vector1_6E50E8E7;
                    float Vector1_1AB3DB99;
                    float4 Color_A19D7DB2;
                    float4 Color_17013314;
                    CBUFFER_END
                    TEXTURE2D(Texture2D_29578A37); SAMPLER(samplerTexture2D_29578A37); float4 Texture2D_29578A37_TexelSize;
                    TEXTURE2D(Texture2D_17583793); SAMPLER(samplerTexture2D_17583793); float4 Texture2D_17583793_TexelSize;
                    SAMPLER(_SampleTexture2DLOD_90AE17E8_Sampler_3_Linear_Repeat);
                    SAMPLER(_SampleTexture2DLOD_CB399CA6_Sampler_3_Linear_Repeat);

                    // Graph Functions

                    void Unity_Multiply_float(float A, float B, out float Out)
                    {
                        Out = A * B;
                    }

                    void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                    {
                        Out = A * B;
                    }

                    void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                    {
                        Out = UV * Tiling + Offset;
                    }

                    void Unity_Add_float(float A, float B, out float Out)
                    {
                        Out = A + B;
                    }

                    void Unity_Saturate_float(float In, out float Out)
                    {
                        Out = saturate(In);
                    }

                    // Graph Vertex
                    struct VertexDescriptionInputs
                    {
                        float3 ObjectSpaceNormal;
                        float3 ObjectSpaceTangent;
                        float3 ObjectSpacePosition;
                        float4 uv0;
                        float3 TimeParameters;
                    };

                    struct VertexDescription
                    {
                        float3 VertexPosition;
                        float3 VertexNormal;
                        float3 VertexTangent;
                    };

                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        float _Split_5C99FBDE_R_1 = IN.ObjectSpacePosition[0];
                        float _Split_5C99FBDE_G_2 = IN.ObjectSpacePosition[1];
                        float _Split_5C99FBDE_B_3 = IN.ObjectSpacePosition[2];
                        float _Split_5C99FBDE_A_4 = 0;
                        float _Property_15BA5427_Out_0 = Vector1_6E50E8E7;
                        float _Multiply_84A22B8D_Out_2;
                        Unity_Multiply_float(_Property_15BA5427_Out_0, 1.5, _Multiply_84A22B8D_Out_2);
                        float2 _Vector2_49268348_Out_0 = float2(0.1, 1);
                        float2 _Multiply_DEC6DFE8_Out_2;
                        Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_49268348_Out_0, _Multiply_DEC6DFE8_Out_2);
                        float _Property_DD8A2173_Out_0 = Vector1_C8C7ABCA;
                        float2 _Multiply_7CA558EB_Out_2;
                        Unity_Multiply_float(_Multiply_DEC6DFE8_Out_2, (_Property_DD8A2173_Out_0.xx), _Multiply_7CA558EB_Out_2);
                        float2 _TilingAndOffset_52970F6_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, (_Multiply_84A22B8D_Out_2.xx), _Multiply_7CA558EB_Out_2, _TilingAndOffset_52970F6_Out_3);
                        float4 _SampleTexture2DLOD_90AE17E8_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_52970F6_Out_3, 0);
                        float _SampleTexture2DLOD_90AE17E8_R_5 = _SampleTexture2DLOD_90AE17E8_RGBA_0.r;
                        float _SampleTexture2DLOD_90AE17E8_G_6 = _SampleTexture2DLOD_90AE17E8_RGBA_0.g;
                        float _SampleTexture2DLOD_90AE17E8_B_7 = _SampleTexture2DLOD_90AE17E8_RGBA_0.b;
                        float _SampleTexture2DLOD_90AE17E8_A_8 = _SampleTexture2DLOD_90AE17E8_RGBA_0.a;
                        float _Property_55FEC0A0_Out_0 = Vector1_6E50E8E7;
                        float2 _Vector2_719B031B_Out_0 = float2(0.3, -1);
                        float2 _Multiply_B9F078AF_Out_2;
                        Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_719B031B_Out_0, _Multiply_B9F078AF_Out_2);
                        float _Property_7A36E455_Out_0 = Vector1_C8C7ABCA;
                        float2 _Multiply_A3BEABAC_Out_2;
                        Unity_Multiply_float(_Multiply_B9F078AF_Out_2, (_Property_7A36E455_Out_0.xx), _Multiply_A3BEABAC_Out_2);
                        float2 _TilingAndOffset_3F74A7B2_Out_3;
                        Unity_TilingAndOffset_float(IN.uv0.xy, (_Property_55FEC0A0_Out_0.xx), _Multiply_A3BEABAC_Out_2, _TilingAndOffset_3F74A7B2_Out_3);
                        float4 _SampleTexture2DLOD_CB399CA6_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_3F74A7B2_Out_3, 0);
                        float _SampleTexture2DLOD_CB399CA6_R_5 = _SampleTexture2DLOD_CB399CA6_RGBA_0.r;
                        float _SampleTexture2DLOD_CB399CA6_G_6 = _SampleTexture2DLOD_CB399CA6_RGBA_0.g;
                        float _SampleTexture2DLOD_CB399CA6_B_7 = _SampleTexture2DLOD_CB399CA6_RGBA_0.b;
                        float _SampleTexture2DLOD_CB399CA6_A_8 = _SampleTexture2DLOD_CB399CA6_RGBA_0.a;
                        float _Add_77F26DA0_Out_2;
                        Unity_Add_float(_SampleTexture2DLOD_90AE17E8_R_5, _SampleTexture2DLOD_CB399CA6_R_5, _Add_77F26DA0_Out_2);
                        float _Saturate_E41E12E3_Out_1;
                        Unity_Saturate_float(_Add_77F26DA0_Out_2, _Saturate_E41E12E3_Out_1);
                        float _Property_6421F4FD_Out_0 = Vector1_1AB3DB99;
                        float _Multiply_553C6DE2_Out_2;
                        Unity_Multiply_float(_Saturate_E41E12E3_Out_1, _Property_6421F4FD_Out_0, _Multiply_553C6DE2_Out_2);
                        float3 _Vector3_74CFAD86_Out_0 = float3(_Split_5C99FBDE_R_1, _Multiply_553C6DE2_Out_2, _Split_5C99FBDE_B_3);
                        description.VertexPosition = _Vector3_74CFAD86_Out_0;
                        description.VertexNormal = IN.ObjectSpaceNormal;
                        description.VertexTangent = IN.ObjectSpaceTangent;
                        return description;
                    }

                    // Graph Pixel
                    struct SurfaceDescriptionInputs
                    {
                    };

                    struct SurfaceDescription
                    {
                        float Alpha;
                        float AlphaClipThreshold;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        surface.Alpha = 1;
                        surface.AlphaClipThreshold = 0.5;
                        return surface;
                    }

                    // --------------------------------------------------
                    // Structs and Packing

                    // Generated Type: Attributes
                    struct Attributes
                    {
                        float3 positionOS : POSITION;
                        float3 normalOS : NORMAL;
                        float4 tangentOS : TANGENT;
                        float4 uv0 : TEXCOORD0;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : INSTANCEID_SEMANTIC;
                        #endif
                    };

                    // Generated Type: Varyings
                    struct Varyings
                    {
                        float4 positionCS : SV_POSITION;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Generated Type: PackedVaryings
                    struct PackedVaryings
                    {
                        float4 positionCS : SV_POSITION;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    // Packed Type: Varyings
                    PackedVaryings PackVaryings(Varyings input)
                    {
                        PackedVaryings output = (PackedVaryings)0;
                        output.positionCS = input.positionCS;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // Unpacked Type: Varyings
                    Varyings UnpackVaryings(PackedVaryings input)
                    {
                        Varyings output = (Varyings)0;
                        output.positionCS = input.positionCS;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // --------------------------------------------------
                    // Build Graph Inputs

                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                    {
                        VertexDescriptionInputs output;
                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                        output.ObjectSpaceNormal = input.normalOS;
                        output.ObjectSpaceTangent = input.tangentOS;
                        output.ObjectSpacePosition = input.positionOS;
                        output.uv0 = input.uv0;
                        output.TimeParameters = _TimeParameters.xyz;

                        return output;
                    }

                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif
                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                        return output;
                    }


                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

                    ENDHLSL
                }

                Pass
                {
                    Name "Meta"
                    Tags
                    {
                        "LightMode" = "Meta"
                    }

                        // Render State
                        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                        Cull Back
                        ZTest LEqual
                        ZWrite On
                        // ColorMask: <None>


                        HLSLPROGRAM
                        #pragma vertex vert
                        #pragma fragment frag

                        // Debug
                        // <None>

                        // --------------------------------------------------
                        // Pass

                        // Pragmas
                        #pragma prefer_hlslcc gles
                        #pragma exclude_renderers d3d11_9x
                        #pragma target 2.0

                        // Keywords
                        #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                        // GraphKeywords: <None>

                        // Defines
                        #define _SURFACE_TYPE_TRANSPARENT 1
                        #define _AlphaClip 1
                        #define _NORMALMAP 1
                        #define _NORMAL_DROPOFF_TS 1
                        #define ATTRIBUTES_NEED_NORMAL
                        #define ATTRIBUTES_NEED_TANGENT
                        #define ATTRIBUTES_NEED_TEXCOORD0
                        #define ATTRIBUTES_NEED_TEXCOORD1
                        #define ATTRIBUTES_NEED_TEXCOORD2
                        #define VARYINGS_NEED_POSITION_WS 
                        #define VARYINGS_NEED_TEXCOORD0
                        #define FEATURES_GRAPH_VERTEX
                        #define SHADERPASS_META
                        #define REQUIRE_DEPTH_TEXTURE
                        #define REQUIRE_OPAQUE_TEXTURE

                        // Includes
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
                        #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                        // --------------------------------------------------
                        // Graph

                        // Graph Properties
                        CBUFFER_START(UnityPerMaterial)
                        float Vector1_9E1333AC;
                        float Vector1_E0C9B14B;
                        float Vector1_B9D5A880;
                        float Vector1_FE9E9249;
                        float Vector1_C8C7ABCA;
                        float Vector1_6E50E8E7;
                        float Vector1_1AB3DB99;
                        float4 Color_A19D7DB2;
                        float4 Color_17013314;
                        CBUFFER_END
                        TEXTURE2D(Texture2D_29578A37); SAMPLER(samplerTexture2D_29578A37); float4 Texture2D_29578A37_TexelSize;
                        TEXTURE2D(Texture2D_17583793); SAMPLER(samplerTexture2D_17583793); float4 Texture2D_17583793_TexelSize;
                        SAMPLER(_SampleTexture2DLOD_90AE17E8_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2DLOD_CB399CA6_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_AD662BE1_Sampler_3_Linear_Repeat);

                        // Graph Functions

                        void Unity_Multiply_float(float A, float B, out float Out)
                        {
                            Out = A * B;
                        }

                        void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                        {
                            Out = A * B;
                        }

                        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                        {
                            Out = UV * Tiling + Offset;
                        }

                        void Unity_Add_float(float A, float B, out float Out)
                        {
                            Out = A + B;
                        }

                        void Unity_Saturate_float(float In, out float Out)
                        {
                            Out = saturate(In);
                        }

                        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
                        {
                            Out = lerp(A, B, T);
                        }

                        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
                        {
                            RGBA = float4(R, G, B, A);
                            RGB = float3(R, G, B);
                            RG = float2(R, G);
                        }

                        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                        {
                            Out = A + B;
                        }

                        void Unity_SceneColor_float(float4 UV, out float3 Out)
                        {
                            Out = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV.xy);
                        }

                        void Unity_SceneDepth_Linear01_float(float4 UV, out float Out)
                        {
                            Out = Linear01Depth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
                        }

                        void Unity_Subtract_float(float A, float B, out float Out)
                        {
                            Out = A - B;
                        }

                        void Unity_OneMinus_float(float In, out float Out)
                        {
                            Out = 1 - In;
                        }

                        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
                        {
                            Out = lerp(A, B, T);
                        }

                        void Unity_Saturate_float3(float3 In, out float3 Out)
                        {
                            Out = saturate(In);
                        }

                        // Graph Vertex
                        struct VertexDescriptionInputs
                        {
                            float3 ObjectSpaceNormal;
                            float3 ObjectSpaceTangent;
                            float3 ObjectSpacePosition;
                            float4 uv0;
                            float3 TimeParameters;
                        };

                        struct VertexDescription
                        {
                            float3 VertexPosition;
                            float3 VertexNormal;
                            float3 VertexTangent;
                        };

                        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                        {
                            VertexDescription description = (VertexDescription)0;
                            float _Split_5C99FBDE_R_1 = IN.ObjectSpacePosition[0];
                            float _Split_5C99FBDE_G_2 = IN.ObjectSpacePosition[1];
                            float _Split_5C99FBDE_B_3 = IN.ObjectSpacePosition[2];
                            float _Split_5C99FBDE_A_4 = 0;
                            float _Property_15BA5427_Out_0 = Vector1_6E50E8E7;
                            float _Multiply_84A22B8D_Out_2;
                            Unity_Multiply_float(_Property_15BA5427_Out_0, 1.5, _Multiply_84A22B8D_Out_2);
                            float2 _Vector2_49268348_Out_0 = float2(0.1, 1);
                            float2 _Multiply_DEC6DFE8_Out_2;
                            Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_49268348_Out_0, _Multiply_DEC6DFE8_Out_2);
                            float _Property_DD8A2173_Out_0 = Vector1_C8C7ABCA;
                            float2 _Multiply_7CA558EB_Out_2;
                            Unity_Multiply_float(_Multiply_DEC6DFE8_Out_2, (_Property_DD8A2173_Out_0.xx), _Multiply_7CA558EB_Out_2);
                            float2 _TilingAndOffset_52970F6_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, (_Multiply_84A22B8D_Out_2.xx), _Multiply_7CA558EB_Out_2, _TilingAndOffset_52970F6_Out_3);
                            float4 _SampleTexture2DLOD_90AE17E8_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_52970F6_Out_3, 0);
                            float _SampleTexture2DLOD_90AE17E8_R_5 = _SampleTexture2DLOD_90AE17E8_RGBA_0.r;
                            float _SampleTexture2DLOD_90AE17E8_G_6 = _SampleTexture2DLOD_90AE17E8_RGBA_0.g;
                            float _SampleTexture2DLOD_90AE17E8_B_7 = _SampleTexture2DLOD_90AE17E8_RGBA_0.b;
                            float _SampleTexture2DLOD_90AE17E8_A_8 = _SampleTexture2DLOD_90AE17E8_RGBA_0.a;
                            float _Property_55FEC0A0_Out_0 = Vector1_6E50E8E7;
                            float2 _Vector2_719B031B_Out_0 = float2(0.3, -1);
                            float2 _Multiply_B9F078AF_Out_2;
                            Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_719B031B_Out_0, _Multiply_B9F078AF_Out_2);
                            float _Property_7A36E455_Out_0 = Vector1_C8C7ABCA;
                            float2 _Multiply_A3BEABAC_Out_2;
                            Unity_Multiply_float(_Multiply_B9F078AF_Out_2, (_Property_7A36E455_Out_0.xx), _Multiply_A3BEABAC_Out_2);
                            float2 _TilingAndOffset_3F74A7B2_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, (_Property_55FEC0A0_Out_0.xx), _Multiply_A3BEABAC_Out_2, _TilingAndOffset_3F74A7B2_Out_3);
                            float4 _SampleTexture2DLOD_CB399CA6_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_3F74A7B2_Out_3, 0);
                            float _SampleTexture2DLOD_CB399CA6_R_5 = _SampleTexture2DLOD_CB399CA6_RGBA_0.r;
                            float _SampleTexture2DLOD_CB399CA6_G_6 = _SampleTexture2DLOD_CB399CA6_RGBA_0.g;
                            float _SampleTexture2DLOD_CB399CA6_B_7 = _SampleTexture2DLOD_CB399CA6_RGBA_0.b;
                            float _SampleTexture2DLOD_CB399CA6_A_8 = _SampleTexture2DLOD_CB399CA6_RGBA_0.a;
                            float _Add_77F26DA0_Out_2;
                            Unity_Add_float(_SampleTexture2DLOD_90AE17E8_R_5, _SampleTexture2DLOD_CB399CA6_R_5, _Add_77F26DA0_Out_2);
                            float _Saturate_E41E12E3_Out_1;
                            Unity_Saturate_float(_Add_77F26DA0_Out_2, _Saturate_E41E12E3_Out_1);
                            float _Property_6421F4FD_Out_0 = Vector1_1AB3DB99;
                            float _Multiply_553C6DE2_Out_2;
                            Unity_Multiply_float(_Saturate_E41E12E3_Out_1, _Property_6421F4FD_Out_0, _Multiply_553C6DE2_Out_2);
                            float3 _Vector3_74CFAD86_Out_0 = float3(_Split_5C99FBDE_R_1, _Multiply_553C6DE2_Out_2, _Split_5C99FBDE_B_3);
                            description.VertexPosition = _Vector3_74CFAD86_Out_0;
                            description.VertexNormal = IN.ObjectSpaceNormal;
                            description.VertexTangent = IN.ObjectSpaceTangent;
                            return description;
                        }

                        // Graph Pixel
                        struct SurfaceDescriptionInputs
                        {
                            float3 WorldSpacePosition;
                            float4 ScreenPosition;
                            float4 uv0;
                            float3 TimeParameters;
                        };

                        struct SurfaceDescription
                        {
                            float3 Albedo;
                            float3 Emission;
                            float Alpha;
                            float AlphaClipThreshold;
                        };

                        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                        {
                            SurfaceDescription surface = (SurfaceDescription)0;
                            float4 _Property_A04E65CB_Out_0 = Color_17013314;
                            float4 _Property_532E50A6_Out_0 = Color_A19D7DB2;
                            float _Property_15BA5427_Out_0 = Vector1_6E50E8E7;
                            float _Multiply_84A22B8D_Out_2;
                            Unity_Multiply_float(_Property_15BA5427_Out_0, 1.5, _Multiply_84A22B8D_Out_2);
                            float2 _Vector2_49268348_Out_0 = float2(0.1, 1);
                            float2 _Multiply_DEC6DFE8_Out_2;
                            Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_49268348_Out_0, _Multiply_DEC6DFE8_Out_2);
                            float _Property_DD8A2173_Out_0 = Vector1_C8C7ABCA;
                            float2 _Multiply_7CA558EB_Out_2;
                            Unity_Multiply_float(_Multiply_DEC6DFE8_Out_2, (_Property_DD8A2173_Out_0.xx), _Multiply_7CA558EB_Out_2);
                            float2 _TilingAndOffset_52970F6_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, (_Multiply_84A22B8D_Out_2.xx), _Multiply_7CA558EB_Out_2, _TilingAndOffset_52970F6_Out_3);
                            float4 _SampleTexture2DLOD_90AE17E8_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_52970F6_Out_3, 0);
                            float _SampleTexture2DLOD_90AE17E8_R_5 = _SampleTexture2DLOD_90AE17E8_RGBA_0.r;
                            float _SampleTexture2DLOD_90AE17E8_G_6 = _SampleTexture2DLOD_90AE17E8_RGBA_0.g;
                            float _SampleTexture2DLOD_90AE17E8_B_7 = _SampleTexture2DLOD_90AE17E8_RGBA_0.b;
                            float _SampleTexture2DLOD_90AE17E8_A_8 = _SampleTexture2DLOD_90AE17E8_RGBA_0.a;
                            float _Property_55FEC0A0_Out_0 = Vector1_6E50E8E7;
                            float2 _Vector2_719B031B_Out_0 = float2(0.3, -1);
                            float2 _Multiply_B9F078AF_Out_2;
                            Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_719B031B_Out_0, _Multiply_B9F078AF_Out_2);
                            float _Property_7A36E455_Out_0 = Vector1_C8C7ABCA;
                            float2 _Multiply_A3BEABAC_Out_2;
                            Unity_Multiply_float(_Multiply_B9F078AF_Out_2, (_Property_7A36E455_Out_0.xx), _Multiply_A3BEABAC_Out_2);
                            float2 _TilingAndOffset_3F74A7B2_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, (_Property_55FEC0A0_Out_0.xx), _Multiply_A3BEABAC_Out_2, _TilingAndOffset_3F74A7B2_Out_3);
                            float4 _SampleTexture2DLOD_CB399CA6_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_3F74A7B2_Out_3, 0);
                            float _SampleTexture2DLOD_CB399CA6_R_5 = _SampleTexture2DLOD_CB399CA6_RGBA_0.r;
                            float _SampleTexture2DLOD_CB399CA6_G_6 = _SampleTexture2DLOD_CB399CA6_RGBA_0.g;
                            float _SampleTexture2DLOD_CB399CA6_B_7 = _SampleTexture2DLOD_CB399CA6_RGBA_0.b;
                            float _SampleTexture2DLOD_CB399CA6_A_8 = _SampleTexture2DLOD_CB399CA6_RGBA_0.a;
                            float _Add_77F26DA0_Out_2;
                            Unity_Add_float(_SampleTexture2DLOD_90AE17E8_R_5, _SampleTexture2DLOD_CB399CA6_R_5, _Add_77F26DA0_Out_2);
                            float _Saturate_E41E12E3_Out_1;
                            Unity_Saturate_float(_Add_77F26DA0_Out_2, _Saturate_E41E12E3_Out_1);
                            float4 _Lerp_EAAAE123_Out_3;
                            Unity_Lerp_float4(_Property_A04E65CB_Out_0, _Property_532E50A6_Out_0, (_Saturate_E41E12E3_Out_1.xxxx), _Lerp_EAAAE123_Out_3);
                            float4 _ScreenPosition_2A4C382C_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                            float _Split_F5815293_R_1 = _ScreenPosition_2A4C382C_Out_0[0];
                            float _Split_F5815293_G_2 = _ScreenPosition_2A4C382C_Out_0[1];
                            float _Split_F5815293_B_3 = _ScreenPosition_2A4C382C_Out_0[2];
                            float _Split_F5815293_A_4 = _ScreenPosition_2A4C382C_Out_0[3];
                            float4 _Combine_390BEBAA_RGBA_4;
                            float3 _Combine_390BEBAA_RGB_5;
                            float2 _Combine_390BEBAA_RG_6;
                            Unity_Combine_float(_Split_F5815293_R_1, _Split_F5815293_G_2, 0, 0, _Combine_390BEBAA_RGBA_4, _Combine_390BEBAA_RGB_5, _Combine_390BEBAA_RG_6);
                            float4 _SampleTexture2D_AD662BE1_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_29578A37, samplerTexture2D_29578A37, _TilingAndOffset_3F74A7B2_Out_3);
                            _SampleTexture2D_AD662BE1_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_AD662BE1_RGBA_0);
                            float _SampleTexture2D_AD662BE1_R_4 = _SampleTexture2D_AD662BE1_RGBA_0.r;
                            float _SampleTexture2D_AD662BE1_G_5 = _SampleTexture2D_AD662BE1_RGBA_0.g;
                            float _SampleTexture2D_AD662BE1_B_6 = _SampleTexture2D_AD662BE1_RGBA_0.b;
                            float _SampleTexture2D_AD662BE1_A_7 = _SampleTexture2D_AD662BE1_RGBA_0.a;
                            float4 _Combine_D817EB00_RGBA_4;
                            float3 _Combine_D817EB00_RGB_5;
                            float2 _Combine_D817EB00_RG_6;
                            Unity_Combine_float(_SampleTexture2D_AD662BE1_R_4, _SampleTexture2D_AD662BE1_G_5, 0, 0, _Combine_D817EB00_RGBA_4, _Combine_D817EB00_RGB_5, _Combine_D817EB00_RG_6);
                            float _Property_161D095F_Out_0 = Vector1_9E1333AC;
                            float2 _Multiply_E7BAE2BC_Out_2;
                            Unity_Multiply_float(_Combine_D817EB00_RG_6, (_Property_161D095F_Out_0.xx), _Multiply_E7BAE2BC_Out_2);
                            float2 _Add_4BBA4B9E_Out_2;
                            Unity_Add_float2(_Combine_390BEBAA_RG_6, _Multiply_E7BAE2BC_Out_2, _Add_4BBA4B9E_Out_2);
                            float3 _SceneColor_8E29EF8E_Out_1;
                            Unity_SceneColor_float((float4(_Add_4BBA4B9E_Out_2, 0.0, 1.0)), _SceneColor_8E29EF8E_Out_1);
                            float _SceneDepth_85022D96_Out_1;
                            Unity_SceneDepth_Linear01_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_85022D96_Out_1);
                            float _Multiply_B09530DE_Out_2;
                            Unity_Multiply_float(_SceneDepth_85022D96_Out_1, _ProjectionParams.z, _Multiply_B09530DE_Out_2);
                            float4 _ScreenPosition_5BB9DC9_Out_0 = IN.ScreenPosition;
                            float _Split_44E4B6AD_R_1 = _ScreenPosition_5BB9DC9_Out_0[0];
                            float _Split_44E4B6AD_G_2 = _ScreenPosition_5BB9DC9_Out_0[1];
                            float _Split_44E4B6AD_B_3 = _ScreenPosition_5BB9DC9_Out_0[2];
                            float _Split_44E4B6AD_A_4 = _ScreenPosition_5BB9DC9_Out_0[3];
                            float _Property_983ECE44_Out_0 = Vector1_E0C9B14B;
                            float _Multiply_ACECC948_Out_2;
                            Unity_Multiply_float(_Split_44E4B6AD_A_4, _Property_983ECE44_Out_0, _Multiply_ACECC948_Out_2);
                            float _Subtract_C1760102_Out_2;
                            Unity_Subtract_float(_Multiply_B09530DE_Out_2, _Multiply_ACECC948_Out_2, _Subtract_C1760102_Out_2);
                            float _OneMinus_490CA95D_Out_1;
                            Unity_OneMinus_float(_Subtract_C1760102_Out_2, _OneMinus_490CA95D_Out_1);
                            float _Property_97100872_Out_0 = Vector1_B9D5A880;
                            float _Multiply_D97971B7_Out_2;
                            Unity_Multiply_float(_OneMinus_490CA95D_Out_1, _Property_97100872_Out_0, _Multiply_D97971B7_Out_2);
                            float _Saturate_D8E9DE67_Out_1;
                            Unity_Saturate_float(_Multiply_D97971B7_Out_2, _Saturate_D8E9DE67_Out_1);
                            float3 _Lerp_8566F9F4_Out_3;
                            Unity_Lerp_float3((_Lerp_EAAAE123_Out_3.xyz), _SceneColor_8E29EF8E_Out_1, (_Saturate_D8E9DE67_Out_1.xxx), _Lerp_8566F9F4_Out_3);
                            float3 _Saturate_C4759D20_Out_1;
                            Unity_Saturate_float3(_Lerp_8566F9F4_Out_3, _Saturate_C4759D20_Out_1);
                            surface.Albedo = _Saturate_C4759D20_Out_1;
                            surface.Emission = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                            surface.Alpha = 1;
                            surface.AlphaClipThreshold = 0.5;
                            return surface;
                        }

                        // --------------------------------------------------
                        // Structs and Packing

                        // Generated Type: Attributes
                        struct Attributes
                        {
                            float3 positionOS : POSITION;
                            float3 normalOS : NORMAL;
                            float4 tangentOS : TANGENT;
                            float4 uv0 : TEXCOORD0;
                            float4 uv1 : TEXCOORD1;
                            float4 uv2 : TEXCOORD2;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : INSTANCEID_SEMANTIC;
                            #endif
                        };

                        // Generated Type: Varyings
                        struct Varyings
                        {
                            float4 positionCS : SV_POSITION;
                            float3 positionWS;
                            float4 texCoord0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        // Generated Type: PackedVaryings
                        struct PackedVaryings
                        {
                            float4 positionCS : SV_POSITION;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            float3 interp00 : TEXCOORD0;
                            float4 interp01 : TEXCOORD1;
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        // Packed Type: Varyings
                        PackedVaryings PackVaryings(Varyings input)
                        {
                            PackedVaryings output = (PackedVaryings)0;
                            output.positionCS = input.positionCS;
                            output.interp00.xyz = input.positionWS;
                            output.interp01.xyzw = input.texCoord0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        // Unpacked Type: Varyings
                        Varyings UnpackVaryings(PackedVaryings input)
                        {
                            Varyings output = (Varyings)0;
                            output.positionCS = input.positionCS;
                            output.positionWS = input.interp00.xyz;
                            output.texCoord0 = input.interp01.xyzw;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        // --------------------------------------------------
                        // Build Graph Inputs

                        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                        {
                            VertexDescriptionInputs output;
                            ZERO_INITIALIZE(VertexDescriptionInputs, output);

                            output.ObjectSpaceNormal = input.normalOS;
                            output.ObjectSpaceTangent = input.tangentOS;
                            output.ObjectSpacePosition = input.positionOS;
                            output.uv0 = input.uv0;
                            output.TimeParameters = _TimeParameters.xyz;

                            return output;
                        }

                        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                        {
                            SurfaceDescriptionInputs output;
                            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                            output.WorldSpacePosition = input.positionWS;
                            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                            output.uv0 = input.texCoord0;
                            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                        #else
                        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                        #endif
                        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                            return output;
                        }


                        // --------------------------------------------------
                        // Main

                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

                        ENDHLSL
                    }

                    Pass
                    {
                            // Name: <None>
                            Tags
                            {
                                "LightMode" = "Universal2D"
                            }

                            // Render State
                            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                            Cull Back
                            ZTest LEqual
                            ZWrite Off
                            // ColorMask: <None>


                            HLSLPROGRAM
                            #pragma vertex vert
                            #pragma fragment frag

                            // Debug
                            // <None>

                            // --------------------------------------------------
                            // Pass

                            // Pragmas
                            #pragma prefer_hlslcc gles
                            #pragma exclude_renderers d3d11_9x
                            #pragma target 2.0
                            #pragma multi_compile_instancing

                            // Keywords
                            // PassKeywords: <None>
                            // GraphKeywords: <None>

                            // Defines
                            #define _SURFACE_TYPE_TRANSPARENT 1
                            #define _AlphaClip 1
                            #define _NORMALMAP 1
                            #define _NORMAL_DROPOFF_TS 1
                            #define ATTRIBUTES_NEED_NORMAL
                            #define ATTRIBUTES_NEED_TANGENT
                            #define ATTRIBUTES_NEED_TEXCOORD0
                            #define VARYINGS_NEED_POSITION_WS 
                            #define VARYINGS_NEED_TEXCOORD0
                            #define FEATURES_GRAPH_VERTEX
                            #define SHADERPASS_2D
                            #define REQUIRE_DEPTH_TEXTURE
                            #define REQUIRE_OPAQUE_TEXTURE

                            // Includes
                            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                            // --------------------------------------------------
                            // Graph

                            // Graph Properties
                            CBUFFER_START(UnityPerMaterial)
                            float Vector1_9E1333AC;
                            float Vector1_E0C9B14B;
                            float Vector1_B9D5A880;
                            float Vector1_FE9E9249;
                            float Vector1_C8C7ABCA;
                            float Vector1_6E50E8E7;
                            float Vector1_1AB3DB99;
                            float4 Color_A19D7DB2;
                            float4 Color_17013314;
                            CBUFFER_END
                            TEXTURE2D(Texture2D_29578A37); SAMPLER(samplerTexture2D_29578A37); float4 Texture2D_29578A37_TexelSize;
                            TEXTURE2D(Texture2D_17583793); SAMPLER(samplerTexture2D_17583793); float4 Texture2D_17583793_TexelSize;
                            SAMPLER(_SampleTexture2DLOD_90AE17E8_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2DLOD_CB399CA6_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_AD662BE1_Sampler_3_Linear_Repeat);

                            // Graph Functions

                            void Unity_Multiply_float(float A, float B, out float Out)
                            {
                                Out = A * B;
                            }

                            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
                            {
                                Out = A * B;
                            }

                            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                            {
                                Out = UV * Tiling + Offset;
                            }

                            void Unity_Add_float(float A, float B, out float Out)
                            {
                                Out = A + B;
                            }

                            void Unity_Saturate_float(float In, out float Out)
                            {
                                Out = saturate(In);
                            }

                            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
                            {
                                Out = lerp(A, B, T);
                            }

                            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
                            {
                                RGBA = float4(R, G, B, A);
                                RGB = float3(R, G, B);
                                RG = float2(R, G);
                            }

                            void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                            {
                                Out = A + B;
                            }

                            void Unity_SceneColor_float(float4 UV, out float3 Out)
                            {
                                Out = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV.xy);
                            }

                            void Unity_SceneDepth_Linear01_float(float4 UV, out float Out)
                            {
                                Out = Linear01Depth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
                            }

                            void Unity_Subtract_float(float A, float B, out float Out)
                            {
                                Out = A - B;
                            }

                            void Unity_OneMinus_float(float In, out float Out)
                            {
                                Out = 1 - In;
                            }

                            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
                            {
                                Out = lerp(A, B, T);
                            }

                            void Unity_Saturate_float3(float3 In, out float3 Out)
                            {
                                Out = saturate(In);
                            }

                            // Graph Vertex
                            struct VertexDescriptionInputs
                            {
                                float3 ObjectSpaceNormal;
                                float3 ObjectSpaceTangent;
                                float3 ObjectSpacePosition;
                                float4 uv0;
                                float3 TimeParameters;
                            };

                            struct VertexDescription
                            {
                                float3 VertexPosition;
                                float3 VertexNormal;
                                float3 VertexTangent;
                            };

                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                            {
                                VertexDescription description = (VertexDescription)0;
                                float _Split_5C99FBDE_R_1 = IN.ObjectSpacePosition[0];
                                float _Split_5C99FBDE_G_2 = IN.ObjectSpacePosition[1];
                                float _Split_5C99FBDE_B_3 = IN.ObjectSpacePosition[2];
                                float _Split_5C99FBDE_A_4 = 0;
                                float _Property_15BA5427_Out_0 = Vector1_6E50E8E7;
                                float _Multiply_84A22B8D_Out_2;
                                Unity_Multiply_float(_Property_15BA5427_Out_0, 1.5, _Multiply_84A22B8D_Out_2);
                                float2 _Vector2_49268348_Out_0 = float2(0.1, 1);
                                float2 _Multiply_DEC6DFE8_Out_2;
                                Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_49268348_Out_0, _Multiply_DEC6DFE8_Out_2);
                                float _Property_DD8A2173_Out_0 = Vector1_C8C7ABCA;
                                float2 _Multiply_7CA558EB_Out_2;
                                Unity_Multiply_float(_Multiply_DEC6DFE8_Out_2, (_Property_DD8A2173_Out_0.xx), _Multiply_7CA558EB_Out_2);
                                float2 _TilingAndOffset_52970F6_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, (_Multiply_84A22B8D_Out_2.xx), _Multiply_7CA558EB_Out_2, _TilingAndOffset_52970F6_Out_3);
                                float4 _SampleTexture2DLOD_90AE17E8_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_52970F6_Out_3, 0);
                                float _SampleTexture2DLOD_90AE17E8_R_5 = _SampleTexture2DLOD_90AE17E8_RGBA_0.r;
                                float _SampleTexture2DLOD_90AE17E8_G_6 = _SampleTexture2DLOD_90AE17E8_RGBA_0.g;
                                float _SampleTexture2DLOD_90AE17E8_B_7 = _SampleTexture2DLOD_90AE17E8_RGBA_0.b;
                                float _SampleTexture2DLOD_90AE17E8_A_8 = _SampleTexture2DLOD_90AE17E8_RGBA_0.a;
                                float _Property_55FEC0A0_Out_0 = Vector1_6E50E8E7;
                                float2 _Vector2_719B031B_Out_0 = float2(0.3, -1);
                                float2 _Multiply_B9F078AF_Out_2;
                                Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_719B031B_Out_0, _Multiply_B9F078AF_Out_2);
                                float _Property_7A36E455_Out_0 = Vector1_C8C7ABCA;
                                float2 _Multiply_A3BEABAC_Out_2;
                                Unity_Multiply_float(_Multiply_B9F078AF_Out_2, (_Property_7A36E455_Out_0.xx), _Multiply_A3BEABAC_Out_2);
                                float2 _TilingAndOffset_3F74A7B2_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, (_Property_55FEC0A0_Out_0.xx), _Multiply_A3BEABAC_Out_2, _TilingAndOffset_3F74A7B2_Out_3);
                                float4 _SampleTexture2DLOD_CB399CA6_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_3F74A7B2_Out_3, 0);
                                float _SampleTexture2DLOD_CB399CA6_R_5 = _SampleTexture2DLOD_CB399CA6_RGBA_0.r;
                                float _SampleTexture2DLOD_CB399CA6_G_6 = _SampleTexture2DLOD_CB399CA6_RGBA_0.g;
                                float _SampleTexture2DLOD_CB399CA6_B_7 = _SampleTexture2DLOD_CB399CA6_RGBA_0.b;
                                float _SampleTexture2DLOD_CB399CA6_A_8 = _SampleTexture2DLOD_CB399CA6_RGBA_0.a;
                                float _Add_77F26DA0_Out_2;
                                Unity_Add_float(_SampleTexture2DLOD_90AE17E8_R_5, _SampleTexture2DLOD_CB399CA6_R_5, _Add_77F26DA0_Out_2);
                                float _Saturate_E41E12E3_Out_1;
                                Unity_Saturate_float(_Add_77F26DA0_Out_2, _Saturate_E41E12E3_Out_1);
                                float _Property_6421F4FD_Out_0 = Vector1_1AB3DB99;
                                float _Multiply_553C6DE2_Out_2;
                                Unity_Multiply_float(_Saturate_E41E12E3_Out_1, _Property_6421F4FD_Out_0, _Multiply_553C6DE2_Out_2);
                                float3 _Vector3_74CFAD86_Out_0 = float3(_Split_5C99FBDE_R_1, _Multiply_553C6DE2_Out_2, _Split_5C99FBDE_B_3);
                                description.VertexPosition = _Vector3_74CFAD86_Out_0;
                                description.VertexNormal = IN.ObjectSpaceNormal;
                                description.VertexTangent = IN.ObjectSpaceTangent;
                                return description;
                            }

                            // Graph Pixel
                            struct SurfaceDescriptionInputs
                            {
                                float3 WorldSpacePosition;
                                float4 ScreenPosition;
                                float4 uv0;
                                float3 TimeParameters;
                            };

                            struct SurfaceDescription
                            {
                                float3 Albedo;
                                float Alpha;
                                float AlphaClipThreshold;
                            };

                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                            {
                                SurfaceDescription surface = (SurfaceDescription)0;
                                float4 _Property_A04E65CB_Out_0 = Color_17013314;
                                float4 _Property_532E50A6_Out_0 = Color_A19D7DB2;
                                float _Property_15BA5427_Out_0 = Vector1_6E50E8E7;
                                float _Multiply_84A22B8D_Out_2;
                                Unity_Multiply_float(_Property_15BA5427_Out_0, 1.5, _Multiply_84A22B8D_Out_2);
                                float2 _Vector2_49268348_Out_0 = float2(0.1, 1);
                                float2 _Multiply_DEC6DFE8_Out_2;
                                Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_49268348_Out_0, _Multiply_DEC6DFE8_Out_2);
                                float _Property_DD8A2173_Out_0 = Vector1_C8C7ABCA;
                                float2 _Multiply_7CA558EB_Out_2;
                                Unity_Multiply_float(_Multiply_DEC6DFE8_Out_2, (_Property_DD8A2173_Out_0.xx), _Multiply_7CA558EB_Out_2);
                                float2 _TilingAndOffset_52970F6_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, (_Multiply_84A22B8D_Out_2.xx), _Multiply_7CA558EB_Out_2, _TilingAndOffset_52970F6_Out_3);
                                float4 _SampleTexture2DLOD_90AE17E8_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_52970F6_Out_3, 0);
                                float _SampleTexture2DLOD_90AE17E8_R_5 = _SampleTexture2DLOD_90AE17E8_RGBA_0.r;
                                float _SampleTexture2DLOD_90AE17E8_G_6 = _SampleTexture2DLOD_90AE17E8_RGBA_0.g;
                                float _SampleTexture2DLOD_90AE17E8_B_7 = _SampleTexture2DLOD_90AE17E8_RGBA_0.b;
                                float _SampleTexture2DLOD_90AE17E8_A_8 = _SampleTexture2DLOD_90AE17E8_RGBA_0.a;
                                float _Property_55FEC0A0_Out_0 = Vector1_6E50E8E7;
                                float2 _Vector2_719B031B_Out_0 = float2(0.3, -1);
                                float2 _Multiply_B9F078AF_Out_2;
                                Unity_Multiply_float((IN.TimeParameters.x.xx), _Vector2_719B031B_Out_0, _Multiply_B9F078AF_Out_2);
                                float _Property_7A36E455_Out_0 = Vector1_C8C7ABCA;
                                float2 _Multiply_A3BEABAC_Out_2;
                                Unity_Multiply_float(_Multiply_B9F078AF_Out_2, (_Property_7A36E455_Out_0.xx), _Multiply_A3BEABAC_Out_2);
                                float2 _TilingAndOffset_3F74A7B2_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, (_Property_55FEC0A0_Out_0.xx), _Multiply_A3BEABAC_Out_2, _TilingAndOffset_3F74A7B2_Out_3);
                                float4 _SampleTexture2DLOD_CB399CA6_RGBA_0 = SAMPLE_TEXTURE2D_LOD(Texture2D_17583793, samplerTexture2D_17583793, _TilingAndOffset_3F74A7B2_Out_3, 0);
                                float _SampleTexture2DLOD_CB399CA6_R_5 = _SampleTexture2DLOD_CB399CA6_RGBA_0.r;
                                float _SampleTexture2DLOD_CB399CA6_G_6 = _SampleTexture2DLOD_CB399CA6_RGBA_0.g;
                                float _SampleTexture2DLOD_CB399CA6_B_7 = _SampleTexture2DLOD_CB399CA6_RGBA_0.b;
                                float _SampleTexture2DLOD_CB399CA6_A_8 = _SampleTexture2DLOD_CB399CA6_RGBA_0.a;
                                float _Add_77F26DA0_Out_2;
                                Unity_Add_float(_SampleTexture2DLOD_90AE17E8_R_5, _SampleTexture2DLOD_CB399CA6_R_5, _Add_77F26DA0_Out_2);
                                float _Saturate_E41E12E3_Out_1;
                                Unity_Saturate_float(_Add_77F26DA0_Out_2, _Saturate_E41E12E3_Out_1);
                                float4 _Lerp_EAAAE123_Out_3;
                                Unity_Lerp_float4(_Property_A04E65CB_Out_0, _Property_532E50A6_Out_0, (_Saturate_E41E12E3_Out_1.xxxx), _Lerp_EAAAE123_Out_3);
                                float4 _ScreenPosition_2A4C382C_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
                                float _Split_F5815293_R_1 = _ScreenPosition_2A4C382C_Out_0[0];
                                float _Split_F5815293_G_2 = _ScreenPosition_2A4C382C_Out_0[1];
                                float _Split_F5815293_B_3 = _ScreenPosition_2A4C382C_Out_0[2];
                                float _Split_F5815293_A_4 = _ScreenPosition_2A4C382C_Out_0[3];
                                float4 _Combine_390BEBAA_RGBA_4;
                                float3 _Combine_390BEBAA_RGB_5;
                                float2 _Combine_390BEBAA_RG_6;
                                Unity_Combine_float(_Split_F5815293_R_1, _Split_F5815293_G_2, 0, 0, _Combine_390BEBAA_RGBA_4, _Combine_390BEBAA_RGB_5, _Combine_390BEBAA_RG_6);
                                float4 _SampleTexture2D_AD662BE1_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_29578A37, samplerTexture2D_29578A37, _TilingAndOffset_3F74A7B2_Out_3);
                                _SampleTexture2D_AD662BE1_RGBA_0.rgb = UnpackNormal(_SampleTexture2D_AD662BE1_RGBA_0);
                                float _SampleTexture2D_AD662BE1_R_4 = _SampleTexture2D_AD662BE1_RGBA_0.r;
                                float _SampleTexture2D_AD662BE1_G_5 = _SampleTexture2D_AD662BE1_RGBA_0.g;
                                float _SampleTexture2D_AD662BE1_B_6 = _SampleTexture2D_AD662BE1_RGBA_0.b;
                                float _SampleTexture2D_AD662BE1_A_7 = _SampleTexture2D_AD662BE1_RGBA_0.a;
                                float4 _Combine_D817EB00_RGBA_4;
                                float3 _Combine_D817EB00_RGB_5;
                                float2 _Combine_D817EB00_RG_6;
                                Unity_Combine_float(_SampleTexture2D_AD662BE1_R_4, _SampleTexture2D_AD662BE1_G_5, 0, 0, _Combine_D817EB00_RGBA_4, _Combine_D817EB00_RGB_5, _Combine_D817EB00_RG_6);
                                float _Property_161D095F_Out_0 = Vector1_9E1333AC;
                                float2 _Multiply_E7BAE2BC_Out_2;
                                Unity_Multiply_float(_Combine_D817EB00_RG_6, (_Property_161D095F_Out_0.xx), _Multiply_E7BAE2BC_Out_2);
                                float2 _Add_4BBA4B9E_Out_2;
                                Unity_Add_float2(_Combine_390BEBAA_RG_6, _Multiply_E7BAE2BC_Out_2, _Add_4BBA4B9E_Out_2);
                                float3 _SceneColor_8E29EF8E_Out_1;
                                Unity_SceneColor_float((float4(_Add_4BBA4B9E_Out_2, 0.0, 1.0)), _SceneColor_8E29EF8E_Out_1);
                                float _SceneDepth_85022D96_Out_1;
                                Unity_SceneDepth_Linear01_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_85022D96_Out_1);
                                float _Multiply_B09530DE_Out_2;
                                Unity_Multiply_float(_SceneDepth_85022D96_Out_1, _ProjectionParams.z, _Multiply_B09530DE_Out_2);
                                float4 _ScreenPosition_5BB9DC9_Out_0 = IN.ScreenPosition;
                                float _Split_44E4B6AD_R_1 = _ScreenPosition_5BB9DC9_Out_0[0];
                                float _Split_44E4B6AD_G_2 = _ScreenPosition_5BB9DC9_Out_0[1];
                                float _Split_44E4B6AD_B_3 = _ScreenPosition_5BB9DC9_Out_0[2];
                                float _Split_44E4B6AD_A_4 = _ScreenPosition_5BB9DC9_Out_0[3];
                                float _Property_983ECE44_Out_0 = Vector1_E0C9B14B;
                                float _Multiply_ACECC948_Out_2;
                                Unity_Multiply_float(_Split_44E4B6AD_A_4, _Property_983ECE44_Out_0, _Multiply_ACECC948_Out_2);
                                float _Subtract_C1760102_Out_2;
                                Unity_Subtract_float(_Multiply_B09530DE_Out_2, _Multiply_ACECC948_Out_2, _Subtract_C1760102_Out_2);
                                float _OneMinus_490CA95D_Out_1;
                                Unity_OneMinus_float(_Subtract_C1760102_Out_2, _OneMinus_490CA95D_Out_1);
                                float _Property_97100872_Out_0 = Vector1_B9D5A880;
                                float _Multiply_D97971B7_Out_2;
                                Unity_Multiply_float(_OneMinus_490CA95D_Out_1, _Property_97100872_Out_0, _Multiply_D97971B7_Out_2);
                                float _Saturate_D8E9DE67_Out_1;
                                Unity_Saturate_float(_Multiply_D97971B7_Out_2, _Saturate_D8E9DE67_Out_1);
                                float3 _Lerp_8566F9F4_Out_3;
                                Unity_Lerp_float3((_Lerp_EAAAE123_Out_3.xyz), _SceneColor_8E29EF8E_Out_1, (_Saturate_D8E9DE67_Out_1.xxx), _Lerp_8566F9F4_Out_3);
                                float3 _Saturate_C4759D20_Out_1;
                                Unity_Saturate_float3(_Lerp_8566F9F4_Out_3, _Saturate_C4759D20_Out_1);
                                surface.Albedo = _Saturate_C4759D20_Out_1;
                                surface.Alpha = 1;
                                surface.AlphaClipThreshold = 0.5;
                                return surface;
                            }

                            // --------------------------------------------------
                            // Structs and Packing

                            // Generated Type: Attributes
                            struct Attributes
                            {
                                float3 positionOS : POSITION;
                                float3 normalOS : NORMAL;
                                float4 tangentOS : TANGENT;
                                float4 uv0 : TEXCOORD0;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                uint instanceID : INSTANCEID_SEMANTIC;
                                #endif
                            };

                            // Generated Type: Varyings
                            struct Varyings
                            {
                                float4 positionCS : SV_POSITION;
                                float3 positionWS;
                                float4 texCoord0;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                uint instanceID : CUSTOM_INSTANCE_ID;
                                #endif
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                #endif
                            };

                            // Generated Type: PackedVaryings
                            struct PackedVaryings
                            {
                                float4 positionCS : SV_POSITION;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                uint instanceID : CUSTOM_INSTANCE_ID;
                                #endif
                                float3 interp00 : TEXCOORD0;
                                float4 interp01 : TEXCOORD1;
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                #endif
                            };

                            // Packed Type: Varyings
                            PackedVaryings PackVaryings(Varyings input)
                            {
                                PackedVaryings output = (PackedVaryings)0;
                                output.positionCS = input.positionCS;
                                output.interp00.xyz = input.positionWS;
                                output.interp01.xyzw = input.texCoord0;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                output.instanceID = input.instanceID;
                                #endif
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                output.cullFace = input.cullFace;
                                #endif
                                return output;
                            }

                            // Unpacked Type: Varyings
                            Varyings UnpackVaryings(PackedVaryings input)
                            {
                                Varyings output = (Varyings)0;
                                output.positionCS = input.positionCS;
                                output.positionWS = input.interp00.xyz;
                                output.texCoord0 = input.interp01.xyzw;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                output.instanceID = input.instanceID;
                                #endif
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                output.cullFace = input.cullFace;
                                #endif
                                return output;
                            }

                            // --------------------------------------------------
                            // Build Graph Inputs

                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                            {
                                VertexDescriptionInputs output;
                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                output.ObjectSpaceNormal = input.normalOS;
                                output.ObjectSpaceTangent = input.tangentOS;
                                output.ObjectSpacePosition = input.positionOS;
                                output.uv0 = input.uv0;
                                output.TimeParameters = _TimeParameters.xyz;

                                return output;
                            }

                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                            {
                                SurfaceDescriptionInputs output;
                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                output.WorldSpacePosition = input.positionWS;
                                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                output.uv0 = input.texCoord0;
                                output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                            #else
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                            #endif
                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                return output;
                            }


                            // --------------------------------------------------
                            // Main

                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

                            ENDHLSL
                        }

        }
            CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
                                FallBack "Hidden/Shader Graph/FallbackError"
}
