using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DebugCreateSprite : MonoBehaviour
{
    void Awake()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        
        SpriteTexture spriteTexture = new SpriteTexture(8, 8, new Vector2(0, 1));
        spriteTexture.Clear(new Color(0, 0, 0, 0));

        spriteTexture.SetLine(0, 0x00);
        spriteTexture.SetLine(1, 0x66);
        spriteTexture.SetLine(2, 0x42); // smiley face
        spriteTexture.SetLine(5, 0x42);
        spriteTexture.SetLine(6, 0x3c);
        spriteTexture.SetLine(7, 0x00);

        //for (int i = 0; i < 8; i++)
        //{
        //    spriteTexture.SetLine(i, (byte)Random.Range(128, 256));
        //}

        spriteRenderer.sprite = spriteTexture.Apply();
    }
}
