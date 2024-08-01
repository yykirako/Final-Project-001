Shader "Unlit/specularLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Gloss("Gloss", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;

                //world position of the fragment
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float _Gloss;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                //world space normal
                o.normal = UnityObjectToWorldNormal(v.normal);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
                //Normal vector
                float3 N = i.normal;
                //Light direction vector
                float3 L = _WorldSpaceLightPos0.xyz;
                //View vector
                float3 V = normalize(_WorldSpaceCameraPos - i.worldPos);
                //Reflected light vector
                float3 R = reflect(-L, i.normal);

                float3 specLight = saturate(dot(V, R));
                
                //apply glossiness
                specLight = pow(specLight, _Gloss); //specular exponent

                return float4(specLight,1);
            }
            ENDCG
        }
       
    }
    
}
