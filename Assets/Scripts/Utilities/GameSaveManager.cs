using Player;
using System;
using UnityEngine;

namespace Utilities
{
    public class GameSaveManager : PocoSingleton<GameSaveManager>
    {
        // this makes me cry - Matt
        private static AsyncSceneSwitcher.SceneAsEnum[] saveRoomRanges = {
           AsyncSceneSwitcher.SceneAsEnum.AnxietyGameWorld,
           AsyncSceneSwitcher.SceneAsEnum.AnxietyGameWorld,
           AsyncSceneSwitcher.SceneAsEnum.DespairGameWorld,
           AsyncSceneSwitcher.SceneAsEnum.ParanoiaGameWorld,
           AsyncSceneSwitcher.SceneAsEnum.ParanoiaGameWorld,
           AsyncSceneSwitcher.SceneAsEnum.RageGameWorld,
           AsyncSceneSwitcher.SceneAsEnum.FinalAreaGameWorld
};

        [Flags]
        private enum Abilities
        {
            None = 1,
            Crawl = 1 << 1,
            DoubleJump = 1 << 2,
            Dash = 1 << 3,
            Pushback = 1 << 4
        }

        
        [Serializable]
        private class SaveGameModel
        {
            public int saveRoomIndex;
            public int lightItems;
            public int heavyItems;
            public Abilities abilitiesGained;
        }
        

        public void SaveGame(int saveRoomIndex)
        {
            Abilities abilities = Abilities.None;
            
            foreach(var component in PlayerController.Instance.GetComponents<PlayerAbilityBehaviour>())
            {
                abilities |= component switch
                {
                    CrawlAbility => Abilities.Crawl,
                    DashAbility => Abilities.Dash,
                    DoubleJumpAbility => Abilities.DoubleJump,
                    RageAbility => Abilities.Pushback,
                    _ => throw new NotImplementedException()
                };
            }

            var inventory = PlayerController.Instance.GetComponent<Inventory>();

            SaveGameModel model = new()
            {
                saveRoomIndex = saveRoomIndex,
                lightItems = inventory.LightItems,
                heavyItems = inventory.HeavyItems,
                abilitiesGained = abilities
            };

            PlayerPrefs.SetString("StainedGlassHeartSave", JsonUtility.ToJson(model));
        }

        public void LoadGame()
        {   
            var json = PlayerPrefs.GetString("StainedGlassHeartSave");

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException(nameof(json), "Save does not exist!");
            }

            var model = JsonUtility.FromJson<SaveGameModel>(json);


        }
    }
}
