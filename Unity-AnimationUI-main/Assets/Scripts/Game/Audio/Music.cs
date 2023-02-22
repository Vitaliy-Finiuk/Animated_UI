using UnityEngine;

namespace Game.Audio
{
	public class Music : MonoBehaviour
	{

		public AudioClip[] tracks;
		public AudioSource source;
		public bool shuffleTracksOnStart;
		private int[] _playOrder;
		private int _nextTrackIndex;
		private float _nextTrackStartTime;

		private static Music instance;


		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
				DontDestroyOnLoad(gameObject);
				Init();
			}
			else
				Destroy(gameObject);
		}


		private void Init()
		{
			_playOrder = Seb.ArrayHelper.CreateIndexArray(tracks.Length);
			if (shuffleTracksOnStart) 
				Seb.ArrayHelper.ShuffleArray(_playOrder, new System.Random());
			
			_nextTrackIndex = 0;
		}

		private void Update()
		{
			if (Time.time > _nextTrackStartTime)
			{
				if (tracks[_nextTrackIndex] != null)
				{
					source.Stop();
					source.clip = tracks[_nextTrackIndex];
					source.Play();
					_nextTrackStartTime = Time.time + source.clip.length;
					_nextTrackIndex = (_nextTrackIndex + 1) % tracks.Length;
				}
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticValues()
		{
			instance = null;
		}
	}
}
