using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Misc
{
	public enum GameState
	{
		InMainMenu,
		Playing,
		ViewingMap,
		Paused,
		GameOver
	}

	public class GameController : MonoBehaviour
	{
		public event System.Action onGameStarted;

		[SerializeField] GameState startupState;
		[SerializeField] bool allowDevModeToggleInBuild;

		[Header("Debug")]
		[SerializeField]
		private GameState _debugCurrentState;

		private Stack<GameState> _stateStack;

		private static GameController instance;
		private bool _devModeEnabledInBuild;

		private void Awake()
		{
			_stateStack = new Stack<GameState>();
			_stateStack.Push(startupState);
		}

		private void Start()
		{
			if (IsState(GameState.Playing)) 
				StartGame();
		}

		private void Update() => 
			_debugCurrentState = _stateStack.Peek();

		public static void GameOver()
		{
			if (!IsState(GameState.GameOver))
			{
				Time.timeScale = 0;
				SetState(GameState.GameOver);
			}
		}

		public static void SwitchToEndlessMode()
		{
			Time.timeScale = 1;
			ReturnToPreviousState();
		}

		public static void SetPauseState(bool paused)
		{
			if (IsAnyState(GameState.Playing, GameState.ViewingMap, GameState.Paused))
			{
				Time.timeScale = (paused) ? 0 : 1;
				if (paused)
					SetState(GameState.Paused);
				else
					ReturnToPreviousState();
			}
			else
				Debug.Log($"Cannot set pause state when current game state = {CurrentState}");
		}

		static void ReturnToPreviousState()
		{
			if (Instance._stateStack.Count > 0)
			{
				Instance._stateStack.Pop();
			}
			else
			{
				Debug.Log("No previous state to return to... Something went wrong.");
				SetState(GameState.InMainMenu);
			}
		}

		public static void TogglePauseState()
		{
			bool pause = CurrentState == GameState.Playing;
			SetPauseState(pause);
		}

		public static void StartGame()
		{
			SetState(GameState.Playing);
			Instance.onGameStarted?.Invoke();
		}

		public static void SetState(GameState newState)
		{
			if (newState != CurrentState) 
				Instance._stateStack.Push(newState);
		}

		public static void ExitToMainMenu()
		{
			if (IsState(GameState.Paused))
			{
				SetPauseState(false);
			}
			SceneManager.LoadScene(0);
		}

		public static void Quit()
		{
			if (Application.isEditor)
			{
				ExitPlayMode();
			}
			else
			{
				Application.Quit();
			}

		}

		public static bool InDevMode
		{
			get
			{
				return Application.isEditor || Instance._devModeEnabledInBuild;
			}
		}

		public static GameState CurrentState
		{
			get
			{
				return Instance._stateStack.Peek();
			}
		}

		public static bool IsState(GameState state)
		{
			return CurrentState == state;
		}

		public static bool IsAnyState(params GameState[] states)
		{
			foreach (var state in states)
			{
				if (CurrentState == state)
				{
					return true;
				}
			}
			return false;
		}

		public static GameController Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<GameController>(includeInactive: true);
				}
				return instance;
			}
		}


		static void ExitPlayMode()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
	}
}