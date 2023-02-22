using UnityEngine;

namespace Generation.Terrain.Settings
{
	[CreateAssetMenu(menuName = "Terrain/Height Settings")]
	public class TerrainHeightSettings : ScriptableObject
	{
		public float worldRadius = 150;
		public float heightMultiplier = 3;
	}

}