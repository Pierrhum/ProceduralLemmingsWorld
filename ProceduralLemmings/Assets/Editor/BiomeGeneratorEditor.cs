using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (BiomeGenerator))]
public class BiomeGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		BiomeGenerator mapGen = (BiomeGenerator)target;

		if (DrawDefaultInspector ()) {
			if (mapGen.autoUpdate) {
				mapGen.GenerateMap ();
			}
		}

		if (GUILayout.Button ("Generate")) {
			mapGen.GenerateMap ();
		}
	}
}