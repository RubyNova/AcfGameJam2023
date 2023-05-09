using ACHNarrativeDriver;
using UnityEngine;
using Utilities;

namespace UI
{
    public class MenuController : MonoSingleton<MenuController> 
    {
        [SerializeField]
        private GameObject _pauseMenu;

        [SerializeField]
        private NarrativeUIController _narrativeMenu;

        public NarrativeUIController NarrativeMenu => _narrativeMenu;

        public bool IsPaused { get; private set; }

        protected override void OnInit()
        {
            IsPaused = false;
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
