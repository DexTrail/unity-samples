using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (G_Map))]
public class E_MapInspector : Editor {

	public override void OnInspectorGUI () {
		base.OnInspectorGUI ();

		if (GUILayout.Button ("Regenerate")) {
			((G_Map)target).Awake ();
		}
	}
}
