using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.ShaderGraph;
using System.Reflection;

[Title("Custom", "CrackedCustomNode")]
public class CrackedCustomNode : CodeFunctionNode
{
    public CrackedCustomNode()
    {
        name = "Cracked Custom Node";

    }

    protected override MethodInfo GetFunctionToConvert()
    {
        return GetType().GetMethod("CrackedCustomFunction", BindingFlags.Static | BindingFlags.NonPublic);
    }

    static string CrackedCustomFunction(
        [Slot(0, Binding.None)] Vector3 View,
        [Slot(1, Binding.None)] Vector3 Normal,
        [Slot(2, Binding.None)] Vector1 IOR,
        [Slot(3, Binding.None)] out Vector3 Out)
    {
        Out = Vector3.zero;
        return
            @"
{
Out = refract(View,Normal,IOR);
}";
    }
}
