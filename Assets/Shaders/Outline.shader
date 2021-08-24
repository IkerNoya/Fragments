Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Color", Color) = (0.5, 0.5, 0.5, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth("Outline Width", Range(1.0, 5.0)) = 1
    }
        CGINCLUDE // Lo agrego para añadir codigo de HLSL
        #include "UnityCG.cginc"

        struct appdata // creo estructura del programa
        {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
        };
        struct v2f // creo estructura para el vertex shader
        {
            float4 pos : POSITION;
            float3 normal : NORMAL;
        };

        float _OutlineWidth;
        float4 _OutlineColor;

        v2f vert(appdata v)  // creo vertex shader
        {
            v.vertex.xyz *= _OutlineWidth;

            v2f o;

            o.pos = UnityObjectToClipPos(v.vertex); // transformo las posiciones locales del objeto para que clipeen con la camara?
            return o;
        }

    ENDCG
         
    SubShader
    {
        Tags{"Queue" = "Transparent"}

        Pass // Renderizar outline
        {
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert    // compilo el vertex shader que cree arriba
            #pragma fragment frag //compilo el fragment shader de abajo que devuelve el color del outline

            half4 frag(v2f i) : COLOR
            {
                return _OutlineColor;
            }

            ENDCG

        }

        Pass // Normal Material Render
        {
             ZWrite On

             Material
             {
                 Diffuse[_Color]
                 Ambient[_Color]
             }

             Lighting On

             SetTexture  [_MainTex]
             {
                ConstantColor[_Color]
             }
             SetTexture[_MainTex]
            {
                Combine previous* primary DOUBLE
            }
        }
    }
}
