Shader "Unlit/lightWithShadow"
{
    //full light shader!!
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Gloss ("Gloss", Range(0,1)) = 1
        _Color ("Color", Color) = (1,1,1,1)


    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            // compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD2;
                float3 worldPos : TEXCOORD3;
            };
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul( unity_ObjectToWorld, v.vertex );

                half nl = max(0, dot(o.normal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                
                o.ambient = ShadeSH9(half4(o.normal,1));
                // compute shadows data
                TRANSFER_SHADOW(o)
                return o;
            }

            sampler2D _MainTex;
            float _Gloss;
            float4 _Color;


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact

                float3 N = normalize(i.normal);
                float3 L = _WorldSpaceLightPos0.xyz;
                float3 lambert = saturate( dot( N, L ) );
                // float3 diffuseLight = lambert * _LightColor0.xyz;

                // specular lighting
                float3 V = normalize( _WorldSpaceCameraPos - i.worldPos );
                float3 H = normalize(L + V);
                //float3 R = reflect( -L, N ); // uses for Phong
                float3 specularLight = saturate(dot(H, N)) * (lambert > 0); // Blinn-Phong
                float specularExponent = exp2( _Gloss * 11 ) + 2;
                specularLight = pow( specularLight, specularExponent ) * _Gloss; // specular exponent
                specularLight *= _LightColor0.xyz;

                fixed3 lighting = (i.diff+specularLight) * shadow + i.ambient;
                col.rgb *= lighting;
                return col;
            }
            ENDCG
        }

        // shadow casting support
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
