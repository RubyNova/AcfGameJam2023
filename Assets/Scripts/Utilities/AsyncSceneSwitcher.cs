using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class AsyncSceneSwitcher : MonoSingleton<AsyncSceneSwitcher>
    {
        public enum SceneAsEnum
        {
            MainMenu,
            AnxietyGameWorld,
            DespairGameWorld,
            ParanoiaGameWorld,
            RageGameWorld,
            FinalAreaGameWorld,
            Credits
        }

        private Coroutine _sceneLoadingCoroutine;

        public SceneAsEnum? PreviousScene { get; private set; }
        public SceneAsEnum CurrentScene { get; private set; }
        public int? EntranceExitId { get; set; }

        protected override void OnInit()
        {
            _sceneLoadingCoroutine = null;
            PreviousScene = null;
            CurrentScene = (SceneAsEnum)SceneManager.GetActiveScene().buildIndex;
        }

        public void SwitchScene(SceneAsEnum newScene, Action<AsyncOperation> sceneReadyForActivationCallback)
        {
            if (_sceneLoadingCoroutine != null) 
            {
                throw new InvalidOperationException("Attempted to switch scenes while a switch scene was in progress. Are you invoking the same scene multiple times?");
            }

            _sceneLoadingCoroutine = StartCoroutine(LoadSceneAsync());

            IEnumerator LoadSceneAsync()
            {
                var asyncSceneLoad = SceneManager.LoadSceneAsync((int)newScene);
                asyncSceneLoad.allowSceneActivation = false;
                asyncSceneLoad.completed += _ => CurrentScene = newScene;

                while (asyncSceneLoad.progress < 0.9f)
                {
                    yield return null;
                }

                sceneReadyForActivationCallback(asyncSceneLoad);

                _sceneLoadingCoroutine = null;
            }
        }
    }
}
