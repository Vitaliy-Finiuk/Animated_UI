using System.Collections.Generic;
using Game.City_Lights;
using Game.Terrain_Lookup;
using Generation.Terrain;
using Generation.Terrain.Settings;
using Menu;
using UnityEngine;

namespace Game.World
{
	public class LoadingManager : MonoBehaviour
	{

		public bool logTaskLoadTimes;
		public bool logTotalLoadTime;

		[Header("References")]
		public LoadScreen loadScreen;
		public TerrainHeightSettings heightSettings;
		public TerrainHeightProcessor heightProcessor;
		public CityLights cityLights;
		public WorldLookup worldLookup;
		public Light sunLight;
		public AtmosphereEffect atmosphereEffect;

		public LodMeshLoader terrainLoader;
		public MeshLoader oceanLoader;
		public MeshLoader countryOutlineLoader;

		public GameObject[] deactivateWhileLoading;

		private void Awake()
		{
			Load();
		}

		public LoadTask[] GetTasks()
		{
			List<LoadTask> tasks = new List<LoadTask>();

			AddTask(() => heightProcessor.ProcessHeightMap(), "Processing Height Map");
			AddTask(() => cityLights.Init(heightProcessor.processedHeightMap, sunLight), "Creating City Lights");
			AddTask(() => worldLookup.Init(heightProcessor.processedHeightMap), "Initializing World Lookup");
			AddTask(() => terrainLoader.Load(), "Loading Terrain Mesh");
			AddTask(() => oceanLoader.Load(), "Loading Ocean Mesh");
			AddTask(() => countryOutlineLoader.Load(), "Loading Country Outlines");

			void AddTask(System.Action task, string name)
			{
				tasks.Add(new LoadTask(task, name));
			}

			return tasks.ToArray();
		}


		private void Load()
		{
			var loadTimer = System.Diagnostics.Stopwatch.StartNew();
			OnLoadStart();
			LoadTask[] tasks = GetTasks();

			foreach (LoadTask task in tasks)
			{
				long taskTime = task.Execute(null, false);
				if (logTaskLoadTimes)
				{
					Debug.Log($"{task.taskName}: {taskTime} ms.");
				}
			}

			OnLoadFinish();
			if (logTotalLoadTime)
			{
				Debug.Log($"Total load duration: {loadTimer.ElapsedMilliseconds} ms.");
			}
		}


		private void OnLoadStart()
		{
			SetActiveStateAll(deactivateWhileLoading, false);
			loadScreen.gameObject.SetActive(true);
			loadScreen.Init();
		}

		private void OnLoadFinish()
		{
			// Release any memory from stuff no longer needed after all generation is finished
			heightProcessor.Release();
			Resources.UnloadUnusedAssets(); // not sure if any good reason to do this (?)

			// Start game
			SetActiveStateAll(deactivateWhileLoading, true);
			loadScreen.Close();
		}

		public class LoadTask
		{
			public System.Action task;
			public string taskName;

			public LoadTask(System.Action task, string name)
			{
				this.task = task;
				this.taskName = name;
			}

			public long Execute(LoadScreen loadScreen, bool log)
			{
				if (log)
				{
					loadScreen.Log(taskName, newLine: true);
				}
				var sw = System.Diagnostics.Stopwatch.StartNew();
				task.Invoke();

				if (log)
				{
					loadScreen.Log($" {sw.ElapsedMilliseconds}ms.", newLine: false);
				}
				return sw.ElapsedMilliseconds;
			}
		}

		static void SetActiveStateAll(GameObject[] gameObjects, bool isActive)
		{
			foreach (var g in gameObjects) 
				g.SetActive(isActive);
		}

	}
}
