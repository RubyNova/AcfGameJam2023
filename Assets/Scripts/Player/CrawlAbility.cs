using System.Collections;

namespace Player
{
    public class CrawlAbility : PlayerAbilityBehaviour
    {
        private void Awake()
        {
            Name = "Crawl";
            _isPassiveAbility = true;
        }

        private void Start()
        {
            PlayerController.Instance.CanCrawl = true; // god this is such a hack LOL
        }

        protected override void EnforceInputDefaults() // this does nothing input-wise since this is a passive ability.
        {
            _isPassiveAbility = true;
        }

        protected override IEnumerator ExecuteAbility(PlayerContextObject context)
        {
            throw new System.NotImplementedException();
        }
    }
}
