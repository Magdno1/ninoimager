// -----------------------------------------------------------------------
// <copyright file="Map.cs" company="none">
// Copyright (C) 2013
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by 
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful, 
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details. 
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see "http://www.gnu.org/licenses/". 
// </copyright>
// <author>pleoNeX</author>
// <email>benito356@gmail.com</email>
// <date>15/08/2013</date>
// -----------------------------------------------------------------------
using System;
using System.Drawing;

namespace Ninoimager.Format
{
	public struct MapInfo
	{
		public MapInfo(int tileIndex, int paletteIndex, bool flipX, bool flipY)
			: this()
		{
			this.TileIndex = tileIndex;
			this.PaletteIndex = paletteIndex;
			this.FlipX = flipX;
			this.FlipY = flipY;
		}

		public int TileIndex {
			get;
			private set;
		}

		public int PaletteIndex {
			get;
			private set;
		}

		public bool FlipX {
			get;
			private set;
		}

		public bool FlipY {
			get;
			private set;
		}
	}

	public class Map
	{
		private MapInfo[] info;

		private Size tileSize;
		private int width;
		private int height;

		public Map()
		{
			this.info     = null;
			this.tileSize = new Size(0, 0);
			this.width    = 0;
			this.height   = 0;
		}

		public int Width {
			get { return this.width; }
			set { this.width = value; }
		}

		public int Height {
			get { return this.height; }
			set { this.height = value; }
		}

		public Size TileSize {
			get { return this.tileSize; }
			set { this.tileSize = value; }
		}

		public Bitmap CreateBitmap(Image image, Palette palette)
		{
			// TODO: Try to change the tile size of the image
			if (this.tileSize != image.TileSize)
				throw new FormatException("Image with different tile size");

			// TODO: Try to convert image to tiled
			if (image.PixelEncoding != PixelEncoding.HorizontalTiles &&
				image.PixelEncoding != PixelEncoding.VerticalTiles)
				throw new FormatException("Image not tiled.");

			Pixel[] mapImage = new Pixel[this.width * this.height];
			int[] paletteIndex = new int[this.width * this.height];

			int count = 0;
			foreach (MapInfo info in this.info) {
				Pixel[] tile = image.GetTile(info.TileIndex);
				if (info.FlipX)
					FlipX(tile, this.tileSize);	// UNDONE: Flip tile X
				if (info.FlipY)
					FlipY(tile, this.tileSize);	// UNDONE: Flip tile Y

				tile.CopyTo(mapImage, count);

				for (int i = 0; i < tile.Length; i++)
					paletteIndex[count + i] = info.PaletteIndex;

				count += tile.Length;
			}

			// UNDONE: Palette Index must be lineal but it's tiled, convert it.

			Image finalImg = new Image(
				mapImage,
			    this.width,
			    this.height,
			    image.PixelEncoding,
			    image.Format,
			    image.TileSize);
			return finalImg.CreateBitmap(palette, paletteIndex);
		}

		public void SetMapInfo(MapInfo[] mapInfo)
		{
			this.info = (MapInfo[])mapInfo.Clone();
		}

		public MapInfo[] GetMapInfo()
		{
			return (MapInfo[])this.info.Clone();
		}

		private static void FlipX(Pixel[] tile, Size tileSize)
		{
			throw new NotImplementedException();
		}

		private static void FlipY(Pixel[] tile, Size tileSize)
		{
			throw new NotImplementedException();
		}
	}
}