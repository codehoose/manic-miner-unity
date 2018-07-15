//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class CharScreen : MonoBehaviour
//{
//    class TextMessage
//    {
//        public string Message { get; private set; }

//        public int X { get; private set; }

//        public int Y { get; private set; }

//        public TextMessage(string msg, int x, int y)
//        {
//            Message = msg;
//            X = x;
//            Y = y;
//        }
//    }

//    List<TextMessage> messages;

//    Texture2D tex;

//    byte[] charSet;

//    //[Tooltip("The char set resource file name")]
//    //public string charSetResource = "charset";

//    [Tooltip("The pixel perfect material")]
//    public Material pixelPerfect;

//    public SpriteRenderer target;

//    public void Cls(bool clearMessages = false)
//    {
//        // Remove any messages that are in the queue to be displayed
//        if (clearMessages) messages.Clear();

//        //Fill the spectrum screen with transparency
//        Color[] fill = tex.GetPixels();
//        Array.ForEach(fill, (c) => c = new Color(0, 0, 0, 0));

//        tex.SetPixels(fill);
//        tex.Apply();
//    }

//    public void PrintAt(string msg, int x, int y) // x <= 31, y <=23
//    {
//        messages.Add(new TextMessage(msg, x, y));
//    }

//    void Awake()
//    {
//        messages = new List<TextMessage>();

//        LoadCharSet();
//        CreateTexture();

//        target.material = pixelPerfect;
//    }

//    public void ApplyText()
//    {
//        Cls();

//        List<Color> pixels = new List<Color>(tex.GetPixels());

//        foreach (var msg in messages)
//        {
//            PrintMessage(pixels, msg.Message, msg.X * 8, (23 - msg.Y) * 8);
//        }

//        tex.SetPixels(pixels.ToArray());
//        tex.Apply();

//        target.sprite = Sprite.Create(tex, new Rect(0, 0, 256, 192), new Vector2(0, 1), 1);
//    }

//    //Texture2D Apply()
//    //{
//    //    ApplyText();
//    //    return tex;
//    //}

//    private void PrintMessage(List<Color> pixels, string msg, int x, int y)
//    {
//        int ptr = (y * 256) + x;

//        foreach (var ch in msg)
//        {
//            int ptrCopy = ptr;

//            int offsetIntoCharset = (ch - ' ') * 8;

//            for (int c = 0; c < 8; c++)
//            {
//                int lineValue = offsetIntoCharset + (7 - c);

//                for (int i = 0; i < 8; i++)
//                {
//                    int pow = 7 - i;
//                    byte mask = (byte)(1 << pow);

//                    if ((charSet[lineValue] & mask) == mask)
//                    {
//                        pixels[ptr + i] = Color.white;
//                    }
//                }

//                ptr += 256;
//            }

//            ptr = ptrCopy;
//            ptr += 8;
//        }
//    }

//    private void CreateTexture()
//    {
//        // Create the texture that represents the text mode of the Speccy
//        tex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
//        tex.filterMode = FilterMode.Point;
//        Cls(true);
//    }

//    //private void LoadCharSet()
//    //{
//    //    // Load the resource from disk
//    //    var ta = Resources.Load<TextAsset>(charSetResource);

//    //    // Reserve memory to load the charset 
//    //    charSet = new byte[ta.bytes.Length];

//    //    // Copy the loaded charset into the member field buffer
//    //    Array.Copy(ta.bytes, charSet, ta.bytes.Length);
//    //}
//}