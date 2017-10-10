using UnityEngine;
using System.Collections;
using Isosurface;
using Mathx;
using Debugx;

[AddComponentMenu ("Easy Isosurface/Spline Isosurface")]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
[ExecuteInEditMode]
public class SplineIsosurface : MonoBehaviour
{
	#region Public Properties

	[Header ("Isosurface Parameters")]
	public Isosurface3D isosurface;
	public bool update = true;
	[Header ("Spline Parameters")]
	public Vector3 scale = Vector3.one;
	public float spacing = 0.1f;
	public float maxDistance = 1f;
	public bool fill;

	[Header ("References")]
	public BezierSpline spline;

	#endregion



	#region Hidden Properties

	MeshFilter meshFilter;

	#endregion



	#region Main Methods

	void Update ()
	{
		if (update)
			UpdateMesh ();

		Resources.UnloadUnusedAssets ();
	}

	void OnDrawGizmos ()
	{
		if (!this.isActiveAndEnabled)
			return;

		Vector3 sizeTimesScale = Vectorx.Multiply (isosurface.Size.ToVector (), transform.localScale);
		Gizmos.DrawWireCube (transform.position + (sizeTimesScale / 2), sizeTimesScale);
	}

	#endregion



	#region Utility Methods

	public void UpdateMesh ()
	{
		if (spline != null)
		{
			meshFilter = GetComponent<MeshFilter> ();
			meshFilter.sharedMesh = new Mesh ();
			Mesh mesh = meshFilter.sharedMesh;
			isosurface.ResizeData (isosurface.Size);
			isosurface.ClearData ();
			SetData ();
			isosurface.BuildData (ref mesh);

			mesh.name = transform.name + " Mesh";
			meshFilter.sharedMesh = mesh;
		}
	}
	public GameObject CreateAndAssignSpline ()
	{
		GameObject splineObject = new GameObject ("Bezier Spline");
		splineObject.transform.position = transform.position;
		splineObject.transform.rotation = transform.rotation;
		spline = splineObject.AddComponent<BezierSpline> ();
		return splineObject;
	}
	void SetData ()
	{
		if (spacing < 0.005f)
			spacing = 0.005f;
		isosurface.EraseAll ();
		for (float i = 0; i < 1f; i += spacing)
		{
			Vector3 splinePoint = spline.GetPoint (i);
			Vector3 splineDirection = spline.GetDirection (i).normalized;
			Vector3Int splinePointIndex = new Vector3Int ();

			isosurface.WorldPositionToArrayIndex (ref splinePointIndex, splinePoint, transform, maxDistance);

			if (fill)
				isosurface.FillCube (splinePointIndex, new Vector3Int (scale.x, scale.y, scale.z));
			else
				isosurface.EraseCube (splinePointIndex, new Vector3Int (scale.x, scale.y, scale.z));
		}
	}

	#endregion
}