using UnityEngine;
[ExecuteInEditMode]
public class BilateralFilterEffect : MonoBehaviour
{
    public enum BlurType
    {
        GaussianBlur = 0,
        BilateralColorFilter = 1,
        BilateralNormalFilter = 2,
    }
    private Material filterMaterial = null;
private Camera currentCamera = null;
[Range(1, 4)]
public int BlurRadius = 1;
public BlurType blurType = BlurType.GaussianBlur;
[Range(0, 0.2f)]
public float bilaterFilterStrength = 0.15f;
private void Awake()
    {
        var shader = Shader.Find("AO/BilateralFilterEffect");
filterMaterial = new Material(shader);
currentCamera = GetComponent<Camera>();
    }
    private void OnEnable()
    {
        currentCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
    }
    private void OnDisable()
    {
        currentCamera.depthTextureMode &= ~DepthTextureMode.DepthNormals;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var tempRT = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
var blurPass = (int)blurType;
filterMaterial.SetFloat("_BilaterFilterFactor", 1.0f - bilaterFilterStrength);
        filterMaterial.SetVector("_BlurRadius", new Vector4(BlurRadius, 0, 0, 0));
        Graphics.Blit(source, tempRT, filterMaterial, blurPass);
        filterMaterial.SetVector("_BlurRadius", new Vector4(0, BlurRadius, 0, 0));
        Graphics.Blit(tempRT, destination, filterMaterial, blurPass);
        RenderTexture.ReleaseTemporary(tempRT);
    }
}
