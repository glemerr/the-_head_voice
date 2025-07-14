Shader "Custom/AutodeskInteractive_RoughnessMultiplier"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _MetallicMap ("Metallic Map", 2D) = "white" {}
        _RoughnessMap ("Roughness Map", 2D) = "white" {}
        _RoughnessMultiplier ("Roughness Multiplier", Range(0,2)) = 1.0
        _BumpMap ("Normal Map", 2D) = "bump" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _MetallicMap;
        sampler2D _RoughnessMap;
        sampler2D _BumpMap;

        fixed4 _Color;
        float _Metallic;
        float _RoughnessMultiplier;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MetallicMap;
            float2 uv_RoughnessMap;
            float2 uv_BumpMap;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = albedo.rgb;

            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

            float metallicTex = tex2D(_MetallicMap, IN.uv_MetallicMap).r;
            o.Metallic = saturate(_Metallic * metallicTex);

            float roughnessTex = tex2D(_RoughnessMap, IN.uv_RoughnessMap).r;
            float smoothness = 1.0 - saturate(roughnessTex * _RoughnessMultiplier);
            o.Smoothness = smoothness;
        }
        ENDCG
    }

    FallBack "Diffuse"
}