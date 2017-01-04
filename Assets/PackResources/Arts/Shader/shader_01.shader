// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:32655,y:32930,varname:node_4013,prsc:2|emission-6599-OUT,alpha-4985-OUT,clip-5265-OUT;n:type:ShaderForge.SFN_Color,id:4973,x:31625,y:32785,ptovrint:False,ptlb:texcolor,ptin:_texcolor,varname:node_4973,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:5632,x:31806,y:32892,varname:node_5632,prsc:2|A-4973-RGB,B-1775-RGB;n:type:ShaderForge.SFN_If,id:4347,x:31625,y:33138,varname:node_4347,prsc:2|A-1522-R,B-1914-OUT,GT-397-OUT,EQ-397-OUT,LT-1329-OUT;n:type:ShaderForge.SFN_Tex2d,id:1775,x:31625,y:32969,ptovrint:False,ptlb:tex,ptin:_tex,varname:node_1775,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6563d54d4539244489a92887acf755cb,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1522,x:31391,y:33006,ptovrint:False,ptlb:rjtex,ptin:_rjtex,varname:node_1522,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:356d020116f4cf542811577c28b1afe1,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Vector1,id:397,x:31391,y:33175,varname:node_397,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:1329,x:31391,y:33261,varname:node_1329,prsc:2,v1:0;n:type:ShaderForge.SFN_If,id:5265,x:31625,y:33309,varname:node_5265,prsc:2|A-1522-R,B-8219-R,GT-397-OUT,EQ-397-OUT,LT-1329-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2384,x:31138,y:33337,ptovrint:False,ptlb:rjb,ptin:_rjb,varname:node_2384,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;n:type:ShaderForge.SFN_Color,id:9527,x:31807,y:33464,ptovrint:False,ptlb:rjcolor,ptin:_rjcolor,varname:node_9527,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:5583,x:31979,y:33403,varname:node_5583,prsc:2|A-898-OUT,B-9527-RGB;n:type:ShaderForge.SFN_Subtract,id:898,x:31807,y:33205,varname:node_898,prsc:2|A-5265-OUT,B-4347-OUT;n:type:ShaderForge.SFN_Add,id:1858,x:32042,y:33000,varname:node_1858,prsc:2|A-5632-OUT,B-8286-OUT;n:type:ShaderForge.SFN_VertexColor,id:8219,x:31048,y:33145,varname:node_8219,prsc:2;n:type:ShaderForge.SFN_Add,id:1914,x:31391,y:33370,varname:node_1914,prsc:2|A-8219-R,B-2384-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3605,x:31979,y:33317,ptovrint:False,ptlb:rjadd,ptin:_rjadd,varname:node_3605,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:8286,x:32204,y:33358,varname:node_8286,prsc:2|A-3605-OUT,B-5583-OUT;n:type:ShaderForge.SFN_Multiply,id:6599,x:32265,y:32967,varname:node_6599,prsc:2|A-8375-OUT,B-1858-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8375,x:32067,y:32930,ptovrint:False,ptlb:texadd,ptin:_texadd,varname:node_8375,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:4985,x:32435,y:33285,varname:node_4985,prsc:2|A-1775-R,B-8219-A;proporder:4973-1775-1522-2384-9527-3605-8375;pass:END;sub:END;*/

Shader "Shader Forge/shader_01" {
    Properties {
        _texcolor ("texcolor", Color) = (1,1,1,1)
        _tex ("tex", 2D) = "white" {}
        _rjtex ("rjtex", 2D) = "bump" {}
        _rjb ("rjb", Float ) = 0.05
        _rjcolor ("rjcolor", Color) = (1,0,0,1)
        _rjadd ("rjadd", Float ) = 1
        _texadd ("texadd", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
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
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _texcolor;
            uniform sampler2D _tex; uniform float4 _tex_ST;
            uniform sampler2D _rjtex; uniform float4 _rjtex_ST;
            uniform float _rjb;
            uniform float4 _rjcolor;
            uniform float _rjadd;
            uniform float _texadd;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _rjtex_var = tex2D(_rjtex,TRANSFORM_TEX(i.uv0, _rjtex));
                float node_5265_if_leA = step(_rjtex_var.r,i.vertexColor.r);
                float node_5265_if_leB = step(i.vertexColor.r,_rjtex_var.r);
                float node_1329 = 0.0;
                float node_397 = 1.0;
                float node_5265 = lerp((node_5265_if_leA*node_1329)+(node_5265_if_leB*node_397),node_397,node_5265_if_leA*node_5265_if_leB);
                clip(node_5265 - 0.5);
////// Lighting:
////// Emissive:
                float4 _tex_var = tex2D(_tex,TRANSFORM_TEX(i.uv0, _tex));
                float node_4347_if_leA = step(_rjtex_var.r,(i.vertexColor.r+_rjb));
                float node_4347_if_leB = step((i.vertexColor.r+_rjb),_rjtex_var.r);
                float3 emissive = (_texadd*((_texcolor.rgb*_tex_var.rgb)+(_rjadd*((node_5265-lerp((node_4347_if_leA*node_1329)+(node_4347_if_leB*node_397),node_397,node_4347_if_leA*node_4347_if_leB))*_rjcolor.rgb))));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(_tex_var.r*i.vertexColor.a));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _rjtex; uniform float4 _rjtex_ST;
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
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _rjtex_var = tex2D(_rjtex,TRANSFORM_TEX(i.uv0, _rjtex));
                float node_5265_if_leA = step(_rjtex_var.r,i.vertexColor.r);
                float node_5265_if_leB = step(i.vertexColor.r,_rjtex_var.r);
                float node_1329 = 0.0;
                float node_397 = 1.0;
                float node_5265 = lerp((node_5265_if_leA*node_1329)+(node_5265_if_leB*node_397),node_397,node_5265_if_leA*node_5265_if_leB);
                clip(node_5265 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
