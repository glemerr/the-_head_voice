Shader "Custom/StandardDoubleSided"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
        Cull Off
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:clip addshadow

        sampler2D _MainTex;
        fixed4 _Color;
        half _Cutoff;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir; // Dirección hacia la cámara
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            clip(c.a - _Cutoff);

            // Ajuste para iluminar reverso: invertir normales si vista está desde atrás
            if (dot(IN.viewDir, o.Normal) < 0)
            {
                o.Normal = -o.Normal;
            }

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    FallBack "Transparent/Cutout/VertexLit"
}