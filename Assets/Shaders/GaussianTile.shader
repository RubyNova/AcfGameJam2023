// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ACF/GaussianTile"
{

    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        _ScreenResolutionWidth("Screen Res Width", Float) = 1920
        _ScreenResolutionHeight("Screen Res Height", Float) = 1080
        _BlurIntensity("Blur Intensity", Float) = 7.0
    }
        SubShader
        {

            
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }
            

            Cull Off
            Lighting Off
            ZWrite Off
            Fog { Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile DUMMY PIXELSNAP_ON
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
                    fixed4 color : COLOR;
                    half2 texcoord  : TEXCOORD0;
                };

                fixed4 _Color;

                v2f vert(appdata_t IN)
                {
                    v2f OUT;
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    OUT.texcoord = IN.texcoord;
                    OUT.color = IN.color * _Color;
                    #ifdef PIXELSNAP_ON
                    OUT.vertex = UnityPixelSnap(OUT.vertex);
                    #endif

                    return OUT;
                }

                float normpdf(float x, float sigma)
                {
	                return 0.39894*exp(-0.5*x*x/(sigma*sigma))/sigma;
                }

                sampler2D _MainTex;
                float _ScreenResolutionWidth;
                float _ScreenResolutionHeight;
                float _BlurIntensity;

                fixed4 frag(v2f IN) : SV_Target
                {
                    float2 screenRes = (_ScreenResolutionWidth, _ScreenResolutionHeight);
                    float4 c = tex2D(_MainTex, IN.texcoord / screenRes);
                    //declare stuff
                    const int mSize = 31;
                    const int kSize = (mSize - 1) / 2;
                    float kernel[mSize];
                    float4 final_colour = (0.0, 0.0, 0.0, 0.0);

                    //create the 1-D kernel
                    float sigma = _BlurIntensity;
                    float Z = 0.0;
                    for (int j = 0; j <= kSize; ++j)
                    {
                        kernel[kSize + j] = kernel[kSize - j] = normpdf(float(j), sigma);
                    }

                    //get the normalization factor (as the gaussian has been clamped)
                    for (int j = 0; j < mSize; ++j)
                    {
                        Z += kernel[j];
                    }

                    //read out the texels
                    for (int i = -kSize; i <= kSize; ++i)
                    {
                        for (int j = -kSize; j <= kSize; ++j)
                        {
                            float4 test = tex2D(_MainTex, (IN.texcoord + (float2(float(i), float(j)) / screenRes)));
                            final_colour += kernel[kSize + j] * kernel[kSize + i] * test;
                        }
                    }


                    return fixed4(final_colour / (Z * Z));
                }
            ENDCG
            }
        }
}