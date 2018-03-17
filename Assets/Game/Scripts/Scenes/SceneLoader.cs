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

            GameStateMachine.Instance.GetState<ThrowDiceState>().EnterEvent += OnThrowDiceStateEnter;

            LoadUIScene();
        }

        private void OnGameStateEnter()
        {
            LoadGameScene();
        }

        private void OnThrowDiceStateEnter()
        {
            LoadThrowDiceScene();
        }

        private void LoadUIScene()
        {
            SceneController.LoadSceneAdditive("UI", () =>
            {
                // Need to wait for end of frame so Awake and Start can be called on components first
                StartCoroutine(Wait(() =>
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
                StartCoroutine(Wait(() =>
                {
                    GameStateMachine.Instance.DoTransition<CreateLevelTransition>();
                }));
            });
        }

        private void LoadThrowDiceScene()
        {
            SceneController.LoadSceneAdditive("ThrowDice");
        }

        private IEnumerator Wait(Action callback)
        {
            yield return new WaitForEndOfFrame();

            callback();
        }
    }
}