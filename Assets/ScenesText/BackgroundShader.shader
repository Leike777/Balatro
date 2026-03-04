Shader "Custom/PaintSwirlEffectWithGlow"
{
    Properties
    {
        _Colour1 ("Color 1", Color) = (1,1,1,1)
        _Colour2 ("Color 2", Color) = (1,1,1,1)
        _Colour3 ("Color 3", Color) = (1,1,1,1)
        _Colour4 ("Color 4 (Low Priority)", Color) = (1,1,1,1)
        _TimeParam ("Time", Float) = 0
        _SpinTime ("Spin Time", Float) = 0
        _Contrast ("Contrast", Float) = 1
        _SpinAmount ("Spin Amount", Float) = 0
        _MainTex ("Texture", 2D) = "white" {}
        _Offset ("Center Offset", Vector) = (0,0,0,0)
        _ScaleXY ("Scale XY", Vector) = (1,1,0,0)
        _OutlineIntensity ("Outline Intensity", Range(0,1)) = 0.6
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

            sampler2D _MainTex;
            float4 _Colour1, _Colour2, _Colour3, _Colour4;
            float _TimeParam, _SpinTime, _Contrast, _SpinAmount;
            float4 _MainTex_TexelSize;
            float4 _Offset;
            float4 _ScaleXY;
            float _OutlineIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 screenPos : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = o.vertex.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 screenSize = float2(1.0 / _MainTex_TexelSize.x, 1.0 / _MainTex_TexelSize.y);

                // 计算中心偏移后的屏幕坐标（像素坐标系）
                float2 center = 0.5 * screenSize.xy + _Offset.xy * screenSize.xy;
                float2 centered = i.screenPos.xy - center;

                // xy方向拉伸
                centered.x *= _ScaleXY.x;
                centered.y *= _ScaleXY.y;

                // 转为类似 LÖVE 的 UV 变换和 pixelate
                float pixel_size = length(screenSize.xy) / 700.0;
                float2 uv = (floor(centered * (1.0 / pixel_size)) * pixel_size) / length(screenSize.xy) - float2(0.12, 0.0);
                float uv_len = length(uv);

                float speed = (_SpinTime * 0.5 * 0.2) + 302.2;
                float new_pixel_angle = atan2(uv.y, uv.x) + speed - 0.5 * 20.0 * (1.0 * _SpinAmount * uv_len + (1.0 - 1.0 * _SpinAmount));
                float2 mid = (screenSize.xy / length(screenSize.xy)) / 2.0;
                uv = float2((uv_len * cos(new_pixel_angle) + mid.x), (uv_len * sin(new_pixel_angle) + mid.y)) - mid;

                uv *= 30.0;
                speed = _TimeParam * 2.0;
                float2 uv2 = float2(uv.x + uv.y, uv.x + uv.y);

                for (int j = 0; j < 5; j++) {
                    uv2 += sin(max(uv.x, uv.y)) + uv;
                    uv += 0.5 * float2(cos(5.1123314 + 0.353 * uv2.y + speed * 0.131121), sin(uv2.x - 0.113 * speed));
                    uv -= 1.0 * cos(uv.x + uv.y) - 1.0 * sin(uv.x * 0.711 - uv.y);
                }

                float contrast_mod = (0.25 * _Contrast + 0.5 * _SpinAmount + 1.2);
                float paint_res = min(2.0, max(0.0, length(uv) * (0.035) * contrast_mod));

                // ===== 调整颜色权重 =====
                // 提高颜色3的权重，颜色4仅在极少数区域显示
                float c1p = max(0.0, 1.0 - contrast_mod * abs(1.0 - paint_res));
                float c2p = max(0.0, 1.0 - contrast_mod * abs(paint_res - 0.3));
                float c3p = max(0.0, 1.0 - contrast_mod * abs(paint_res - 0.7));
                float c4p = max(0.0, 0.3 - contrast_mod * abs(paint_res - 0.95)); // 颜色4仅在极边缘显示

                // 归一化，确保总和不超过1
                float total = c1p + c2p + c3p + c4p;
                if (total > 1.0) {
                    c1p /= total;
                    c2p /= total;
                    c3p /= total;
                    c4p /= total;
                }

                // 进一步降低颜色4的强度（使其更不明显）
                c4p *= 0.3;

                float4 ret_col = (0.3 / _Contrast) * _Colour1 + (1.0 - 0.3 / _Contrast) * 
                                (_Colour1 * c1p + _Colour2 * c2p + _Colour3 * c3p + _Colour4 * c4p);

                // ===== 泛白描边效果 =====
                float edge_strength = 0.0;
                float offset = 0.003;

                float len_center = length(uv);
                float len_up = length(uv + float2(0, offset));
                float len_down = length(uv - float2(0, offset));
                float len_left = length(uv - float2(offset, 0));
                float len_right = length(uv + float2(offset, 0));

                float diff = abs(len_center - len_up) + abs(len_center - len_down) + abs(len_center - len_left) + abs(len_center - len_right);
                edge_strength = saturate(diff * 10.0) * _OutlineIntensity;

                float4 white_glow = float4(1, 1, 1, 1) * edge_strength;
                ret_col.rgb = saturate(ret_col.rgb + white_glow.rgb * white_glow.a);

                return ret_col;
            }
            ENDCG
        }
    }
}