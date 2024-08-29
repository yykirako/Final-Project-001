//This shader was used for my 3D graphics assignments, part of the code from this tutorial:
//https://www.youtube.com/watch?v=kfM-yu0iQBk

Shader "Unlit/toonShader"
{
 Properties
    {
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
        _Brightness("Brightness", Range(0,1)) = 0.5
        _Strength("Strength", Range(0,1)) = 0.5
        _Color("Color", COLOR) = (1,1,1,1)
        _Detail("Detail", Range(0,1)) = 0.25
        _MinBrightness("Min Brightness", Range(0,1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            // compile shader into multiple variants, with and without shadows
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
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
                float4 pos : SV_POSITION;
                half3 worldNormal: NORMAL;
                fixed3 ambient : COLOR1;
                SHADOW_COORDS(1) // put shadows data 

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Brightness;
            float _Strength;
            float4 _Color;
            float _Detail;
            float _MinBrightness;


            float Toon(float3 normal, float3 lightDir) {
                //float NdotL = max(0.0,dot(normalize(normal), normalize(lightDir)));
                float NdotL = max(_MinBrightness,dot(normalize(normal), normalize(lightDir)));
                //float NdotL = max(dot(normal, lightDir), _MinBrightness);

                return floor(NdotL / _Detail);
            }

            //helper functions
            float map(float value, float inMin, float inMax, float outMin, float outMax)
            {
                return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
            }

            //----------------------------------------------


            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.ambient = ShadeSH9(half4(o.worldNormal, 1));
                 // compute shadows data
                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float _mappedBrightness = map(_Brightness, 0.0, 1.0, _MinBrightness, _MinBrightness +0.1);

                col *= Toon(i.worldNormal, _WorldSpaceLightPos0.xyz) * _Strength * _Color;
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = 1;
                if(SHADOW_ATTENUATION(i)==0){
                    shadow = saturate(_Brightness*3);
                }
                col *= shadow;
                col += float4(i.ambient *(5+ _MinBrightness),1);
                return col;
            }
            ENDCG
        }
         //Pass for Casting Shadows 
        Pass 
        {
            Name "CastShadow"
            Tags { "LightMode" = "ShadowCaster" }
    
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"
    
            struct v2f 
            { 
                V2F_SHADOW_CASTER;
            };
    
            v2f vert( appdata_base v )
            {
                v2f o;
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
    
            float4 frag( v2f i ) : COLOR
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}   
