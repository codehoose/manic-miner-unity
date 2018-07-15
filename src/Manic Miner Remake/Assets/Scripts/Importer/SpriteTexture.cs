using UnityEngine;
using UnityEngine.Assertions;

public class SpriteTexture
{
    Texture2D _tex;
    Color[] _colours;
    Vector2 _pivot;

    int _width;
    int _height;

    public SpriteTexture(int width, int height, Vector2 pivot)
    {
        _width = width;
        _height = height;
        _pivot = pivot;

        _tex = new Texture2D(8, 8, TextureFormat.RGBA32, false);
        _tex.filterMode = FilterMode.Point;

        _colours = _tex.GetPixels();
    }

    public void Clear(Color color)
    {
        int length = _width * _height;

        for (int i = 0; i < length; i++)
        {
            _colours[i] = color;
        }
    }

    public void SetLine(int index, byte lineValue)
    {
        index = (_height - 1) - index;
        Assert.IsTrue(index >= 0 && index < _height, "The index parameter is out of range!");

        int ptr = index * _width;

        for (int i = 0; i < 8; i++)
        {
            int pow = 7 - i;
            byte mask = (byte)(1 << pow);

            if ((lineValue & mask) == mask)
            {
                _colours[ptr + i] = Color.white;
            }
        }
    }

    public Sprite Apply()
    {
        _tex.SetPixels(_colours);
        _tex.Apply();

        return Sprite.Create(_tex, new Rect(0, 0, _width, _height), _pivot, 1f, 1, SpriteMeshType.FullRect);
    }
}
