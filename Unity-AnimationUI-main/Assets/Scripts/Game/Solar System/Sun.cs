using UnityEngine;

namespace Game.Solar_System
{
	[ExecuteInEditMode]
	public class Sun : MonoBehaviour
	{

		public bool animateSunColour;
		public Gradient sunColGradient;
		public float dayStartOffset;
		Light lightSource;
		Camera cam;

		[Header("Debug")]
		public Color sunColour;
		public float timeOfDayT;
		public int maxSize;

		private void Start()
		{
			lightSource = GetComponent<Light>();
			cam = Camera.main;
		}

		public void UpdateOrbit(EarthOrbit earth, bool geocentric)
		{

			if (geocentric)
			{
				transform.position = Quaternion.Inverse(earth.earthRot) * -earth.earthPos;
				transform.LookAt(Vector3.zero);

				UpdateColourApprox(Vector3.zero);
			}
			else
			{
				transform.position = Vector3.zero;
				transform.LookAt(earth.earthPos);

				UpdateColourApprox(earth.earthPos);
			}
		}

		private void UpdateColourApprox(Vector3 earthPos)
		{

			Vector3 dirToCam = (cam.transform.position - earthPos).normalized;
			Vector3 dirToSun = -transform.forward;
			timeOfDayT = Mathf.Max(0, (Vector3.Dot(dirToCam, dirToSun) + dayStartOffset) / (1 + dayStartOffset));
			sunColour = sunColGradient.Evaluate(timeOfDayT);

			if (animateSunColour) 
				lightSource.color = sunColour;
		}
	}
}