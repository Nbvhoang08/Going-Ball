Shader "Unlit/Dissolve_UnLit"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _DissolveMap("Dissolve Map", 2D) = "white" {}
        _DissolveAmount("Dissolve Amount", Range(0, 1)) = 0
        _DissolveColor("Dissolve Edge Color", Color) = (1, 1, 1, 1)
        _DissolveEmission("Dissolve Emission", Range(0, 5)) = 1
        _DissolveWidth("Dissolve Width", Range(0, 0.1)) = 0.05
        _Color("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" }
        LOD 100
        Cull Off
        ZWrite On
        Blend Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _DissolveMap;

            float4 _MainTex_ST;
            float4 _DissolveMap_ST;

            float _DissolveAmount;
            float _DissolveWidth;
            float4 _DissolveColor;
            float _DissolveEmission;
            float4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uvDissolve : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvDissolve = TRANSFORM_TEX(v.uv, _DissolveMap);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dissolveValue = tex2D(_DissolveMap, i.uvDissolve).r;
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;

                if (dissolveValue < _DissolveAmount)
                    discard;

                float edge = smoothstep(_DissolveAmount, _DissolveAmount + _DissolveWidth, dissolveValue);
                fixed4 finalColor;
                finalColor.rgb = lerp(_DissolveColor.rgb * _DissolveEmission, texColor.rgb, edge);
                finalColor.a = 1.0; // alpha full vì dùng discard rồi

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack Off
}
