using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Solar_System
{
	[ExecuteInEditMode]
	public class RenderingManager : MonoBehaviour
	{

		public StarRenderer starRenderer;
		public AtmosphereEffect atmosphereEffect;

		private bool _atmosphereActive;
		private CommandBuffer _outerSpaceRenderCommand;
		private CommandBuffer _skyRenderCommand;
		private Camera _cam;

		public Mesh mesh;
		public Material mat;
		public Moon moon;

		private void OnEnable() => 
			Setup();

		private void Setup()
		{
			_cam = Camera.main;
			_cam.RemoveAllCommandBuffers();

			_outerSpaceRenderCommand = new CommandBuffer();
			_outerSpaceRenderCommand.name = "Outer Space Render";


			starRenderer?.SetUpStarRenderingCommand(_outerSpaceRenderCommand);
			moon?.Setup(_outerSpaceRenderCommand);
			_cam.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, _outerSpaceRenderCommand);

			_skyRenderCommand = new CommandBuffer();
			_skyRenderCommand.name = "Sky Render";
			atmosphereEffect.SetupSkyRenderingCommand(_skyRenderCommand);

			_atmosphereActive = atmosphereEffect.enabled;
			if (_atmosphereActive) 
				_cam.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, _skyRenderCommand);
		}

		private void Update()
		{
			if (atmosphereEffect.enabled != _atmosphereActive)
			{
				_atmosphereActive = atmosphereEffect.enabled;
				if (_atmosphereActive)
					_cam.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, _skyRenderCommand);
				else
					_cam.RemoveCommandBuffer(CameraEvent.BeforeForwardOpaque, _skyRenderCommand);
			}
		}

		private void OnDisable()
		{
			_skyRenderCommand?.Release();
			_outerSpaceRenderCommand?.Release();
		}

	}
}
