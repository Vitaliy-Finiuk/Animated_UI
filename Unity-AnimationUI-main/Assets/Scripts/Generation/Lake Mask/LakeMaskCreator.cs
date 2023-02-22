using Generation.Marine;
using Generation.Terrain;
using UnityEngine;

namespace Generation.Lake_Mask
{
	public class LakeMaskCreator : MonoBehaviour
	{

		public TextAsset lakeFile;

		void Start()
		{
			var polygons = MarineLoad.ReadPolygons(lakeFile.text);

			foreach (var p in polygons)
			{
				Vector2[] points = p.Outline.GetPointsAsVector2(false);
				int[] tris = Triangulator.Triangulate(points, null);

				Vector3[] verts = Seb.VectorHelper.To3DArray(points);
				var meshData = new Seb.Meshing.SimpleMeshData(verts, tris);
				Seb.Meshing.MeshHelper.CreateRendererObject("Lake", meshData);
			}
		}

	}
}
