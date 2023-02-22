using UnityEngine;

namespace Generation.JFA
{
	public class DistanceDisplay : MonoBehaviour
	{
		public enum Mode { Distance, Direction };

		[Header("Settings")]
		public Mode displayMode;
		public bool highlightLand;
		public float dstMultiplier = 0.75f;

		[Header("References")]
		public JumpFloodTest jumpFlood;
		public MeshRenderer display;

		private void Start() => 
			display.material.mainTexture = jumpFlood.result;

		private void Update()
		{
			display.material.SetInt("displayMode", (int)displayMode);
			display.material.SetInt("highlightLand", (highlightLand) ? 1 : 0);
			display.material.SetFloat("dstMultiplier", dstMultiplier);
		}
	}
}
