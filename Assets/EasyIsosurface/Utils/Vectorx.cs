using UnityEngine;
using System.Collections;

namespace Mathx
{
	public static class Vectorx
	{
		//--Methods--
		public static Vector3 Multiply(Vector3 thisVector, Vector3 vector)
		{
			float x, y, z;
			x = thisVector.x * vector.x;
			y = thisVector.y * vector.y;
			z = thisVector.z * vector.z;
			return new Vector3(x, y, z);
		}
		public static Vector3 Divide(Vector3 thisVector, Vector3 vector)
		{
			float x, y, z;
			x = thisVector.x / vector.x;
			y = thisVector.y / vector.y;
			z = thisVector.z / vector.z;
			return new Vector3(x, y, z);
		}
	}
	[System.Serializable]
	public struct Vector3Int
	{
		public int x, y, z;

		public static readonly Vector3Int one = new Vector3Int(1, 1, 1);
		
		//--Constructors--
		public Vector3Int(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		public Vector3Int(float x, float y, float z)
		{
			this.x = (int)x;
			this.y = (int)y;
			this.z = (int)z;
		}
		//--Methods--
		public Vector3 ToVector()
		{
			return new Vector3(x, y, z);
		}

		public void SetX (int x)
		{
			this.x = x;
		}
		public void SetY (int y)
		{
			this.y = y;
		}
		public void SetZ (int z)
		{
			this.z = z;
		}

		public override string ToString ()
		{
			return "(" + x + ", " + y + ", " + z + ")";
		}
	}
}