using System;
using CCore.Scenes;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;
using UnityEngine.SceneManagement;

namespace CCore.Senary.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        private void Awake()
        {
            UIStateMachine.Instance.GetState<GameState>().EnterEvent += OnGameStateEnter;
        }

        private void OnGameStateEnter()
        {
            SceneController.LoadSceneAdditive("Game", (Scene Scene, LoadSceneMode bla) =>
            {
                GameStateMachine.Instance.DoTransition<CreateLevelTransition>();
            });
        }
    }
}