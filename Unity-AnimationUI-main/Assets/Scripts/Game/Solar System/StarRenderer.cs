using Editor_Helper;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Solar_System
{
	public class StarRenderer : MonoBehaviour
	{
		public SolarSystemManager solarSystemManager;
		public Shader starInstanceShader;
		public Light sun;
		public float size;
		Material starMaterial;


		Mesh quadMesh;
		ComputeBuffer argsBuffer;
		ComputeBuffer starDataBuffer;
		Camera cam;

		public float brightnessMultiplier;
		public float appearTimeMin;
		public float appearTimeMax;

		public StarData starData;

		public void SetUpStarRenderingCommand(CommandBuffer cmd)
		{
			if (Application.isPlaying)
			{
				cam = Camera.main;

				//stars = loader.LoadStars();
				CreateQuadMesh();
				EditorOnlyInit();

				starMaterial = new Material(starInstanceShader);

				ComputeHelper.Release(argsBuffer, starDataBuffer);
				argsBuffer = ComputeHelper.CreateArgsBuffer(quadMesh, starData.NumStars);

				starDataBuffer = ComputeHelper.CreateStructuredBuffer(starData.Stars);

				SetBuffer();

				cmd.DrawMeshInstancedIndirect(quadMesh, 0, starMaterial, 0, argsBuffer, 0);
			}
		}

		private void SetBuffer() => 
			starMaterial.SetBuffer("StarData", starDataBuffer);

		public void UpdateFixedStars(EarthOrbit earth, bool geocentric)
		{
			if (Application.isPlaying)//
			{
				starMaterial.SetFloat("size", size);
				starMaterial.SetFloat("brightnessMultiplier", brightnessMultiplier);
				Matrix4x4 rotMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
				// Earth remains stationary and without rotation, so rotate the stars instead
				if (geocentric)
				{
					rotMatrix = Matrix4x4.Rotate(Quaternion.Inverse(earth.earthRot));
				}

				starMaterial.SetMatrix("rotationMatrix", rotMatrix);
			}
		}


		private void CreateQuadMesh()
		{
			quadMesh = new Mesh();

			Vector3[] vertices = {
			new Vector3(-1,-1), // bottom left
			new Vector3(1,-1), // bottom right
			new Vector3(1,1), // top left
			new Vector3(-1, 1) // top right
		};

			int[] triangles = { 0, 2, 1, 0, 3, 2 };

			quadMesh.SetVertices(vertices);
			quadMesh.SetTriangles(triangles, 0, true);
		}

		private void OnDestroy() => 
			ComputeHelper.Release(argsBuffer, starDataBuffer);

		private void EditorOnlyInit()
		{
#if UNITY_EDITOR
			EditorShaderHelper.onRebindRequired += () => SetBuffer();
#endif
		}//
	}
}