using Game.Audio;
using Game.Solar_System;
using UnityEngine;

// Quick test to ensure that a bunch of settings I often change while testing have been properly reset for build.
namespace Editor_Helper
{
	public class BuildReadyTest : MonoBehaviour
	{
		public SolarSystemManager solarSystem;
		public Music music;

		[NaughtyAttributes.Button]
		public void Test()
		{
			Debug.Assert(solarSystem.animate == true, "SolarSystem animation disabled");
			Debug.Assert(music.tracks != null && music.tracks.Length > 0 && music.tracks[0] != null, "Music Missing");
		}
	}
}
