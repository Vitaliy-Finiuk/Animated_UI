using Generation.Terrain;
using UnityEngine;

namespace Game.Misc
{
	public class RenderSettingsController : MonoBehaviour
	{

		[Header("Culling Settings")]
		const float minCullDst = 0.01f;
		public float maxCameraCullDst;
		public float maxLightShadowCullDst;
		public LayerOverride[] layerOverrides;

		[Header("Shadow Settings")]
		public ShadowResolution shadowResolution = ShadowResolution.VeryHigh;
		public float shadowDrawDistance;


		[Header("References")]
		public SimpleLodSystem lodSystem;
		public Light mainLight;
		public Camera mainCamera;

		private static RenderSettingsController instance;

		private void Awake() => 
			ApplySettings();

		private void ApplySettings()
		{
			ApplyCullingValues();
			ApplyShadowSettings();
		}


		private void ApplyCullingValues()
		{
			const int numLayers = 32;
			float[] cameraCullDstPerLayer = new float[numLayers];
			float[] lightCullDstPerLayer = new float[numLayers];

			// Initialize layers to max values
			for (int i = 0; i < numLayers; i++)
			{
				cameraCullDstPerLayer[i] = maxCameraCullDst;
				lightCullDstPerLayer[i] = maxLightShadowCullDst;
			}

			// Override specific layers
			for (int i = 0; i < layerOverrides.Length; i++)
			{
				LayerOverride layerOverride = layerOverrides[i];
				cameraCullDstPerLayer[layerOverride.layer] = layerOverride.cameraCullDst;
				lightCullDstPerLayer[layerOverride.layer] = layerOverride.shadowCullDst;
			}

			mainCamera.farClipPlane = maxCameraCullDst;
			mainCamera.layerCullDistances = cameraCullDstPerLayer;
			mainLight.layerShadowCullDistances = lightCullDstPerLayer;
		}

		private void ApplyShadowSettings()
		{
			QualitySettings.shadowResolution = shadowResolution;
			QualitySettings.shadowDistance = shadowDrawDistance;
		}


		private void OnValidate()
		{
			EnforceCorrectValues();
			ApplySettings();
		}

		private void EnforceCorrectValues()
		{
			if (layerOverrides != null)
			{
				for (int i = 0; i < layerOverrides.Length; i++)
				{
					layerOverrides[i].EnforceCorrectValues();

					maxCameraCullDst = Mathf.Max(maxCameraCullDst, layerOverrides[i].cameraCullDst);
					maxLightShadowCullDst = Mathf.Max(maxLightShadowCullDst, layerOverrides[i].shadowCullDst);
				}
			}

			maxCameraCullDst = Mathf.Max(maxCameraCullDst, minCullDst);
			maxLightShadowCullDst = Mathf.Max(maxLightShadowCullDst, minCullDst);
			maxLightShadowCullDst = Mathf.Min(maxLightShadowCullDst, maxCameraCullDst);
		}

		static RenderSettingsController Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<RenderSettingsController>(includeInactive: true);
				}
				return instance;
			}
		}

		[System.Serializable]
		public struct LayerOverride
		{
			[NaughtyAttributes.Layer]
			public int layer;
			public float cameraCullDst;
			public float shadowCullDst;

			public void EnforceCorrectValues()
			{
				cameraCullDst = Mathf.Max(cameraCullDst, minCullDst);
				shadowCullDst = Mathf.Max(shadowCullDst, minCullDst);
				shadowCullDst = Mathf.Min(shadowCullDst, cameraCullDst);
			}
		}
	}
}
