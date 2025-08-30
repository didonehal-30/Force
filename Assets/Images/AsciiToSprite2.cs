using UnityEngine;
using UnityEngine.UI;

public class AsciiToSprite2 : MonoBehaviour
{
    public Font asciiFont; // 拖入 Unity 自带 Arial 字体
    public string asciiChar = "✔"; // 例如 "X" 或 "✔" 或 "🔒"

    public int size = 64;  // 生成的图像大小（像素）
    public Color color = Color.black;

    public Image targetImage;

    void Start()
    {
        // 创建临时纹理
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        RenderTexture rt = new RenderTexture(size, size, 24);
        RenderTexture.active = rt;

        // 创建临时摄像机和文字对象
        var goCam = new GameObject("Cam");
        var cam = goCam.AddComponent<Camera>();
        cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.clear;
        cam.targetTexture = rt;

        GameObject goText = new GameObject("Text");
        goText.transform.position = Vector3.zero;
        var text = goText.AddComponent<Text>();
        text.text = asciiChar;
        text.font = asciiFont;
        text.fontSize = size;
        text.color = color;
        text.alignment = TextAnchor.MiddleCenter;
        text.rectTransform.sizeDelta = new Vector2(size, size);

        // 渲染并读取
        cam.Render();
        tex.ReadPixels(new Rect(0, 0, size, size), 0, 0);
        tex.Apply();

        // 生成 Sprite
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        targetImage.sprite = sprite;

        // 清理
        Destroy(goCam);
        Destroy(goText);
        RenderTexture.active = null;
    }
}
