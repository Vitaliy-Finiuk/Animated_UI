#if UNITY_EDITOR
namespace Editor_Helper
{
	public static class EditorShaderHelper
	{

		public static event System.Action onRebindRequired;
		static bool editorHasFocus;


		static EditorShaderHelper()
		{
			editorHasFocus = true;
			UnityEditor.EditorApplication.update += Update;
			UnityEditor.EditorApplication.playModeStateChanged += PlaymodeStateChanged;
		}

		private static void PlaymodeStateChanged(UnityEditor.PlayModeStateChange state)
		{
			if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
			{
				onRebindRequired = null;
			}
		}

		private static void Update()
		{
			if (UnityEditor.EditorApplication.isPlaying)
			{
				bool focus = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
				if (focus && !editorHasFocus)
				{
					onRebindRequired?.Invoke();
				}
				editorHasFocus = focus;
			}
		}
	}
}
#endif