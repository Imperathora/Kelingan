using UnityEngine;
using UnityEditor.ShaderGraph;
using System.Reflection;


[Title("Custom", "Get Lighting Information")]
public class GetLightingInformation : CodeFunctionNode
{

	
	public GetLightingInformation()
	{
		name = "Get Lighting Information";
	}

	protected override MethodInfo GetFunctionToConvert()
	{
		return GetType().GetMethod("GetLightingInformationFunction",
			BindingFlags.Static | BindingFlags.NonPublic);
	}

	static string GetLightingInformationFunction(
		[Slot(0, Binding.None)] out Vector3 Direction,
		[Slot(1, Binding.None)] out Vector3 Color,
		[Slot(2, Binding.None)] out Vector1 Attenuation)
	{
		Direction = Vector3.zero;
		Color = Vector3.zero;
		//Attenuation = ;

		return
			@"";


//			@"
//{
//#ifdef SHADERGRAPH_PREVIEW
//        Direction = float3(-0.5,0.5,-0.5);
//        Color = float3(1,1,1);
//        Attenuation = 0.4;
//    #else
//        Light light = GetMainLight();
//        Direction = light.direction;
//        Attenuation = light.distanceAttenuation;
//        Color = light.color;
//    #endif
//}
//";

	}
}
