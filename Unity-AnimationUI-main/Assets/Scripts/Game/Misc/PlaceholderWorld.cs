using Generation.Terrain.Settings;
using UnityEngine;

namespace Game.Misc
{
	[ExecuteInEditMode]
	public class PlaceholderWorld : MonoBehaviour
	{

		public TerrainHeightSettings heightSettings;


		private void Start()
		{
			if (Application.isPlaying) 
				gameObject.SetActive(false);
		}

		private void Update()
		{
			if (!Application.isPlaying)
			{
				transform.position = Vector3.zero;
				transform.localScale = Vector3.one * heightSettings.worldRadius * 2;
			}
		}
	}
}
