using System;
using UnityEngine;
using System.Collections.Generic;
using Mathx;
using Isosurface;

namespace Isosurface
{
	public static class IsoUtils
	{
		#region Main Methods

		public static void BuildMeshFromData (ref Mesh meshToUpdate, float[,,] data, Vector3Int size, float surfaceCrossValue)
		{
			int vertexIndex = 0;
			Vector3[] interpolatedValues = new Vector3[12];

			List<Vector3> vertices = new List<Vector3> ();
			List<int> triangleIndices = new List<int> ();

			for (int x = 0; x < size.x - 1; x++)
			{
				for (int y = 0; y < size.y - 1; y++)
				{
					for (int z = 0; z < size.z - 1; z++)
					{
						if (vertices.Count > 64000)
						{
							Debug.Log ("Isosurfaces vertex count is greater than 64,000, which is the maximum amount that Unity supports.");
							break;
						}

						Vector3 basePoint = new Vector3 (x, y, z);

						//Get the 8 corners of this cube
						float p0 = data[x, y, z];
						float p1 = data[x + 1, y, z];
						float p2 = data[x, y + 1, z];
						float p3 = data[x + 1, y + 1, z];
						float p4 = data[x, y, z + 1];
						float p5 = data[x + 1, y, z + 1];
						float p6 = data[x, y + 1, z + 1];
						float p7 = data[x + 1, y + 1, z + 1];

						//A bitmap indicating which edges the surface of the volume crosses
						int crossBitMap = 0;

						if (p0 < surfaceCrossValue)
							crossBitMap |= 1;
						if (p1 < surfaceCrossValue)
							crossBitMap |= 2;

						if (p2 < surfaceCrossValue)
							crossBitMap |= 8;
						if (p3 < surfaceCrossValue)
							crossBitMap |= 4;

						if (p4 < surfaceCrossValue)
							crossBitMap |= 16;
						if (p5 < surfaceCrossValue)
							crossBitMap |= 32;

						if (p6 < surfaceCrossValue)
							crossBitMap |= 128;
						if (p7 < surfaceCrossValue)
							crossBitMap |= 64;

						//Use the edge look up table to determin the configuration of edges
						int edgeBits = Contouring3D.EdgeTableLookup[crossBitMap];

						//The surface did not cross any edges, this cube is either complelety inside, or completely outside the volume
						if (edgeBits == 0)
							continue;

						float interpolatedCrossingPoint = 0f;

						//Calculate the interpolated positions for each edge that has a crossing value

						//Bottom four edges
						if ((edgeBits & 1) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p0) / (p1 - p0);
							interpolatedValues[0] = Vector3.Lerp (new Vector3 (x, y, z), new Vector3 (x + 1, y, z), interpolatedCrossingPoint);
						}

						if ((edgeBits & 2) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p1) / (p3 - p1);
							interpolatedValues[1] = Vector3.Lerp (new Vector3 (x + 1, y, z), new Vector3 (x + 1, y + 1, z), interpolatedCrossingPoint);
						}

						if ((edgeBits & 4) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p2) / (p3 - p2);
							interpolatedValues[2] = Vector3.Lerp (new Vector3 (x, y + 1, z), new Vector3 (x + 1, y + 1, z), interpolatedCrossingPoint);
						}

						if ((edgeBits & 8) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p0) / (p2 - p0);
							interpolatedValues[3] = Vector3.Lerp (new Vector3 (x, y, z), new Vector3 (x, y + 1, z), interpolatedCrossingPoint);
						}

						//Top four edges
						if ((edgeBits & 16) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p4) / (p5 - p4);
							interpolatedValues[4] = Vector3.Lerp (new Vector3 (x, y, z + 1), new Vector3 (x + 1, y, z + 1), interpolatedCrossingPoint);
						}

