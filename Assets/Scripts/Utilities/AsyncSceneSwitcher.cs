using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Utilities
{
    class AsyncSceneSwitcher : MonoSingleton<AsyncSceneSwitcher>
    {
        public enum SceneToLoad
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

        protected override void OnInit()
        {
            _sceneLoadingCoroutine = null;
        }

        public void SwitchScene(SceneToLoad newScene, Action<AsyncOperation> sceneReadyForActivationCallback)
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
