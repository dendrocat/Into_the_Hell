Shader "Hidden/NewImageEffectShader"
{
    Properties
    {
        _Brightness ("Float", Float) = 0.5
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _Brightness;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                //2x^{2}-x-4x^{2}a+4xa
                float bright = i.uv.x;
                //col.rgb = (2*bright*bright-bright)*float4(1, 1, 1, 1)+4*col.rgb(bright-bright*bright)
                float3 c=float3(1, 1, 1);
                c = (2*_Brightness*_Brightness-_Brightness)*c+4*col.rgb*(_Brightness-_Brightness*_Brightness);
                col.rgb = c;//1-(1-col.rgb)*1.0;
                return col;
            }
            ENDCG
        }
    }
}
