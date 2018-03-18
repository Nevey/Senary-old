using System;
using System.Collections;
using CCore.Scenes;
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

            LoadUIScene();
        }

        private void OnGameStateEnter()
        {
            LoadGameScene();
        }

        private void LoadUIScene()
        {
            SceneController.LoadSceneAdditive("UI", () =>
            {
                // Need to wait for end of frame so Awake and Start can be called on components first
                StartCoroutine(WaitOneFrame(() =>
                {
                    UIStateMachine.Instance.DoTransition<SplashTransition>();
                }));
            });
        }

        private void LoadGameScene()
        {
            SceneController.LoadSceneAdditive("Game", () =>
            {
                // Need to wait for end of frame so Awake and Start can be called on components first
                StartCoroutine(WaitOneFrame(() =>
                {
                    GameStateMachine.Instance.DoTransition<CreateLevelTransition>();
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