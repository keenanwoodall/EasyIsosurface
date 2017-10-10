using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SplineIsosurface))]
[CanEditMultipleObjects]
public class SplineIsosurfaceInspector : Editor
{
	SplineIsosurface splineIsosurface;

	void OnEnable()
	{
		splineIsosurface = (SplineIsosurface)target;
		Resources.UnloadUnusedAssets();
	}
	void OnDisable()
	{
		Resources.UnloadUnusedAssets();
	}
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if(GUILayout.Button("Create Spline"))
			Selection.activeGameObject = splineIsosurface.CreateAndAssignSpline();
	}
}