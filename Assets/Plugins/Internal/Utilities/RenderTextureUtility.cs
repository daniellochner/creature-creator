using UnityEngine;

namespace DanielLochner.Assets
{
    public static class RenderTextureUtility
    {
        public static Texture2D ToTexture2D(this RenderTexture renderTexture, int resolution = 512)
        {
            Texture2D tex = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);

            RenderTexture currentRenderTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = currentRenderTexture;

            return tex;
        }
    }
}