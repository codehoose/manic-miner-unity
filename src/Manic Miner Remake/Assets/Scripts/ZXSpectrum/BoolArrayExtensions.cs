using System;
using System.Collections.Generic;

namespace Com.SloanKelly.ZXSpectrum
{
	/// <summary>
	/// Extension methods for boolean arrays.
	/// </summary>
	internal static class BoolArrayExtensions
	{
		/// <summary>
		/// Set the value at given co-ordinates.
		/// </summary>
		/// <param name="arr">Arr.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="row">Row.</param>
		/// <param name="b">The blue component.</param>
		public static void Set(this bool[,] arr, int x, int y, int row, byte b, bool overwrite)
		{
			y += (7 - row);

			for (int i = 7; i >= 0; i--) 
			{
                if ((b & (1 << i)) > 0)
                {
                    // UnityEngine.Debug.Log (x + " " + y);
                    try
                    {
                        arr[x + (7 - i), y] = true;
                    }
                    catch (Exception)
                    {
                        UnityEngine.Debug.Log(x + " " + (7 - i) + " " + i + " " + y);
                    }
                }
                else if (overwrite)
                {
                    arr[x + (7 - i), y] = false;
                }
			}
		}

        //public static void SetPP(this bool[,] arr, int x, int y, byte b)
        //{
        //    for (int i = 7; i >= 0; i--)
        //    {
        //        if ((b & (1 << i)) > 0)
        //        {
        //            try
        //            {
        //                arr[x + (7 - i), y] = true;
        //            }
        //            catch (Exception)
        //            {
        //                UnityEngine.Debug.Log(x + " " + (7 - i) + " " + i + " " + y);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Get the values at the given co-ordinates.
        /// </summary>
        /// <param name="arr">Arr.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="cols">Cols.</param>
        /// <param name="rows">Rows.</param>
        public static byte[] Get(this bool[,] arr, int x, int y, int cols, int rows)
		{
			List<byte> bytes = new List<byte> ();


			for (int c = 0; c < cols; c++) 
			{
				for (int r = 0; r < rows; r++) 
				{
					for (int br = 0; br < 8; br++) 
					{
						bytes.Add(GetByte(arr, x, y + br));
					}

					y += 8;
				}

				x += 8;
			}

			return bytes.ToArray();
		}

		/// <summary>
		/// Get a single byte value at the given co-ordinates.
		/// </summary>
		/// <returns>The byte.</returns>
		/// <param name="arr">Arr.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public static byte GetByte(this bool[,] arr, int x, int y)
		{
			byte b = 0;
			int c = 7;

			for (int i = x; i < x + 8; i++) 
			{
				if (arr [i, y]) 
				{
					b |= (byte)(Math.Pow (2, c));
				}
				c--;
			}

			return b;
		}
	}
}

