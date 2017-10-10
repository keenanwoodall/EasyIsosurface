using UnityEngine;
using System.Collections;
using Isosurface;
using Procedural.Noise;

public class NoiseExample : MonoBehaviour
{
	#region Public Properties

	public bool useWorldPosition = true;

	public PerlinObject perlin;

	#endregion



	#region Utility Methods

	public void ProcessData (float[,,] data, Isosurface3D isosurfaceReference)
	{
		for (int x = 0; x < data.GetLength (0); x++)
		{
			for (int y = 0; y < data.GetLength (1); y++)
			{
				for (int z = 0; z < data.GetLength (2); z++)
				{
					Vector3 addOffset = new Vector3 ();

					if (useWorldPosition)
						addOffset = transform.position;

					data[x, y, z] = perlin.GetValue (new Vector3 (x, y, z) + addOffset);
				}
			}
		}

		isosurfaceReference.Data = data;
	}

	#endregion
}