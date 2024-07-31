using UnityEngine;

namespace MainMenu
{
    public class GameSelector : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;

        public void OnBackSelected()
        {
            gameObject.SetActive(false);
            mainMenuPanel.SetActive(true);
        }

        internal void Start()
        {
            if (Clusterbyte.QUEUED_LEVEL == 0)
            {

            }
        }
    }
}