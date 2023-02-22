using Game.Misc;
using UnityEngine;

namespace Game.Solar_System
{
	[ExecuteInEditMode]
	public class SolarSystemManager : MonoBehaviour
	{

		public bool animate;

		[Header("Durations")]
		public float dayDurationMinutes;
		public float monthDurationMinutes;
		public float yearDurationMinutes;

		[Header("References")]
		public Sun sun;
		public EarthOrbit earth;
		public Moon moon;
		public StarRenderer stars;
		public Transform player;

		[Header("Time state")]
		[Range(0, 1)]
		public float dayT;
		[Range(0, 1)]
		public float monthT;
		[Range(0, 1)]
		public float yearT;


		private float _fastForwardDayDuration;
		private bool _fastForwarding;
		private float _oldPlayerT;
		private float _fastForwardTargetTime;
		private bool _fastForwardApproachingTargetTime;

		[Header("Debug")]
		public bool geocentric;

		private void Update()
		{

			if (animate && Application.isPlaying && GameController.IsState(GameState.Playing))
			{
				float daySpeed = 1 / (dayDurationMinutes * 60);
				if (_fastForwarding)
				{
					HandleFastforwarding(out daySpeed);
				}

				dayT += daySpeed * Time.deltaTime;
				monthT += 1 / (monthDurationMinutes * 60) * Time.deltaTime;
				yearT += 1 / (yearDurationMinutes * 60) * Time.deltaTime;

				dayT %= 1;
				monthT %= 1;
				yearT %= 1;
			}

			earth?.UpdateOrbit(yearT, dayT, geocentric);
			sun?.UpdateOrbit(earth, geocentric);
			moon?.UpdateOrbit(monthT, earth, geocentric);
			stars?.UpdateFixedStars(earth, geocentric);

		}

		public void FastForward(bool toDaytime)
		{
			_fastForwardTargetTime = (toDaytime) ? 1 : -1;
			_fastForwarding = true;
			_fastForwardApproachingTargetTime = false;
			_oldPlayerT = CalculatePlayerDayT();
		}

		public void SetTimes(float dayT, float monthT, float yearT)
		{
			this.dayT = dayT;
			this.monthT = monthT;
			this.yearT = yearT;
		}


		private void HandleFastforwarding(out float daySpeed)
		{
			daySpeed = 1 / (_fastForwardDayDuration * 60);

			float playerT = CalculatePlayerDayT();
			if (DstToTargetTime(playerT, _fastForwardTargetTime) < DstToTargetTime(_oldPlayerT, _fastForwardTargetTime))
			{
				_fastForwardApproachingTargetTime = true;
			}
			else 
			if (_fastForwardApproachingTargetTime)
				_fastForwarding = false;

			_oldPlayerT = playerT;
		}

		private float CalculatePlayerDayT() => 
			Vector3.Dot(player.position.normalized, -sun.transform.forward);

		private static float DstToTargetTime(float fromT, float targetT) => 
			Mathf.Abs(targetT - fromT);
	}


}