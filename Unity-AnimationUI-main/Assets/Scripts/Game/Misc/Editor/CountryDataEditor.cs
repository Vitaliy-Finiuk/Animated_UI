using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game.Misc.Editor
{
	[CustomEditor(typeof(CountryData))]
	public class CountryDataEditor : UnityEditor.Editor
	{

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			CountryData countryData = target as CountryData;

			if (GUILayout.Button("Load Data"))
			{
				Undo.RecordObject(countryData, "Load Country Data");
				countryData.Load();

				// I wouldn't expect this to be necessary in addition to Undo.RecordObject, but for some reason
				// changes are not appearing in version control after saving unless I mark as dirty... (?)
				EditorUtility.SetDirty(countryData);
				// Mark active scene as dirty since a save is required for scriptable object data to be saved (even though it's not part of the scene)
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

			}

		}
	}
}
