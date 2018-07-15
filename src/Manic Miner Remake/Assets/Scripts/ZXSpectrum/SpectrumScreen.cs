using System;
using UnityEngine;

namespace Com.SloanKelly.ZXSpectrum
{
	/// <summary>
	/// Spectrum screen. 
	/// </summary>
	public class SpectrumScreen : MonoBehaviour, ISpectrumScreen
	{
		// True if flashing squares are to invert their ink/paper colours
		bool inverse = false;

        // The ZX Spectrum font
        byte[] charSet;

        // Flashing timer
        float inverseTime = 0f;

		// Origin of the screen, top left or bottom left. For pixel drawing only. Attributes origin is always top left.
		enum Origin
		{
			Top,
			Bottom
		}

		/// <summary>
		/// The draw mode for the screen.
		/// </summary>
		enum DrawMode
		{
			/// <summary>
			/// Overwrite the pixels that are in that position.
			/// </summary>
			Overwrite,

			/// <summary>
			/// Or the new pixels with the ones already there.
			/// </summary>
			Or
		}

		/// <summary>
		/// Sprite data format.
		/// </summary>
		enum SpriteFormat
		{
			/// <summary>
			/// Rows are described first.
			/// </summary>
			RowOrder,

			/// <summary>
			/// Columns are described first.
			/// </summary>
			ColumnOrder
		}

		// The origin of the screen, for pixel drawing only.
		Origin _origin = Origin.Top;

		// The Spectrum "Screen".
		Texture2D _tex;

		// The attributes.
		ZXAttribute[,] _attrs;

		// Individual pixels that make up the image.
		bool[,] _pixels;

		// The draw mode.
		DrawMode _drawMode = DrawMode.Overwrite;

        internal void FillAttribute(int x, int y, int width, int height, ZXAttribute attr)
        {
            FillAttribute(x, y, width, height, attr.Ink, attr.Paper, attr.Bright, attr.Flashing);
        }

        // The draw order for the sprites
        SpriteFormat _spriteFormat = SpriteFormat.ColumnOrder;

        [Tooltip("The char set resource file name")]
        public string charSetResource = "charset";

        /// <summary>
        /// The Spectrum screen represented as a Texture that can be attached to a sprite. I recommend using a 1-unit-per-pixel size and an
        /// orthogonal camera size 96; this matches the height of the 192-unit height screen.
        /// </summary>
        /// <value>The texture.</value>
        public Texture2D Texture { get { return _tex; } }

        /// <summary>
        /// Poke the specified byte at x, y, and row.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="row">Row.</param>
        /// <param name="val">Value.</param>
        public void Poke(int x, int y, int row, byte val)
		{
			if (_origin == Origin.Top) 
			{
				y = 23 - y;
			}

			x *= 8;
			y *= 8;

			_pixels.Set (x, y, row, val, _drawMode == DrawMode.Overwrite);
		}

        public void PokePP(int x, int y, int row, int rowOffset, byte val)
        {
            if (_origin == Origin.Top)
            {
                y = 23 - y;
            }

            x *= 8;
            y *= 8;

            y -= rowOffset;

            _pixels.Set(x, y, row, val, _drawMode == DrawMode.Overwrite);
        }

        /// <summary>
        /// Fill the attributes at a given position.
        /// </summary>
        /// <returns>The attribute.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="ink">Ink.</param>
        /// <param name="paper">Paper.</param>
        /// <param name="bright">If set to <c>true</c> bright.</param>
        /// <param name="flashing">If set to <c>true</c> flashing.</param>
        public void FillAttribute(int x, int y, int width, int height, int ink, int paper, bool bright = false, bool flashing = false)
		{
			for (int i = x; i < x + width; i++)
			{
				for (int j = y; j < y + height; j++)
				{
					SetAttributeX (i, j, ink, paper, bright, flashing);
				}
			}
		}

		/// <summary>
		/// Set the overwrite draw mode on.
		/// </summary>
		/// <returns>The draw.</returns>
		public void OverwriteDraw()
		{
			_drawMode = DrawMode.Overwrite;
		}

