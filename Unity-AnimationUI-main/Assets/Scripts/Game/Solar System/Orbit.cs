using UnityEngine;
using static System.Math;

namespace Game.Solar_System
{
	public class Orbit
	{
		public static Vector2 CalculatePointOnOrbit(double periapsis, double apoapsis, double t)
		{
			double semiMajorLength = (apoapsis + periapsis) / 2;
			double linearEccentricity = semiMajorLength - periapsis; // distance between centre and focus
			double eccentricity = linearEccentricity / semiMajorLength; // (0 = perfect circle, and up to 1 is increasingly elliptical) 
			double semiMinorLength = Sqrt(Pow(semiMajorLength, 2) - Pow(linearEccentricity, 2));
			double meanAnomaly = t * PI * 2;
			double eccentricAnomaly = SolveKepler(meanAnomaly, eccentricity);

			double ellipseCentreX = -linearEccentricity;
			double pointX = Cos(eccentricAnomaly) * semiMajorLength + ellipseCentreX;
			double pointY = Sin(eccentricAnomaly) * semiMinorLength;

			return new Vector2((float)pointX, (float)pointY);
		}


		private static double SolveKepler(double meanAnomaly, double eccentricity, int maxIterations = 100)
		{
			const double h = 0.0001; 
			const double acceptableError = 0.00000001;
			double guess = meanAnomaly;

			for (int i = 0; i < maxIterations; i++)
			{
				double y = KeplerEquation(guess, meanAnomaly, eccentricity);
				if (Abs(y) < acceptableError)
					break;

				double slope = (KeplerEquation(guess + h, meanAnomaly, eccentricity) - y) / h;
				double step = y / slope;
				guess -= step;
			}
			return guess;

			double KeplerEquation(double E, double M, double e) => 
				M - E + e * Sin(E);
		}
	}
}