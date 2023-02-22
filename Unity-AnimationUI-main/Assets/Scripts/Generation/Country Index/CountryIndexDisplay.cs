using UnityEngine;

namespace Generation.Country_Index
{
	public class CountryIndexDisplay : MonoBehaviour
	{
		public FilterMode filterMode;
		public MeshRenderer display;
		public int textureWidth;
		private RenderTexture _texture;


		private void Start()
		{
			_texture = FindObjectOfType<CountryIndexMapper>().CreateCountryIndexMap(textureWidth, textureWidth / 2);
			_texture.filterMode = filterMode;
			display.material.mainTexture = _texture;
		}

		private void OnDestroy() => 
			ComputeHelper.Release(_texture);
	}
}
