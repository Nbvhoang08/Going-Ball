Shader "Unlit/GlassGlossy"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (0.1, 0.1, 0.1, 0.6)
        _Glossiness ("Smoothness", Range(0,1)) = 1
        _FresnelColor ("Fresnel Color", Color) = (0.3, 1, 0.3, 1)
        _FresnelPower ("Fresnel Power", Range(1, 5)) = 3
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 300

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back
        Lighting On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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
                float3 worldNormal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Glossiness;
            float4 _FresnelColor;
            float _FresnelPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float NdotV = saturate(dot(normal, i.viewDir));

                // Fresnel (viền highlight)
                float fresnel = pow(1.0 - NdotV, _FresnelPower);
                float4 fresnelColor = _FresnelColor * fresnel;

                // Texture base
                float4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // Glossiness / highlight
                texColor.rgb += fresnelColor.rgb;
                texColor.a = _Color.a;

                return texColor;
            }
            ENDCG
        }
    }
}

