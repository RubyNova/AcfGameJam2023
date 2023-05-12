using UnityEngine;
using Utilities;

namespace DevHelpers
{
    public class SceneSwitcherTester : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            AsyncSceneSwitcher.Instance.SwitchScene(AsyncSceneSwitcher.SceneAsEnum.AnxietyGameWorld, operation => operation.allowSceneActivation = true);
        }
    }
}
