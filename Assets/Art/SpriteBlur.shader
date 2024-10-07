Shader "Custom/DissolveTwoColor"
{
    Properties
    {
        _MainTexture("Main Texture(RGB)", 2D) = "white" { }
        _DissolveTexture("Dissolve Texture(R)", 2D) = "white" { }// 噪声图
        _EdgeFirstColor("EdgeColor1", Color) = (1, 1, 1, 1) // 边缘颜色1
        _EdgeSecondColor("EdgeColor2", Color) = (1, 1, 1, 1) // 边缘颜色2
        _DissolveAmount("DissolveAmount", Range(0, 1)) = 1 // 溶解系数
        _EdgeWidth("EdgeWidth", Range(0, 0.3)) = 0.05 // 边缘宽度
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100
            Cull off

            Pass
            {
                CGPROGRAM

                #pragma vertex vert // 声明顶点着色程序函数名
                #pragma fragment frag // 声明片元着色程序函数

                #include "UnityCG.cginc"

                sampler2D _MainTexture;
                sampler2D _DissolveTexture;
                float _DissolveAmount;
                float _ExtrudeAmount;
                float _EdgeWidth;
                fixed4 _EdgeFirstColor;
                fixed4 _EdgeSecondColor;

                // 应用传给顶点着色程序的数据
                struct appdata
                {
                    float4 vertex: POSITION; // 模型的顶点坐标
                    float2 uv: TEXCOORD; // 模型的纹理坐标
                };

                // 顶点着色程序传递给片元程序的数据
                struct v2f
                {
                    float4 pos: SV_POSITION; // 裁剪空间中的顶点坐标
                    float2 uv: TEXCOORD; // 模型的纹理坐标
                };

                // 顶点着色程序函数
                v2f vert(appdata v)
                {
                    v2f o;
                    //
                    o.pos = UnityObjectToClipPos(v.vertex); // 模型空间顶点坐标转换为裁剪空间下坐标
                    o.uv = v.uv; // 纹理uv坐标
                    return o;
                }

                // 片元着色程序函数
                fixed4 frag(v2f i) : SV_TARGET
                {
                    // 溶解贴图采样
                    fixed4 dissolveColor = tex2D(_DissolveTexture, i.uv);
                // 溶解贴图上的R通道值和目前的溶解基准值相差多少
                float offsetValue = dissolveColor.r - _DissolveAmount;
                // offsetValue < 0 则放弃此片元不绘制
                clip(offsetValue);

                // 计算边缘颜色
                if (offsetValue < _EdgeWidth)
                {
                    offsetValue += (1 - sign(_DissolveAmount)) * _EdgeWidth;
                    float value = saturate(offsetValue / _EdgeWidth);
                    return lerp(_EdgeFirstColor, _EdgeSecondColor, value);
                }

                fixed4 textureColor = tex2D(_MainTexture, i.uv);
                return textureColor;
            }

            ENDCG

        }
        }
}