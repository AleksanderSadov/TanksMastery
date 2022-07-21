using Tanks.Shared;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.UI
{
    public class MenuNavigation : MonoBehaviour
    {
        public Selectable DefaultSelection;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void LateUpdate()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (
                    Input.GetButtonDown(GameConstants.BUTTON_NAME_SUBMIT)
                    || Input.GetAxisRaw(GameConstants.AXIS_NAME_HORIZONTAL_UI) != 0
                    || Input.GetAxisRaw(GameConstants.AXIS_NAME_VERTICAL_UI) != 0
                )
                {
                    EventSystem.current.SetSelectedGameObject(DefaultSelection.gameObject);
                }
            }
        }
    }
}
