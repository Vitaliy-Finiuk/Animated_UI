using System.Collections;
using System.Collections.Generic;
using Game.Misc;
using Generation.Terrain.Settings;
using Types;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace Game.Terrain_Lookup
{
	public class WorldLookup : MonoBehaviour
	{
		public TerrainHeightSettings heightSettings;
		public ComputeShader heightMapCompute;
		public ComputeShader lookupShader;
		public Texture2D countryIndices;

		private RenderTexture _heightLookup;

		public void Init(RenderTexture heightMap)
		{
			GraphicsFormat format = GraphicsFormat.R8_UNorm;
			_heightLookup = ComputeHelper.CreateRenderTexture(4096, 2048, FilterMode.Bilinear, format, "Height Lookup");
			Graphics.Blit(heightMap, _heightLookup);
		}

		ComputeBuffer RunLookupCompute(Coordinate coordinate)
		{
			ComputeBuffer resultBuffer = ComputeHelper.CreateStructuredBuffer<float>(2);
			lookupShader.SetTexture(0, "HeightMap", _heightLookup);
			lookupShader.SetTexture(0, "CountryIndices", countryIndices);
			lookupShader.SetBuffer(0, "Result", resultBuffer);
			lookupShader.SetVector("uv", coordinate.ToUV());
			ComputeHelper.Dispatch(lookupShader, 1);
			return resultBuffer;
		}

		public void GetTerrainInfoAsync(Coordinate coord, System.Action<TerrainInfo> callback)
		{
			if (SystemInfo.supportsAsyncGPUReadback)
			{
				ComputeBuffer resultBuffer = RunLookupCompute(coord);
				AsyncGPUReadback.Request(resultBuffer, (request) => AsyncRequestComplete(request, resultBuffer, callback));
			}
			else
				callback.Invoke(GetTerrainInfoImmediate(coord));
		}

		public void GetTerrainInfoAsync(Vector3 point, System.Action<TerrainInfo> callback)
		{
			Coordinate coord = GeoMaths.PointToCoordinate(point.normalized);
			GetTerrainInfoAsync(coord, callback);
		}

		public TerrainInfo GetTerrainInfoImmediate(Coordinate coordinate)
		{
			ComputeBuffer resultBuffer = RunLookupCompute(coordinate);
			float[] data = new float[2];
			resultBuffer.GetData(data);
			resultBuffer.Release();
			return CreateTerrainInfoFromData(data);
		}

		private void AsyncRequestComplete(AsyncGPUReadbackRequest request, ComputeBuffer buffer, System.Action<TerrainInfo> callback)
		{
			if (Application.isPlaying && !request.hasError)
			{
				var info = CreateTerrainInfoFromData(request.GetData<float>().ToArray());
				callback?.Invoke(info);
			}

			ComputeHelper.Release(buffer);

		}

		private TerrainInfo CreateTerrainInfoFromData(float[] data)
		{
			float heightT = data[0];
			float countryT = data[1];

			float worldHeight = heightSettings.worldRadius + heightT * heightSettings.heightMultiplier;
			int countryIndex = (int)(countryT * 255.0) - 1;
			TerrainInfo info = new TerrainInfo(worldHeight, countryIndex);
			return info;
		}


		private void OnDestroy()
		{
			if (RenderTexture.active == _heightLookup) 
				RenderTexture.active = null;

			ComputeHelper.Release(_heightLookup);
		}
	}

	public struct TerrainInfo
	{
		public readonly float height;
		public readonly int countryIndex;

		public TerrainInfo(float height, int countryIndex)
		{
			this.height = height;
			this.countryIndex = countryIndex;
		}

		public bool inOcean
			=> countryIndex < 0;
	}
}