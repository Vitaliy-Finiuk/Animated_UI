using UnityEditor;
using UnityEngine;

namespace Game.Solar_System.Editor
{
	[CustomEditor(typeof(StarData))]
	public class StarDataEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			StarData starData = target as StarData;

			if (GUILayout.Button("Generate")) 
				starData.CreateStarData();
		}
	}
}