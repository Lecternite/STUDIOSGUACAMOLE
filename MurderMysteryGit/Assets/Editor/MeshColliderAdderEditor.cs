using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(MeshColliderAdder))]
public class MeshColliderAdderEditor : Editor
{
	private MeshColliderAdder script;

	private void OnEnable()
	{
		// Method 1
		script = (MeshColliderAdder)target;
	}

	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Update Mesh Colliders"))
		{
			script.addColliders();
		}

		// Draw default inspector after button...
		base.OnInspectorGUI();
	}
}