
// いったんうごけばいいんじゃないかでネットから引っ張ってきたやつだったきがする

Shader "AX/UI/Sample/HUD Image"
{
Properties
     {
         [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
         
         _StencilComp ("Stencil Comparison", Float) = 8
         _Stencil ("Stencil ID", Float) = 0
         _StencilOp ("Stencil Operation", Float) = 0
         _StencilWriteMask ("Stencil Write Mask", Float) = 255
         _StencilReadMask ("Stencil Read Mask", Float) = 255
 
         _ColorMask ("Color Mask", Float) = 15
     }
 
     SubShader
     {
         Tags
         { 
             "Queue"="Overlay" 
             "IgnoreProjector"="True" 
             "RenderType"="Transparent" 
             "PreviewType"="Plane"
             "CanUseSpriteAtlas"="True"
         }
         
         Stencil
         {
             Ref [_Stencil]
             Comp [_StencilComp]
             Pass [_StencilOp] 
             ReadMask [_StencilReadMask]
             WriteMask [_StencilWriteMask]
         }
 
         Cull Off
         Lighting Off
         ZWrite Off
         ZTest Off
         Blend SrcAlpha OneMinusSrcAlpha
         ColorMask [_ColorMask]
 
         Pass
         {
         CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #pragma multi_compile_local UNITY_UI_CLIP_RECT
             #include "UnityCG.cginc"
             
             struct appdata_t
             {
                 float4 vertex   : POSITION;
                 float4 color    : COLOR;
                 float2 texcoord : TEXCOORD0;
             };
 
             struct v2f
             {
                 float4 vertex   : SV_POSITION;
                 fixed4 color    : COLOR;
                 half2 texcoord  : TEXCOORD0;
                 float4 mask : TEXCOORD1;
             };
             
             sampler2D _MainTex;
             float4 _ClipRect;
             float _UIMaskSoftnessX;
             float _UIMaskSoftnessY;
             
             v2f vert(appdata_t v)
             {
                 v2f o;
                 o.vertex = UnityObjectToClipPos(v.vertex);
                 o.texcoord = v.texcoord;
 #ifdef UNITY_HALF_TEXEL_OFFSET
                 OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
 #endif
                 o.color = v.color;

                 #ifdef UNITY_UI_CLIP_RECT
                   float2 pixelSize = o.vertex.w;
                   pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));
                   float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                   float2 maskXY = v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw;
                   float2 maskZW = 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy));
                   o.mask = float4(maskXY, maskZW);
                 #else
                   o.mask = 0;
                 #endif
                 return o;
             }
 
 
             fixed4 frag(v2f i) : SV_Target
             {
                 half4 col = tex2D(_MainTex, i.texcoord) * i.color;
                 clip (col.a - 0.01);
                 #ifdef UNITY_UI_CLIP_RECT
                    half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(i.mask.xy)) * i.mask.zw);
                    col.a *= m.x * m.y;
                 #endif
                 return col;
             }
         ENDCG
         }
     }
}
