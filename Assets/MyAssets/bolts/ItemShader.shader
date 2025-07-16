Shader "Custom/URP/ItemShader"
{
    Properties
    {
        _BaseColor ("Color", Color) = (1,1,1,1)
        _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
        _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
        _BaseMap ("Albedo", 2D) = "white" {}

        _RampThreshold ("Threshold", Range(0.01, 1)) = 0.5
        _RampSmoothing ("Smoothing", Range(0.001, 1)) = 0.5

        _UseSpecular ("Enable Specular", Float) = 0
        _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
        _SpecularRoughnessPBR ("Roughness", Range(0, 1)) = 0.5

        _UseRim ("Enable Rim Lighting", Float) = 0
        _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
        _RimMinVert ("Rim Min", Range(0, 2)) = 0.5
        _RimMaxVert ("Rim Max", Range(0, 2)) = 1

        _TCP2_AMBIENT_RIGHT ("+X (Right)", Color) = (0,0,0,1)
        _TCP2_AMBIENT_LEFT ("-X (Left)", Color) = (0,0,0,1)
        _TCP2_AMBIENT_TOP ("+Y (Top)", Color) = (0,0,0,1)
        _TCP2_AMBIENT_BOTTOM ("-Y (Bottom)", Color) = (0,0,0,1)
        _TCP2_AMBIENT_FRONT ("+Z (Front)", Color) = (0,0,0,1)
        _TCP2_AMBIENT_BACK ("-Z (Back)", Color) = (0,0,0,1)

        _UseNormalMap ("Enable Normal Mapping", Float) = 0
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Scale", Float) = 1

        _UseVerticalFog ("Enable Vertical Fog", Float) = 0
        _VerticalFogThreshold ("Y Threshold", Float) = 0
        _VerticalFogSmoothness ("Smoothness", Float) = 0.5
        _VerticalFogColor ("Vertical Fog Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID

            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float3 worldPos : TEXCOORD3;
                float3 tangentWS : TEXCOORD4;
                float3 binormalWS : TEXCOORD5;
                UNITY_VERTEX_INPUT_INSTANCE_ID

            };
            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            float4 _BaseColor;
            float4 _HColor;
            float4 _SColor;
            float _RampThreshold;
            float _RampSmoothing;

            float _UseSpecular;
            float4 _SpecularColor;
            float _SpecularRoughnessPBR;

            float _UseRim;
            float4 _RimColor;
            float _RimMinVert;
            float _RimMaxVert;

            float4 _TCP2_AMBIENT_RIGHT, _TCP2_AMBIENT_LEFT;
            float4 _TCP2_AMBIENT_TOP, _TCP2_AMBIENT_BOTTOM;
            float4 _TCP2_AMBIENT_FRONT, _TCP2_AMBIENT_BACK;

            float _UseNormalMap;
            sampler2D _BumpMap;
            float _BumpScale;

            float _UseVerticalFog;
            float _VerticalFogThreshold;
            float _VerticalFogSmoothness;
            float4 _VerticalFogColor;


            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);



                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                float3 binormalWS = cross(normalWS, tangentWS) * IN.tangentOS.w;

                //OUT.positionHCS = TransformWorldToHClip(worldPos);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.normalWS = normalWS;
                OUT.viewDirWS = GetWorldSpaceViewDir(worldPos);
                OUT.worldPos = worldPos;
                OUT.tangentWS = tangentWS;
                OUT.binormalWS = binormalWS;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float3 viewDir = normalize(IN.viewDirWS);
                float3 normal = normalize(IN.normalWS);

                UNITY_SETUP_INSTANCE_ID(IN);

                if (_UseNormalMap > 0.5)
                {
                    float3x3 TBN = float3x3(IN.tangentWS, IN.binormalWS, normal);
                    float3 normalTS = UnpackNormal(tex2D(_BumpMap, IN.uv)) * _BumpScale;
                    normal = normalize(mul(normalTS, TBN));
                }

                float3 lightDir = normalize(_MainLightPosition.xyz);
                float ndotl = saturate(dot(normal, lightDir));
                float ramp = smoothstep(_RampThreshold - _RampSmoothing, _RampThreshold + _RampSmoothing, ndotl);
                float3 litColor = lerp(_SColor.rgb, _HColor.rgb, ramp);

                float3 baseCol = tex2D(_BaseMap, IN.uv).rgb * _BaseColor.rgb;

                // Ambient light
                float3 amb = 0;
                float3 n = normal;
                amb += _TCP2_AMBIENT_RIGHT.rgb * saturate(n.x);
                amb += _TCP2_AMBIENT_LEFT.rgb  * saturate(-n.x);
                amb += _TCP2_AMBIENT_TOP.rgb   * saturate(n.y);
                amb += _TCP2_AMBIENT_BOTTOM.rgb* saturate(-n.y);
                amb += _TCP2_AMBIENT_FRONT.rgb * saturate(n.z);
                amb += _TCP2_AMBIENT_BACK.rgb  * saturate(-n.z);

                float3 finalColor = baseCol * litColor + amb;

                // Specular
                if (_UseSpecular > 0.5)
                {
                    float3 halfDir = normalize(viewDir + lightDir);
                    float ndoth = saturate(dot(normal, halfDir));
                    float spec = pow(ndoth, lerp(2, 64, 1 - _SpecularRoughnessPBR));
                    finalColor += _SpecularColor.rgb * spec;
                }

                // Rim Lighting
                if (_UseRim > 0.5)
                {
                    float rim = 1.0 - saturate(dot(normal, viewDir));
                    rim = smoothstep(_RimMinVert, _RimMaxVert, rim);
                    finalColor += _RimColor.rgb * rim * _RimColor.a;
                }

                // Vertical Fog
                if (_UseVerticalFog > 0.5)
                {
                    float fogFactor = saturate((IN.worldPos.y - _VerticalFogThreshold) / _VerticalFogSmoothness);
                    finalColor = lerp(_VerticalFogColor.rgb, finalColor, fogFactor);
                }

                return float4(finalColor, 1);
            }

            ENDHLSL
        }
    }
    //FallBack Off
}
