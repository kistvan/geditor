//ƒeƒXƒg
float4x4 projMat : PROJECTION;
float alpha : alpha;

float4 mainVS(float4 pos : POSITION) : POSITION{
//	return mul(pos, projMat);
	return pos;
}

float4 mainPS() : COLOR {
	return float4(1.0, 1.0, 1.0, alpha);
}

technique technique0 {
	pass p0 {
		CullMode = CCW;
		AlphaBlendEnable        = true;
		SrcBlend = SRCALPHA;
		DestBlend = INVSRCALPHA;
		VertexShader = compile vs_3_0 mainVS();
		PixelShader = compile ps_3_0 mainPS();
	}
}
