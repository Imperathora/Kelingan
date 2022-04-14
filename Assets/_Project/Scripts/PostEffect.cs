
using UnityEngine;


[ExecuteInEditMode]
public class PostEffect : MonoBehaviour
{

    public Material EffectMaterial;

    // Update is called once per frame
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, EffectMaterial);
    }
}