		/// <summary>
		/// Set the Or draw mode on.
		/// </summary>
		/// <returns>The draw.</returns>
		public void OrDraw()
		{
			_drawMode = DrawMode.Or;
		}

        public void PrintMessage(int x, int y, string msg)
        {
            int ptr = (y * 256) + x;

            foreach (var ch in msg)
            {
                int ptrCopy = ptr;

                int offsetIntoCharset = (ch - ' ') * 8;

                byte[] charBlock = new byte[8];
                Array.Copy(charSet, offsetIntoCharset, charBlock, 0, 8);
                DrawSprite(x, y, 1, 1, charBlock);
                x++;
            }
        }

        /// <summary>
        /// Set the sprite draw to column order.
        /// </summary>
        /// <returns>The order sprite.</returns>
        public void ColumnOrderSprite()
		{
			_spriteFormat = SpriteFormat.ColumnOrder;
		}

		/// <summary>
		/// Set the sprite draw to row order.
		/// </summary>
		/// <returns>The order sprite.</returns>
		public void RowOrderSprite()
		{
			_spriteFormat = SpriteFormat.RowOrder;
		}

        /// <summary>
        /// Draw a sprite at the given position.
        /// </summary>
        /// <returns>The sprite.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="cols">Cols.</param>
        /// <param name="rows">Rows.</param>
        /// <param name="data">Data.</param>
        public void DrawSprite(int x, int y, int cols, int rows, params byte[] data)
        {
            if (_spriteFormat == SpriteFormat.ColumnOrder)
            {
                DrawColumnOrderSprite(x, y, cols, rows, data);
            }
            else
            {
                DrawRowOrderSprite(x, y, cols, rows, data);
            }
        }

        public void DrawSpritePP(int x, int y, int cols, int rows, int rowOffset, params byte[] data)
        {
            DrawRowOrderSpritePP(x, y, cols, rows, rowOffset, data);
        }
        
        /// <summary>
        /// Sets the attribute.
        /// </summary>
        /// <returns>The attribute.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="index">Index.</param>
        /// <param name="bright">If set to <c>true</c> bright.</param>
        /// <param name="flashing">If set to <c>true</c> flashing.</param>
        public void SetAttribute(int x, int y, int ink, int paper, bool bright = false, bool flashing = false)
		{
			if (x < 0 || x >= 32)
				throw new IndexOutOfRangeException ("X value out of range");

			if (y < 0 || y >= 24)
				throw new IndexOutOfRangeException ("Y value out of range");
                
			SetAttributeX (x, y, ink, paper, bright, flashing);
		}
        
        public void SetAttribute(int x, int y, ZXAttribute attr)
        {
            SetAttribute(x, y, attr.Ink, attr.Paper, attr.Bright, attr.Flashing);
        }

        /// <summary>
        /// Initialized the texture.
        /// </summary>
        void Awake () 
		{
            // Load the Spectrum font
            LoadCharSet();

            // Create the texture
            _tex = new Texture2D (256, 192, TextureFormat.RGBA32, false, false);
			_tex.filterMode = FilterMode.Point;

			// The attributes section of the screen
			_attrs = new ZXAttribute[32, 24];

			// The pixels
			_pixels = new bool[256, 192];

			Clear(7, 0, false, false);
		}

		/// <summary>
		/// Update the Spectrum screen.
		/// </summary>
		void Update()
		{
			// Timer for ink / paper flash
			inverseTime += Time.deltaTime;
			if (inverseTime > 0.5f) 
			{
				inverseTime -= 0.5f;
				inverse = !inverse;
			}
		}

