Shader "IndexTextureLinear"
{
    Properties
    {
    }
        SubShader
    {
        Cull Off
        ZTest LEqual
        ZWrite On

        Tags {"AnnotationID"="ID"}

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            float _Id;            
            sampler2D _MainTex;

            #pragma vertex vert
            #pragma fragment frag

            static const float linearArray[256] = {
                0.000, 0.0003338797, 0.0006374067, 0.0009409336, 0.001244461, 0.001547988, 0.001851515, 0.002155042, 0.002458568, 0.002762096, 0.003065622, 0.00337872, 0.003710503, 0.004060552, 0.004429143, 0.004816548, 0.005223031, 0.00564885, 0.006094262, 0.006559516, 0.007044855, 0.007550519, 0.008076747, 0.008623769, 0.009191816, 0.009781109, 0.01039187, 0.01102432, 0.01167867, 0.01235513, 0.01305392, 0.01377523, 0.01451928, 0.01528625, 0.01607635, 0.01688978, 0.01772671, 0.01858736, 0.01947189, 0.02038052, 0.0213134, 0.02227072, 0.02325268, 0.02425943, 0.02529116, 0.02634805, 0.02743026, 0.02853796, 0.02967133, 0.03083053, 0.03201573, 0.03322708, 0.03446477, 0.03572893, 0.03701973, 0.03833734, 0.03968192, 0.0410536, 0.04245254, 0.04387891, 0.04533285, 0.04681451, 0.04832404, 0.04986159, 0.0514273, 0.05302133, 0.05464381, 0.05629488, 0.05797471, 0.05968341, 0.06142114, 0.06318803, 0.06498421, 0.06680983, 0.06866503, 0.07054993, 0.07246467, 0.0744094, 0.07638422, 0.07838929, 0.08042473, 0.08249068, 0.08458725, 0.0867146, 0.08887281, 0.09106204, 0.09328241, 0.09553405, 0.09781707, 0.1001316, 0.1024778, 0.1048557, 0.1072655, 0.1097073, 0.1121813, 0.1146874, 0.117226, 0.119797, 0.1224006, 0.1250369, 0.1277061, 0.1304082, 0.1331433, 0.1359117, 0.1387133, 0.1415483, 0.1444168, 0.147319, 0.1502549, 0.1532246, 0.1562284, 0.1592662, 0.1623381, 0.1654444, 0.168585, 0.1717602, 0.17497, 0.1782145, 0.1814938, 0.184808, 0.1881573, 0.1915417, 0.1949614, 0.1984164, 0.2019069, 0.205433, 0.2089947, 0.2125921, 0.2162255, 0.2198948, 0.2236001, 0.2273417, 0.2311195, 0.2349337, 0.2387843, 0.2426715, 0.2465954, 0.2505561, 0.2545536, 0.258588, 0.2626595, 0.2667682, 0.2709141, 0.2750974, 0.2793181, 0.2835763, 0.2878722, 0.2922058, 0.2965772, 0.3009865, 0.3054338, 0.3099192, 0.3144428, 0.3190047, 0.323605, 0.3282437, 0.332921, 0.3376369, 0.3423916, 0.3471852, 0.3520177, 0.3568891, 0.3617997, 0.3667494, 0.3717384, 0.3767668, 0.3818346, 0.386942, 0.392089, 0.3972757, 0.4025023, 0.4077687, 0.4130751, 0.4184216, 0.4238082, 0.429235, 0.4347022, 0.4402098, 0.4457579, 0.4513465, 0.4569758, 0.4626459, 0.4683568, 0.4741085, 0.4799013, 0.4857352, 0.4916102, 0.4975266, 0.5034842, 0.5094832, 0.5155237, 0.5216057, 0.5277295, 0.5338949, 0.5401022, 0.5463514, 0.5526425, 0.5589757, 0.565351, 0.5717686, 0.5782284, 0.5847306, 0.5912753, 0.5978625, 0.6044923, 0.6111648, 0.61788, 0.6246382, 0.6314392, 0.6382833, 0.6451704, 0.6521006, 0.6590741, 0.666091, 0.6731511, 0.6802549, 0.6874021, 0.694593, 0.7018274, 0.7091057, 0.7164278, 0.7237938, 0.7312038, 0.7386579, 0.7461561, 0.7536985, 0.7612852, 0.7689163, 0.7765918, 0.7843119, 0.7920765, 0.7998857, 0.8077397, 0.8156385, 0.8235822, 0.8315708, 0.8396044, 0.8476832, 0.8558071, 0.8639762, 0.8721907, 0.8804504, 0.8887557, 0.8971066, 0.9055029, 0.9139451, 0.9224327, 0.9309664, 0.9395458, 0.9481713, 0.9568425, 0.9655602, 0.9743237, 0.9831337, 0.9919898, 1.000
            };

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 annoID  : COLOR0;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.annoID.r = _Id;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float4 tex = tex2D (_MainTex, i.uv).a;
                clip(tex.a - 0.5);
                return linearArray[(int)_Id];
            }
            ENDCG
        }
    }
}