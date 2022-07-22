using Tanks.Gameplay;
using UnityEngine;

namespace Tanks.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuParent;
        [SerializeField] private GameObject gameMessageText;

        private bool isGamePaused = false;

        private void Update()
        {
            if (Input.GetButtonDown(GameConstants.BUTTON_NAME_PAUSE))
            {
                TogglePauseMenu();
            }
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }

        private void TogglePauseMenu()
        {
            isGamePaused = !isGamePaused;
            
            if (isGamePaused)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenuParent.SetActive(true);
                gameMessageText.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenuParent.SetActive(false);
                gameMessageText.SetActive(true);
            }
        }
    }
}

