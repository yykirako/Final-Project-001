//This shader is a variant of the toonShader

Shader "Unlit/SkyShader"
{
 Properties
    {
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
        _Brightness("Brightness", Range(0,1)) = 0.1
        _Strength("Strength", Range(0,1)) = 0.3
        _Color("Color", COLOR) = (1,1,1,1)
        _Detail("Detail", Range(0,1)) = 0.3
        _MinBrightness("Min Brightness", Range(0,1)) = 0.5
        _TextureRotationSpeed("Texture Rotation Speed", Range(0,0.1)) = 0.01
        _TextureScalingSpeed("Texture Scaling Speed", Range(0,0.1)) = 0.05
        _TextureDensity("Texture Density", Range(0.5, 100.0)) = 10
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

            //texture properties
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TextureDensity;
            float _TextureOffset;
            float _TextureRotationSpeed;
            float _TextureScalingSpeed;
            //frag properties
            float _Brightness;
            float _Strength;
            float4 _Color;
            float _Detail;
            float _MinBrightness;


            float Toon(float3 normal, float3 lightDir) {
                //float NdotL = max(0.0,dot(normalize(normal), normalize(lightDir)));
                float NdotL = max(dot(normal, lightDir), _MinBrightness);

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

                // Add subtle scaling and movement
                float time = _Time.y; 
                float scaleVariation = 1.0 + sin(time * 0.1) * _TextureScalingSpeed; // Scale oscillates 
                float2 moveOffset = float2(sin(time * 0.1), cos(time * 0.1)) * _TextureRotationSpeed; // Rotate textures

                // Apply density and scale variation to UVs
                o.uv = v.uv * (_TextureDensity* scaleVariation) + moveOffset;

                o.uv = TRANSFORM_TEX(o.uv, _MainTex);
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

            _Brightness *= cos(_Time.y);
                float _mappedBrightness = map(_Brightness, 0.0, 1.0, 0.0, 0.1);

                


                col *= Toon(i.worldNormal, _WorldSpaceLightPos0.xyz) * _Strength * _Color + _mappedBrightness;
                

                /*col += float4(i.ambient,1);*/


                return col;
            }
            ENDCG
        }
         //No shadow casting
    }
}   
