using Com.SloanKelly.ZXSpectrum;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpectrumScreen))]
[RequireComponent(typeof(GameController))]
public class RoomRenderer : MonoBehaviour
{
    private SpectrumScreen screen;

    private IList<IRenderer> renderers;

    public Transform target;

    [Tooltip("Clear the screen before each redraw")]
    public bool clearScreen = true;

    [Tooltip("The material that has the pixel perfect check box")]
    public Material pixelPerfect;   
    
    IEnumerator Start()
    {
        screen = GetComponent<SpectrumScreen>();
        var sr = target.GetComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(screen.Texture, 
                                  new Rect(0, 0, 256, 192), 
                                  new Vector2(0, 1), 1f);

        // Wait for the game controller to signal it is ready
        var gameController = GetComponent<GameController>();
        while (!gameController.IsReady)
        {
            yield return null;
        }

        StartCoroutine(DrawScreenInternal(gameController));
    }

    public void Init(IList<IRenderer> renderers)
    {
        var tmp = new List<IRenderer>();
        foreach(var r in renderers)
        {
            r.Init(screen);
            tmp.Add(r);
        }

        this.renderers = tmp;
    }

    public void FloodFill(int paperColour)
    {
        screen.FillAttribute(0, 0, 32, 16, 7, paperColour);
    }

    public void DrawScreen()
    {
        if (clearScreen) screen.Clear(7, 0, false);

        foreach (var r in renderers)
        {
            r.Draw();
        }
    }

    IEnumerator DrawScreenInternal(GameController ctrl)
    {
        while (ctrl.RenderRoom)
        {
            DrawScreen();
            yield return null;
        }
    }
}
