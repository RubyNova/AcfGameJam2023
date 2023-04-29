// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ACF/GaussianTile"
{

    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        _ScreenResolution("Screen Resolution", Vector) = (1920, 1080, 0)
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
                float2 _ScreenResolution;

                half4 blur(sampler2D tex, float2 uv, float blurAmount) {
                    //get our base color...
                    half4 col = tex2D(tex, uv);
                    half3 rgb = col.rgb;
                    half a = col.a;

                    // todo: this is such a hack
                    a += tex2D(tex, float2(uv.x + 1 * blurAmount, uv.y + 1 * blurAmount)).a * normpdf(float(1), 7);
                    a += tex2D(tex, float2(uv.x - 1 * blurAmount, uv.y - 1 * blurAmount)).a * normpdf(float(-1), 7);
                    a += tex2D(tex, float2(uv.x - 1 * blurAmount, uv.y - 1 * blurAmount)).a * normpdf(float(-1), 7);
                    a += tex2D(tex, float2(uv.x + 1 * blurAmount, uv.y - 1 * blurAmount)).a * normpdf(float(1), 7);
                    a += tex2D(tex, float2(uv.x - 1 * blurAmount, uv.y + 1 * blurAmount)).a * normpdf(float(1), 7);
                    a /= 5;



                    //total width/height of our blur "grid":
                    const int mSize = 11;
                    const int mSizeAlpha = 2;
                    //this gives the number of times we'll iterate our blur on each side 
                    //(up,down,left,right) of our uv coordinate;
                    //NOTE that this needs to be a const or you'll get errors about unrolling for loops
                    const int iter = (mSize - 1) / 2;
                    const int iterAlpha = (mSizeAlpha - 1) / 2;
                    //run loops to do the equivalent of what's written out line by line above
                    //(number of blur iterations can be easily sized up and down this way)
                    for (int i = -iter; i <= iter; ++i) {
                        for (int j = -iter; j <= iter; ++j) {
                            rgb += tex2D(tex, float2(uv.x + i * blurAmount, uv.y + j * blurAmount)).rgb * normpdf(float(i), 7);
                        }
                    }


                    col.rgb = rgb / mSize;

                    //return blurred color
                    return col;
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    half4 result = blur(_MainTex, IN.texcoord, 0.0075);

                    if (result.a == 0.0)
                    {
                        discard;
                    }

                    return result;
                }
            ENDCG
            }
        }
}