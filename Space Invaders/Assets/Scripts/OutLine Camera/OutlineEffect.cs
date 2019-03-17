using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    public bool Gauss;

    public Material WhiteMaker;
    public Material PostOutline;
    public Material GaussianBlur;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture TempRT = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);
        RenderTexture whiteRT = new RenderTexture(TempRT.width, TempRT.height, TempRT.depth);

        Graphics.Blit(source, whiteRT, WhiteMaker);
        if (Gauss)
        {
            Graphics.Blit(whiteRT, TempRT, PostOutline);
            Graphics.Blit(TempRT, destination, GaussianBlur);
        }
        else
        {
            Graphics.Blit(whiteRT, destination, PostOutline);
        }
    }
}