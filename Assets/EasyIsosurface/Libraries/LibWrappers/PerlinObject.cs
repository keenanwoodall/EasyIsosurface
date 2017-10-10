using UnityEngine;
using System;
using System.Collections;
using LibNoise.Generator;
using Mathx;

namespace Procedural.Noise
{
	[Serializable]
	public class PerlinObject
	{
		#region Public Properties

		public Vector3 noiseOffset = Vector3.zero;

		public Vector3 noiseScale = Vector3.one;

		public float frequency = 0.1f;

		public float lacunarity = 1f;

		[Range (1, 8)]
		public int octaveCount = 1;

		public float persistance = 1f;

		public LibNoise.QualityMode quality;

		public int seed = 0;

		#endregion



		#region Hidden Properties

		Perlin perlin = new Perlin ();

		#endregion



		#region Main Methods

		public float GetValue (Vector3 position)
		{
			UpdateNoiseValues ();
			return (float)perlin.GetValue (Vectorx.Multiply (position + noiseOffset, noiseScale));
		}

		#endregion



		#region Utility Methods

		private void UpdateNoiseValues ()
		{
			perlin.Frequency = (float)this.frequency;
			perlin.Lacunarity = (float)this.lacunarity;
			perlin.OctaveCount = this.octaveCount;
			perlin.Persistence = this.persistance;
			perlin.Seed = this.seed;
			perlin.Quality = this.quality;
		}

		#endregion
	}
}