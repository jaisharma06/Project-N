// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// NextGenSprites (copyright) 2015 Ruben de la Torre, www.studio-delatorre.com
// Version 1.2.0

Shader "NextGenSprites/Standard/Unlit" {
    Properties {
		//Sprite Props
        [PerRendererData]_MainTex ("Sprite", 2D) = "white" {}
        _Color ("Sprite Tint", Color) = (1,1,1,1)

		//Sprite Layers
		_StencilMask("Stencil Mask", 2D) = "white" {}
		_Layer0ScrollingX("Main Sprite X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer0ScrollingY("Main Sprite Y-Axis Scrolling", Range(-1, 1)) = 0
		_Layer1("Layer 1", 2D) = "black" {}
		_Layer1Color("Layer 1 Tint", Color) = (1,1,1,1)
		_Layer1Opacity("Layer 1 Opacity", Range(0, 1)) = 1
		_Layer1ScrollingX("Layer 1 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer1ScrollingY("Layer 1 Y-Axis Scrolling", Range(-1, 1)) = 0
		_Layer2("Layer 2", 2D) = "black" {}
		_Layer2Color("Layer 2 Tint", Color) = (1,1,1,1)
		_Layer2Opacity("Layer 2 Opacity", Range(0, 1)) = 1
		_Layer2ScrollingX("Layer 2 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer2ScrollingY("Layer 2 Y-Axis Scrolling", Range(-1, 1)) = 0
		_Layer3("Layer 3", 2D) = "black" {}
		_Layer3Color("Layer 3 Tint", Color) = (1,1,1,1)
		_Layer3Opacity("Layer 3 Opacity", Range(0, 1)) = 1
		_Layer3ScrollingX("Layer 3 X-Axis Scrolling", Range(-1, 1)) = 0
		_Layer3ScrollingY("Layer 3 Y-Axis Scrolling", Range(-1, 1)) = 0

		//Reflection Props
        _ReflectionTex ("Reflection Texture", 2D) = "white" {}
        _ReflectionMask ("Reflection Mask", 2D) = "white" {}
        _ReflectionStrength ("Reflection Strength", Range(0, 1)) = 0
        _ReflectionBlur ("Reflection Blur", Range(0, 9)) = 0
		_ReflectionScrollingX ("Scrolling Reflection X", Range(0, 5)) = 0.25
        _ReflectionScrollingY ("Scrolling Reflection Y", Range(0, 5)) = 0.25

		//Dissolve Props
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _DissolveBlend ("Dissolve Blending", Range(0, 1)) = 0
        _DissolveBorderWidth ("Dissolve Border width", Range(0, 100)) = 10
        _DissolveGlowColor ("Dissolve Glow color", Color) = (1,1,1,1)
        _DissolveGlowStrength ("Dissolve Glow strength", Range(0, 5)) = 1

        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [MaterialToggle]PixelSnap ("Pixel snap", float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #pragma shader_feature PIXELSNAP_ON
			#pragma shader_feature SPRITE_MULTILAYER_ON
			#pragma shader_feature SPRITE_SCROLLING_ON
			#pragma shader_feature SPRITE_STENCIL_ON
            #pragma shader_feature DOUBLESIDED_ON
            #pragma multi_compile _ REFLECTION_ON
            #pragma multi_compile _ DISSOLVE_ON
            #include "UnityCG.cginc"
			#pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
			uniform sampler2D _Layer1; uniform float4 _Layer1_ST;
			uniform sampler2D _Layer2; uniform float4 _Layer2_ST;
			uniform sampler2D _Layer3; uniform float4 _Layer3_ST;
			uniform sampler2D _StencilMask; uniform float4 _StencilMask_ST;
			uniform half _Layer0ScrollingX;
			uniform half _Layer0ScrollingY;
			uniform half _Layer1Opacity;
			uniform half4 _Layer1Color;
			uniform half _Layer1ScrollingX;
			uniform half _Layer1ScrollingY;
			uniform half _Layer2Opacity;
			uniform half4 _Layer2Color;
			uniform half _Layer2ScrollingX;
			uniform half _Layer2ScrollingY;
			uniform half _Layer3Opacity;
			uniform half4 _Layer3Color;
			uniform half _Layer3ScrollingX;
			uniform half _Layer3ScrollingY;
            uniform half4 _Color;
            uniform sampler2D _ReflectionMask; uniform float4 _ReflectionMask_ST;
            uniform half _ReflectionStrength;
            uniform half _ReflectionBlur;
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform sampler2D _DissolveTex; uniform float4 _DissolveTex_ST;
            uniform half _DissolveBlend;
            uniform half _ReflectionScrollingX;
            uniform half _ReflectionScrollingY;
            uniform half _DissolveBorderWidth;
            uniform half4 _DissolveGlowColor;
            uniform half _DissolveGlowStrength;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD7;
				float2 uv2 : TEXCOORD8;
				float2 uv3 : TEXCOORD9;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;

				//Scroll UV
				#if SPRITE_SCROLLING_ON
					o.uv0 = half2(o.uv0.r + _Time.y * _Layer0ScrollingX, o.uv0.g + _Time.y * _Layer0ScrollingY);
				#endif

				#if SPRITE_MULTILAYER_ON
					o.uv1 = v.texcoord0;
					o.uv2 = v.texcoord0;
					o.uv3 = v.texcoord0;

					o.uv1 = half2(o.uv1.r + _Time.y * _Layer1ScrollingX, o.uv1.g + _Time.y * _Layer1ScrollingY);
					o.uv2 = half2(o.uv2.r + _Time.y * _Layer2ScrollingX, o.uv2.g + _Time.y * _Layer2ScrollingY);
					o.uv3 = half2(o.uv3.r + _Time.y * _Layer3ScrollingX, o.uv3.g + _Time.y * _Layer3ScrollingY);
				#endif

                o.vertexColor = float4(_Color.rgb, _Color.a);

                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
				#if DOUBLESIDED_ON || REFLECTION_ON
					i.normalDir = normalize(i.normalDir);
					half3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
					half3 normalDirection = i.normalDir;
				#endif
					                
				//Flip Normals if double sided
				#if DOUBLESIDED_ON
					half nSign = sign( dot( viewDirection, i.normalDir ) );
					i.normalDir *= nSign;
					normalDirection *= nSign;
				#endif

				//Texture to RGBA
				#if SPRITE_MULTILAYER_ON
					half _time = _Time.w;

					//Layer 0 / Main Sprite
					half4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));

					//Use MainTex as canvas for the additional layers
					half3 canvas = _MainTex_var.rgb * i.vertexColor.rgb;
					
					//Layer 1
					half4 layer1Tex = tex2D(_Layer1, TRANSFORM_TEX(i.uv1, _Layer1));
					layer1Tex.rgb *= _Layer1Color.rgb;
					layer1Tex.rgb = lerp(layer1Tex.rgb, canvas, ((layer1Tex.a - 1) * -1));

					//Apply stencil by the Red channel
					#if SPRITE_STENCIL_ON
						half3 stencil = tex2D(_StencilMask, TRANSFORM_TEX(i.uv0, _StencilMask));
						layer1Tex.rgb = lerp(canvas, layer1Tex.rgb, stencil.r);
					#endif

					canvas = lerp(canvas, layer1Tex.rgb, _Layer1Opacity);

					//Layer 2
					half4 layer2Tex = tex2D(_Layer2, TRANSFORM_TEX(i.uv2, _Layer2));
					layer2Tex.rgb *= _Layer2Color.rgb;
					layer2Tex.rgb = lerp(layer2Tex.rgb, canvas, ((layer2Tex.a - 1) * -1));

					//Apply stencil by the Green channel
					#if SPRITE_STENCIL_ON
						layer2Tex.rgb = lerp(canvas, layer2Tex.rgb, stencil.g);
					#endif

					canvas = lerp(canvas, layer2Tex.rgb, _Layer2Opacity);

					//Layer 3
					half4 layer3Tex = tex2D(_Layer3, TRANSFORM_TEX(i.uv3, _Layer3));
					layer3Tex.rgb *= _Layer3Color.rgb;
					layer3Tex.rgb = lerp(layer3Tex.rgb, canvas, ((layer3Tex.a - 1) * -1));

					//Apply stencil by the Blue channel
					#if SPRITE_STENCIL_ON
						layer3Tex.rgb = lerp(canvas, layer3Tex.rgb, stencil.b);
					#endif

					canvas = lerp(canvas, layer3Tex.rgb, _Layer3Opacity);

					//Result
					half3 emissive = canvas;
				#else
					//Just tint
					half4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));
					half3 emissive = _MainTex_var.rgb * i.vertexColor.rgb;
				#endif

				//Reflection
				#if REFLECTION_ON
					half ScrollDamp = 0.05;
					half4 objPos = mul(unity_ObjectToWorld, half4(0,0,0,1));
					half3 viewReflectDirection = reflect(-viewDirection, normalDirection);
					half2 ScrollUV = (half2((_ReflectionScrollingX * objPos.r * ScrollDamp), (ScrollDamp * objPos.g * _ReflectionScrollingY)) + (viewReflectDirection.rg * 0.5 + 0.5));
					half4 _ReflectionTex_var = tex2Dlod(_ReflectionTex, half4(TRANSFORM_TEX(ScrollUV, _ReflectionTex), 0.0, _ReflectionBlur));
					half4 _ReflectionMask_var = tex2D(_ReflectionMask, TRANSFORM_TEX(i.uv0, _ReflectionMask));
					half3 reflectionStrength = (_ReflectionStrength * _ReflectionMask_var.rgb);
					emissive = (lerp(emissive, _ReflectionTex_var.rgb, reflectionStrength));
				#endif

				//Dissolve blending
				#if DISSOLVE_ON
					half4 _DissolveTex_var = tex2D(_DissolveTex, i.uv0);
					half DissolveRemap = (_DissolveBlend * -1.0 + 1.0);
					half DissolvePow = pow(((_DissolveTex_var.r * DissolveRemap) * 5.0), 35.0);
					half ClipStepA = step(_DissolveBorderWidth, DissolvePow);
					half ClipStepB = step(DissolvePow, _DissolveBorderWidth);
					clip(DissolvePow - 0.5);
					emissive += _DissolveGlowStrength * lerp((ClipStepA * 0.0) + (ClipStepB * 1.0), 0.0, ClipStepA * ClipStepB) * _DissolveGlowColor.rgb;
				#endif
                
				//Final Emission
				half3 finalColor = emissive;
                return fixed4(finalColor,(_MainTex_var.a * i.vertexColor.a));
            }
            ENDCG
        }
		Pass{
			Name "ShadowCaster"
			Tags{
			"LightMode" = "ShadowCaster"
			}
			Offset 1, 1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_PASS_SHADOWCASTER
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma shader_feature SPRITE_SCROLLING_ON
			#pragma multi_compile_shadowcaster
			#pragma multi_compile _ DISSOLVE_ON
			#pragma target 3.0
			#pragma glsl
			uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
			uniform half4 _Color;
			uniform half _Layer0ScrollingX;
			uniform half _Layer0ScrollingY;
			uniform sampler2D _DissolveTex; uniform float4 _DissolveTex_ST;
			uniform half _DissolveBlend;
			struct VertexInput {
			float4 vertex : POSITION;
			float2 texcoord0 : TEXCOORD0;
			float4 vertexColor : COLOR;
			};
			struct VertexOutput {
				V2F_SHADOW_CASTER;
				float2 uv0 : TEXCOORD1;
				float4 vertexColor : COLOR;
			};
			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;

				//Scroll UV
				#if SPRITE_SCROLLING_ON
					o.uv0 = half2(o.uv0.r + _Time.w * _Layer0ScrollingX, o.uv0.g + _Time.w * _Layer0ScrollingY);
				#endif

				o.vertexColor = float4(_Color.rgb, _Color.a);
				o.pos = UnityObjectToClipPos(v.vertex);
				TRANSFER_SHADOW_CASTER(o)
					return o;
			}
			float4 frag(VertexOutput i) : COLOR{
				#if DISSOLVE_ON
					half4 _DissolveTex_var = tex2D(_DissolveTex, TRANSFORM_TEX(i.uv0, _DissolveTex));
					half DissolveRemap = (_DissolveBlend * -1.0 + 1.0);
					half DissolvePow = pow(((_DissolveTex_var.r*DissolveRemap) * 5.0), 35.0);
					clip(clamp(DissolvePow, 0, 1) - 0.5);
				#else
					half4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));
					clip((_MainTex_var.a * i.vertexColor.a) - 0.5);
				#endif
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
    }
    FallBack "Sprites/Diffuse"
    CustomEditor "NGSMaterialInspector"
}