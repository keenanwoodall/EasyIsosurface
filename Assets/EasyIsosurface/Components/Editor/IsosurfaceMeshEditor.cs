using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (IsosurfaceMesh))]
public class IsosurfaceMeshEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		IsosurfaceMesh currentIsoMesh = (IsosurfaceMesh) target;

		DrawDefaultInspector ();

		if (Application.isEditor && Application.isPlaying)
			if (GUILayout.Button ("Single Update"))
				currentIsoMesh.UpdateMesh ();
	}
}