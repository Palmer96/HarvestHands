using UnityEngine;
using System.Collections;

public class EdgeDetectEffect : MonoBehaviour
{
    public Shader EdgeDetectShader;
    public Material material
    {
        get
        {
            if (mat == null)
            {
                mat = new Material(EdgeDetectShader);
                mat.hideFlags = HideFlags.HideAndDontSave;
            }
            return mat;
        }
    }

    private Material mat;

    void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
        if (!EdgeDetectShader && !EdgeDetectShader.isSupported)
        {
            enabled = false;
            return;
        }
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (EdgeDetectShader != null)
            Graphics.Blit(src, dst, material);
        else
            Graphics.Blit(src, dst);
    }
}