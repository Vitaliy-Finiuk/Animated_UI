using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Quick test to ensure that a bunch of settings I often change while testing have been properly reset for build.
public class BuildReadyTest : MonoBehaviour
{
	public SolarSystem.SolarSystemManager solarSystem;
	public Music music;

	[NaughtyAttributes.Button]
	public void Test()
	{
		Debug.Assert(solarSystem.animate == true, "SolarSystem animation disabled");
		Debug.Assert(music.tracks != null && music.tracks.Length > 0 && music.tracks[0] != null, "Music Missing");
	}
}
