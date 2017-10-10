using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mathx;
using Isosurface;

namespace Isosurface
{
	[System.Serializable]
	public class Isosurface3D
	{
		#region Public Properties

		[SerializeField]
		private Vector3Int size = new Vector3Int (25, 10, 25);
		public Vector3Int Size
		{
			get
			{
				return size;
			}
			set
			{
				size = value;
			}
		}

		[Range (-1f, 1f)]
		public float surfaceCrossValue = 0;

		private float[,,] data;
		public float[,,] Data
		{
			get
			{
				return data;
			}
			set
			{
				data = value;
			}
		}

		#endregion



		#region Constructors

		public Isosurface3D ()
		{
			data = new float[size.x, size.y, size.z];
		}

		public Isosurface3D (int size)
		{
			data = new float[size, size, size];
		}

		public Isosurface3D (Vector3Int size)
		{
			data = new float[size.x, size.y, size.z];
		}

		#endregion



		#region Main Methods

		public void BuildData (ref Mesh isoMesh)
		{
			for (int x = 0; x < data.GetLength (0); x++)
			{
				for (int y = 0; y < data.GetLength (1); y++)
				{
					for (int z = 0; z < data.GetLength (2); z++)
					{
						if (x == 0 || x == size.x - 1)
						{
							data[x, y, z] = -1;
							continue;
						}

						if (y == 0 || y == size.y - 1)
						{
							data[x, y, z] = -1;
							continue;
						}

						if (z == 0 || z == size.z - 1)
						{
							data[x, y, z] = -1;
							continue;
						}
					}
				}
			}

			data.Clamp (-1f, 1f);

			IsoUtils.BuildMeshFromData (ref isoMesh, data, size, surfaceCrossValue + 0.0001f);
		}

		public void ProcessData (System.Func<Vector3, float> method)
		{
			ProcessData (method, Vector3.zero);
		}

		public void ProcessData (System.Func<Vector3, float> method, Vector3 worldPosition)
		{
			ResizeData (size);
		}

		public void ProcessAllData (System.Func<float[,,], float[,,]> method)
		{
			data = method (data);
		}

		public void OverwriteData (float[,,] dataToBeSetTo)
		{
			data = dataToBeSetTo;
		}

		#endregion



		#region Utility Methods

		public void ResizeData (Vector3Int size)
		{
			data = new float[Mathf.Abs (size.x), Mathf.Abs (size.y), Mathf.Abs (size.z)];
		}

		public void ClearData ()
		{
			data = new float[data.GetLength (0), data.GetLength (1), data.GetLength (2)];
		}

		#endregion
	}
}