/*

% Description of my shader.
% Second line of description for my shader.

keywords: material classic

date: YYMMDD

*/

float4x4 WorldViewProj : WorldViewProjection;
float4 DiffuseColor : DIFFUSE = float4(1.0f, 1.0f,1.0f,1.0f);
float4 AmbiColor : AMBIENT <
    string UIName =  "Ambient Light";
    string UIWidget = "Color";
> = {0.8f,0.8f,0.8f, 1.0f};
float3 LightDir : Direction = float3(4,5,10);


struct VS_IN {
	float3 pos : POSITION;
	float3 normal : NORMAL;
	float4 color : COLOR;
	};

struct VS_OUT {
	float4 Pos : POSITION;
	float4 Col : COLOR;
	float3 Normal : NORMAL;
	};
	
struct PS_IN {
	float4 Color : COLOR0;
	float3 Normal : NORMAL;
	};
	
VS_OUT mainVS(VS_IN In) {
	VS_OUT vOut;
	vOut.Pos = mul(float4(In.pos.xyz, 1.0), WorldViewProj);
	vOut.Col = DiffuseColor * max(dot(LightDir, In.normal), 0) + AmbiColor;
	vOut.Normal = In.normal;
	return vOut;
}

VS_OUT mainVS2(VS_IN In) {
	VS_OUT vOut;
	vOut.Pos = mul(float4(In.pos.xyz, 1.0), WorldViewProj);
	vOut.Col = In.color;
	vOut.Normal = In.normal;
	return vOut;
}

float4 mainPS(VS_OUT In ) : COLOR0 {
	return In.Col;
}

float4 mainPS2(PS_IN In ) : COLOR0 {
	return In.Color * max(dot(LightDir, In.Normal), 0);
}
technique technique0 {
	pass p0 {
		CullMode = CCW;
		VertexShader = compile vs_3_0 mainVS();
		PixelShader = compile ps_3_0 mainPS();
	}
}
technique technique1 {
	pass p0 {
		CullMode = CCW;
		VertexShader = compile vs_3_0 mainVS2();
		PixelShader = compile ps_3_0 mainPS2();
	}
}
