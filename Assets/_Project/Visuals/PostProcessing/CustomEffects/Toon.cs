using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ToonRenderer), PostProcessEvent.AfterStack, "Custom/Toon")]
public sealed class Toon : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Toon effect intensity.")]
    public FloatParameter rimPower = new FloatParameter { value = 0.5f };

    [Tooltip("bli.")]
    public Vector3Parameter rimColor = new Vector3Parameter {};

    [Tooltip("blub")]
    public Vector4Parameter color = new Vector4Parameter {};
}

public sealed class ToonRenderer : PostProcessEffectRenderer<Toon>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Toon"));
        sheet.properties.SetFloat("_RimPower", settings.rimPower);
        sheet.properties.SetVector("_RimColor", settings.rimColor);
        sheet.properties.SetVector("_Color", settings.color);


        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
