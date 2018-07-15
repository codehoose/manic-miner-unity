using System;

namespace Com.SloanKelly.ZXSpectrum
{
	/// <summary>
	/// Representation of the attribute block in a ZX Spectrum.
	/// </summary>
	public class ZXAttribute
	{
        #region Static methods

        public static int GetInk(byte attr)
        {
            return attr & 0x07; // 0x07 == 111
        }

        public static int GetPaper(byte attr)
        {
            return (attr >> 3) & 0x07;
        }

        public static bool GetFlashing(byte attr)
        {
            return (attr & 0x80) == 0x80; // 0x80 == 1000 0000
        }

        public static bool GetBright(byte attr)
        {
            return ((attr << 1) & 0x80) == 0x80;
        }

        #endregion

        /// <summary>
        /// Ink colour.
        /// </summary>
        /// <value>The ink.</value>
        public int Ink { get; set; }

		/// <summary>
		/// Paper colour.
		/// </summary>
		/// <value>The paper.</value>
		public int Paper { get; set; }

		/// <summary>
		/// The block is bright.
		/// </summary>
		/// <value><c>true</c> if bright; otherwise, <c>false</c>.</value>
		public bool Bright { get; set; }

		/// <summary>
		/// The block is flashing.
		/// </summary>
		/// <value><c>true</c> if flashing; otherwise, <c>false</c>.</value>
		public bool Flashing { get; set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public ZXAttribute()
		{
			Paper = 0;
			Ink = 7;

			Bright = false;
			Flashing = false;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="ink">Ink.</param>
		/// <param name="paper">Paper.</param>
		/// <param name="bright">If set to <c>true</c> bright.</param>
		/// <param name="flashing">If set to <c>true</c> flashing.</param>
		public ZXAttribute(int ink, int paper, bool bright = false, bool flashing = false)
		{
			Ink = ink;
			Paper = paper;
			Bright = bright;
			Flashing = flashing;
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rawAttr">Raw ZX Spectrum formatted attribute</param>
        public ZXAttribute(byte rawAttr)
        {
            Ink = GetInk(rawAttr); // 0x07 == 111
            Paper = GetPaper(rawAttr);
            Flashing = GetFlashing(rawAttr); // 0x80 == 1000 0000
            Bright = GetBright(rawAttr);
        }
	}
}
	