		/// <summary>
		/// The late update is used because at this point everything has been drawn on the screen. 
		/// </summary>
		void LateUpdate()
		{
			// Get the current state of the screen
			Color[] pixels = _tex.GetPixels ();

			for (int x = 0; x < 256; x++) 
			{
				for (int y = 0; y < 192; y++) 
				{
					// Determine the origin of the attributes, they are always at the top. Origin only
					// affects the drawing of pixels.
					int attrY = /* _origin == Origin.Top ? */ (191 - y) / 8 /* : y / 8 */ ;
						
					// Get the attriute at this position
					ZXAttribute attr = _attrs [x / 8, attrY];

					// Determing if the attribute block is flashing
					bool flashing = attr.Flashing && inverse;

					// Set the colours for ink and paper
					Color paper = ZXColour.Get (flashing ? attr.Ink : attr.Paper, attr.Bright);
					Color ink = ZXColour.Get (flashing ? attr.Paper : attr.Ink, attr.Bright);

					// Output the pixel
					pixels[y * 256 + x] = _pixels [x, y] ? ink : paper;
				}
			}

			// Apply the pixel changes
			_tex.SetPixels (pixels);
			_tex.Apply ();
		}

		/// <summary>
		/// Clear the screen.
		/// </summary>
		/// <param name="ink">Ink.</param>
		/// <param name="paper">Paper.</param>
		/// <param name="bright">If set to <c>true</c> bright.</param>
		/// <param name="flashing">If set to <c>true</c> flashing.</param>
		public void Clear(int ink, int paper, bool bright, bool flashing = false)
		{
			for (int y = 0; y < 24; y++) 
			{
				for (int x = 0; x < 32; x++) 
				{
					_attrs [x, y] = new ZXAttribute (ink, paper, bright, flashing);
				}
			}

			for (int y = 0; y < 192; y++) 
			{
				for (int x = 0; x < 256; x++) 
				{
					_pixels [x, y] = false;
				}
			}
		}

		/// <summary>
		/// Set the attribute value at the given co-ordinates.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="ink">Ink.</param>
		/// <param name="paper">Paper.</param>
		/// <param name="bright">If set to <c>true</c> bright.</param>
		/// <param name="flashing">If set to <c>true</c> flashing.</param>
		private void SetAttributeX(int x, int y, int ink, int paper, bool bright = false, bool flashing = false)
		{
			_attrs [x, y].Ink = ink;
			_attrs [x, y].Paper = paper;
			_attrs [x, y].Bright = bright;
			_attrs [x, y].Flashing = flashing;	
		}

        /// <summary>
        /// This was the old DrawSprite, let's leave it as-is for now.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cols"></param>
        /// <param name="rows"></param>
        /// <param name="data"></param>
        private void DrawColumnOrderSprite(int x, int y, int cols, int rows, byte[] data)
        {
            int offset = 0;

            int most = _spriteFormat == SpriteFormat.ColumnOrder ? x : y;
            int least = _spriteFormat == SpriteFormat.ColumnOrder ? y : x;

            for (int i = most; i < most + cols; i++)
            {
                for (int j = least; j < least + rows; j++)
                {
                    for (int r = 0; r < 8; r++)
                    {
                        Poke(i, j, r, data[r + offset]);
                    }

                    offset += 8;
                    offset %= data.Length;
                }
            }
        }

        /// <summary>
        /// Draw row-order sprite.
        /// </summary>
        /// <param name="x">X-</param>
        /// <param name="y">Y-</param>
        /// <param name="cols"># Columns (characters)</param>
        /// <param name="rows"># Rows (characters)</param>
        /// <param name="data">Shape data</param>
        private void DrawRowOrderSprite(int x, int y, int cols, int rows, byte[] data)
        {
            int index = 0;

            for (int yy = y; yy < y + rows; yy++)
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int xx = x; xx < x + cols; xx++)
                    {
                        Poke(xx, yy, row, data[index]);
                        index++;
                    }
                }
            }
        }

        private void DrawRowOrderSpritePP(int x, int y, int cols, int rows, int rowOffset, byte[] data)
        {
            int index = 0;

            for (int yy = y; yy < y + rows; yy++)
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int xx = x; xx < x + cols; xx++)
                    {
                        PokePP(xx, yy, row, rowOffset, data[index]);
                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// Load the character set into memory.
        /// </summary>
        private void LoadCharSet()
        {
            // Load the resource from disk
            var ta = Resources.Load<TextAsset>(charSetResource);

            // Reserve memory to load the charset 
            charSet = new byte[ta.bytes.Length];

            // Copy the loaded charset into the member field buffer
            Array.Copy(ta.bytes, charSet, ta.bytes.Length);
        }

    }
}