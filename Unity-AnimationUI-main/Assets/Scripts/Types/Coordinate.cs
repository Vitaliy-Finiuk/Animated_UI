using UnityEngine;

namespace Types
{
	[System.Serializable]
	public struct Coordinate
	{
		[Range(-Mathf.PI, Mathf.PI)]
		public float longitude;
		[Range(-Mathf.PI / 2, Mathf.PI / 2)]
		public float latitude;

		public Coordinate(float longitude, float latitude)
		{
			this.longitude = longitude;
			this.latitude = latitude;
		}

		public Vector2 ToUV() => 
			new Vector2((longitude + Mathf.PI) / (2 * Mathf.PI), (latitude + Mathf.PI / 2) / Mathf.PI);

		public Vector2 ToVector2() => 
			new Vector2(longitude, latitude);

		public static Coordinate FromVector2(Vector2 vec2D) => 
			new Coordinate(vec2D.x, vec2D.y);

		public CoordinateDegrees ConvertToDegrees() => 
			new CoordinateDegrees(longitude * Mathf.Rad2Deg, latitude * Mathf.Rad2Deg);

		public override string ToString() => 
			$"Coordinate (radians): (longitude = {longitude}, latitude = {latitude})";
	}

	[System.Serializable]
	public struct CoordinateDegrees
	{
		[Range(-180, 180)]
		public float longitude;
		[Range(-90, 90)]
		public float latitude;

		public CoordinateDegrees(float longitude, float latitude)
		{
			this.longitude = longitude;
			this.latitude = latitude;
		}

		public Coordinate ConvertToRadians()
		{
			return new Coordinate(longitude * Mathf.Deg2Rad, latitude * Mathf.Deg2Rad);
		}

		public override string ToString()
		{
			return $"Coordinate (degrees): (longitude = {longitude}, latitude = {latitude})";
		}
	}
}