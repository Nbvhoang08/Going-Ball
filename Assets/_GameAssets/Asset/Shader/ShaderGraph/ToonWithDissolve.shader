Shader "Custom/ToonDissolveGlow_Mobile"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _ToonRamp ("Toon Ramp", 2D) = "white" {}

        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _HighlightColor ("Highlight Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)

        _SpecularColor ("Specular Color", Color) = (1,1,1,1)
        _Shininess ("Shininess", Range(1,100)) = 20
        [Enum(Regular,0,Cartoon,1)] _SpecularMode ("Specular Mode", Float) = 1

        _DissolveMap("Dissolve Map", 2D) = "white" {}
        _DissolveAmount("Dissolve Amount", Range(0,1)) = 0
        _EdgeWidth("Dissolve Edge Width", Range(0.001, 0.2)) = 0.05
        _GlowColor("Glow Color", Color) = (1, 0.5, 0, 1)
        _GlowIntensity("Glow Intensity", Range(0, 5)) = 2
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            Cull Back
            ZWrite On
            ZTest LEqual
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"

            sampler2D _MainTex, _ToonRamp, _DissolveMap;
            float4 _MainTex_ST;
            float4 _BaseColor, _HighlightColor, _ShadowColor;
            float4 _SpecularColor, _GlowColor;
            float _Shininess, _SpecularMode;
            float _DissolveAmount, _EdgeWidth, _GlowIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float2 dissolveUV : TEXCOORD3;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.dissolveUV = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Dissolve value
                float dissolveVal = tex2D(_DissolveMap, i.dissolveUV).r;
                float edge = smoothstep(_DissolveAmount, _DissolveAmount + _EdgeWidth, dissolveVal);
                if (dissolveVal < _DissolveAmount)
                    clip(-1);

                // Base Lighting
                float3 normal = normalize(i.normal);
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float NdotL = max(0, dot(normal, lightDir));

                float2 rampUV = float2(NdotL, 0.5);
                fixed4 ramp = tex2D(_ToonRamp, rampUV);
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed4 litColor = _BaseColor * ramp * texColor;
                litColor = lerp(_ShadowColor * texColor, litColor, NdotL);

                if (NdotL > 0.95)
                    litColor.rgb = lerp(litColor.rgb, _HighlightColor.rgb * texColor.rgb, saturate((NdotL - 0.95) * 20));

                float3 halfDir = normalize(lightDir + viewDir);
                float specDot = max(0, dot(normal, halfDir));
                float specular = (_SpecularMode < 0.5) ? pow(specDot, _Shininess) : step(0.95, pow(specDot, _Shininess));
                litColor.rgb += _SpecularColor.rgb * specular;

                // Glow Edge
                float glowAlpha = 1.0 - edge;
                fixed3 glow = _GlowColor.rgb * glowAlpha * _GlowIntensity;

                return fixed4(litColor.rgb + glow, 1);
            }
            ENDCG
        }
    }
    FallBack Off
}