						if ((edgeBits & 32) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p5) / (p7 - p5);
							interpolatedValues[5] = Vector3.Lerp (new Vector3 (x + 1, y, z + 1), new Vector3 (x + 1, y + 1, z + 1), interpolatedCrossingPoint);
						}

						if ((edgeBits & 64) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p6) / (p7 - p6);
							interpolatedValues[6] = Vector3.Lerp (new Vector3 (x, y + 1, z + 1), new Vector3 (x + 1, y + 1, z + 1), interpolatedCrossingPoint);
						}

						if ((edgeBits & 128) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p4) / (p6 - p4);
							interpolatedValues[7] = Vector3.Lerp (new Vector3 (x, y, z + 1), new Vector3 (x, y + 1, z + 1), interpolatedCrossingPoint);
						}

						//Side four edges
						if ((edgeBits & 256) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p0) / (p4 - p0);
							interpolatedValues[8] = Vector3.Lerp (new Vector3 (x, y, z), new Vector3 (x, y, z + 1), interpolatedCrossingPoint);
						}

						if ((edgeBits & 512) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p1) / (p5 - p1);
							interpolatedValues[9] = Vector3.Lerp (new Vector3 (x + 1, y, z), new Vector3 (x + 1, y, z + 1), interpolatedCrossingPoint);
						}

						if ((edgeBits & 1024) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p3) / (p7 - p3);
							interpolatedValues[10] = Vector3.Lerp (new Vector3 (x + 1, y + 1, z), new Vector3 (x + 1, y + 1, z + 1), interpolatedCrossingPoint);
						}

						if ((edgeBits & 2048) > 0)
						{
							interpolatedCrossingPoint = (surfaceCrossValue - p2) / (p6 - p2);
							interpolatedValues[11] = Vector3.Lerp (new Vector3 (x, y + 1, z), new Vector3 (x, y + 1, z + 1), interpolatedCrossingPoint);
						}

						crossBitMap <<= 4;

						int triangleIndex = 0;
						while (Contouring3D.TriangleLookupTable[crossBitMap + triangleIndex] != -1)
						{
							int index1 = Contouring3D.TriangleLookupTable[crossBitMap + triangleIndex];
							int index2 = Contouring3D.TriangleLookupTable[crossBitMap + triangleIndex + 1];
							int index3 = Contouring3D.TriangleLookupTable[crossBitMap + triangleIndex + 2];

							vertices.Add (new Vector3 (interpolatedValues[index1].x, interpolatedValues[index1].y, interpolatedValues[index1].z));
							vertices.Add (new Vector3 (interpolatedValues[index2].x, interpolatedValues[index2].y, interpolatedValues[index2].z));
							vertices.Add (new Vector3 (interpolatedValues[index3].x, interpolatedValues[index3].y, interpolatedValues[index3].z));

							triangleIndices.Add (vertexIndex);
							triangleIndices.Add (vertexIndex + 1);
							triangleIndices.Add (vertexIndex + 2);
							vertexIndex += 3;
							triangleIndex += 3;
						}
					}
				}
			}

			List<Vector2> texCoords = new List<Vector2> ();
			Vector2 emptyTexCoords0 = new Vector2 (0, 0);
			Vector2 emptyTexCoords1 = new Vector2 (0, 1);
			Vector2 emptyTexCoords2 = new Vector2 (1, 1);

			for (int texturePointer = 0; texturePointer < vertices.Count; texturePointer += 3)
			{
				texCoords.Add (emptyTexCoords1);
				texCoords.Add (emptyTexCoords2);
				texCoords.Add (emptyTexCoords0);
			}

			//Generate the mesh using the vertices and triangle indices we just created
			if (meshToUpdate != null)
			{
				meshToUpdate.Clear ();
				meshToUpdate.vertices = vertices.ToArray ();
				meshToUpdate.triangles = triangleIndices.ToArray ();
				meshToUpdate.uv = texCoords.ToArray ();
				meshToUpdate.RecalculateNormals ();
				meshToUpdate.RecalculateBounds ();
			}
		}

		#endregion



		#region Utility Methods

		public static void SetCube (this Isosurface3D iso, Vector3Int center, Vector3Int size, float value)
		{
			float[,,] data = iso.Data;
			for (int x = center.x - size.x; x < center.x + size.x; x++)
				for (int y = center.y - size.y; y < center.y + size.y; y++)
					for (int z = center.z - size.z; z < center.z + size.z; z++)
						if (x < data.GetLength (0) && x > 0)
							if (y < data.GetLength (1) && y > 0)
								if (z < data.GetLength (2) && z > 0)
									data[x, y, z] = value;

			iso.Data = data;
		}

		public static void FillCube (this Isosurface3D iso, Vector3Int center, Vector3Int size)
		{
			SetCube (iso, center, size, 1f);
		}

		public static void EraseCube (this Isosurface3D iso, Vector3Int center, Vector3Int size)
		{
			SetCube (iso, center, size, -1f);
		}

		public static void SetAll (this Isosurface3D iso, float value)
		{
			float[,,] data = iso.Data;

			for (int x = 0; x < iso.Size.x; x++)
				for (int y = 0; y < iso.Size.y; y++)
					for (int z = 0; z < iso.Size.z; z++)
						data[x, y, z] = value;

			iso.Data = data;
		}

		public static void FillAll (this Isosurface3D iso)
		{
			SetAll (iso, 1f);
		}

		public static void EraseAll (this Isosurface3D iso)
		{
			SetAll (iso, -1f);
		}

		public static void WorldPositionToArrayIndex (this Isosurface3D iso, ref Vector3Int refIndex, Vector3 position, Transform mesh, float maxDistance)
		{
			float closestDistance = Mathf.Infinity;
			Vector3Int closestIndex = new Vector3Int ();

			for (int x = 0; x < iso.Size.x; x++)
			{
				for (int y = 0; y < iso.Size.y; y++)
				{
					for (int z = 0; z < iso.Size.z; z++)
					{
						Vector3 worldPosition = Vectorx.Multiply (new Vector3 (x, y, z), mesh.localScale) + mesh.position;

						float distance = Vector3.Distance (worldPosition, position);

						if (distance < closestDistance)
						{
							closestDistance = distance;
							closestIndex = new Vector3Int (x, y, z);
						}
					}
				}
			}

			if (closestDistance < maxDistance)
				refIndex = closestIndex;
		}

		#endregion
	}
}