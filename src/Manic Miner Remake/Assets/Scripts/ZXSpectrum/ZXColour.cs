using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.SloanKelly.ZXSpectrum
{
	/// <summary>
	/// ZX Spectrum colour palette, based on https://en.wikipedia.org/wiki/ZX_Spectrum_graphic_modes#Color_palette.
	/// </summary>
	public static class ZXColour
	{
		// Component 'strengths'
		private static float Normal = 0.84f;
		private static float Bright = 1f;

		// Colour matrix
		private static Vector3 VBlack = new Vector3 (0, 0, 0);
		private static Vector3 VBlue = new Vector3 (0, 0, 1f);
		private static Vector3 VGreen = new Vector3(0, 1f, 0);
		private static Vector3 VCyan = new Vector3(0, 1f, 1f);
		private static Vector3 VRed = new Vector3(1f, 0, 0);
		private static Vector3 VMagenta = new Vector3(1f, 0f, 1f);
		private static Vector3 VYellow = new Vector3(1f, 1f, 0f);
		private static Vector3 VWhite = new Vector3 (1f, 1f, 1f);

		// Index accessible Vector3 matrix. TODO: Cheaper to use list..?
		private static Dictionary<int, Vector3> s_colours;

		// Index accessible generated colours
		private static Dictionary<int, Color> s_cache;

		/// <summary>
		/// Initializes the <see cref="Com.SloanKelly.ZXSpectrum.ZXColour"/> class.
		/// </summary>
		static ZXColour()
		{
			s_colours = new Dictionary<int, Vector3> ();
			s_cache = new Dictionary<int, Color> ();

			s_colours [0] = VBlack * Normal;
			s_colours [1] = VBlue * Normal;
			s_colours [2] = VRed * Normal;
			s_colours [3] = VMagenta * Normal;
			s_colours [4] = VGreen * Normal;
			s_colours [5] = VCyan * Normal;
			s_colours [6] = VYellow * Normal;
			s_colours [7] = VWhite * Normal;

			s_colours [8] = VBlack * Bright;
			s_colours [9] = VBlue * Bright;
			s_colours [10] = VRed * Bright;
			s_colours [11] = VMagenta * Bright;
			s_colours [12] = VGreen * Bright;
			s_colours [13] = VCyan * Bright;
			s_colours [14] = VYellow * Bright;
			s_colours [15] = VWhite * Bright;
		}
			
		/// <summary>
		/// Get the colour from the given index. The index is in the range 0->7 inclusive and matches the numbers from the ZX Spectrum palette.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="bright">If set to <c>true</c> for the bright version of the colour.</param>
		public static Color Get(int index, bool bright = false)
		{
			if (index < 0 || index > 7)
				throw new IndexOutOfRangeException ("The specified colour could not be found");

			index = bright ? index + 8 : index;

			// If there is no cached version of this colour, it is made and stored in the cache
			if (!s_cache.ContainsKey(index))
			{
				var clr = s_colours [index];
				s_cache [index] = new Color (clr.x, clr.y, clr.z);
			}

			return s_cache [index];
		}
	}
}

