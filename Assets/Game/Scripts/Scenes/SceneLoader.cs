using System;
using System.Collections;
using CCore.Scenes;
using CCore.Senary.StateMachines;
using CCore.Senary.StateMachines.Game;
using CCore.Senary.StateMachines.UI;
using UnityEngine;

namespace CCore.Senary.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        private void Awake()
        {            
            UIStateMachine.Instance.GetState<GameState>().EnterEvent += OnGameStateEnter;

            GameStateMachine.Instance.GetState<GameOverState>().ExitEvent += OnGameOverStateExit;

            LoadUIScene();
        }

        private void OnGameStateEnter()
        {
            LoadGameScene();
        }

        private void OnGameOverStateExit()
        {
            SceneController.UnLoadScene("Game");
        }

        private void LoadUIScene()
        {
            SceneController.LoadSceneAdditive("UI");
        }

        private void LoadGameScene()
        {
            SceneController.LoadSceneAdditive("Game", () =>
            {
                // Need to wait for end of frame so Awake and Start can be called on components first
                StartCoroutine(WaitOneFrame(() =>
                {
                    StateMachineController.Instance.StartGameStateMachine();
                    
                    SceneController.LoadSceneAdditive("Cards");
                }));
            });
        }

        private IEnumerator WaitOneFrame(Action callback)
        {
            yield return new WaitForEndOfFrame();

            callback();
        }
    }
}