using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Com.SloanKelly.ZXSpectrum;

public class DrawScreen : MonoBehaviour {

	SpectrumScreen scr;

	// Use this for initialization
	void Start () {
		scr = GetComponent<SpectrumScreen> ();
		var sr = GetComponent<SpriteRenderer> ();

		sr.sprite = Sprite.Create (scr.Texture, new Rect (0, 0, 256, 192), new Vector2 (0, 1), 1f);

		scr.Poke (0, 0, 0, 255);

		scr.Poke (0, 0, 1, 255);

		scr.Poke (0, 0, 2, 255);

		scr.Poke (0, 0, 3, 255);

		scr.Poke (0, 0, 7, 255);

		scr.Poke (1, 1, 0, 255);

		scr.Poke (1, 1, 2, 255);

		scr.Poke (1, 1, 4, 255);

		scr.Poke (1, 1, 6, 255);

		byte[] spriteData = new byte[]{ 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0xff, 0x00, 0xff, 0x00, 0xff, 0x00, 0xff, 0x00 };

		//scr.FillAttribute (10, 10, 2, 2, 2, 6, false, true)
		//	.RowOrderSprite ()
		//   .DrawSprite (10, 10, 3, 4, spriteData)
		//	.FillAttribute (12, 12, 2, 2, 5, 6, false, true)
		//	.ColumnOrderSprite ()
		//	.DrawSprite (12, 12, 3, 2, spriteData);

		//scr.SetAttribute (0, 0, 6, 1, true, true);
		scr.FillAttribute(0,0,2,2, 0,7,true,true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
