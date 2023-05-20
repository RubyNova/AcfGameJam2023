using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEngine;
using MenuController = UI.MenuController;

namespace DevHelpers
{
    public class DiaryPages : MonoBehaviour
    {
        [SerializeField] private NarrativeUIController _target;
        [SerializeField] private NarrativeSequence _targetDiaryPage;
        
        public void OnClick()
        {
            //resume the game using Resume() method
            MenuController.Instance.Resume();

            //Now we can play the diary page
            _target.ExecuteSequence(_targetDiaryPage);
        }
    }
}