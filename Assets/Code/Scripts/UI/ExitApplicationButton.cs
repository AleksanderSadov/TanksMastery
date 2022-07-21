using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitApplicationButton : MonoBehaviour
{
    private void Start()
    {
        #if (UNITY_EDITOR || UNITY_STANDALONE)
            gameObject.SetActive(true);
        #elif UNITY_WEBGL
            gameObject.SetActive(false);
        #endif
    }

    private void Update()
    {
        if (
            EventSystem.current.currentSelectedGameObject == gameObject
            && Input.GetButtonDown(GameConstants.BUTTON_NAME_SUBMIT)
        )
        {
            ExitApplication();
        }
    }

    public void ExitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            Application.Quit();
        #endif
    }
}