using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEngine;

namespace DevHelpers
{
    public class DiaryPages : MonoBehaviour
    {
        [SerializeField] private NarrativeUIController _target;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private NarrativeSequence _targetDiaryPage;
        
        public void OnClick()
        {
            //resume the game
            _pauseMenu.SetActive(false);

            //Now we can play the diary page
            _target.ExecuteSequence(_targetDiaryPage);
        }
    }
}