using ACHNarrativeDriver;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class MenuController : MonoSingleton<MenuController> 
    {
        [SerializeField]
        private GameObject _pauseMenu;

        [SerializeField]
        private Canvas _rootCanvas;

        [SerializeField]
        private NarrativeUIController _narrativeMenu;

        [SerializeField]
        private DiaryPagesUnlocker _diaryPageUnlockerControl;

        public NarrativeUIController NarrativeMenu => _narrativeMenu;
        public DiaryPagesUnlocker DiaryPageUnlockerControl => _diaryPageUnlockerControl;

        public bool IsPaused { get; private set; }

        protected override void OnInit()
        {
            Resume();
            SceneManager.activeSceneChanged += (_, _) => _rootCanvas.worldCamera = Camera.main;
        }

        public void Pause()
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            IsPaused = true;
        }

        public void Resume()
        {
            _pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
        }
    }
}
