using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tanks.UI
{
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

        public void ExitApplication()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_STANDALONE
                Application.Quit();
            #endif
        }
    }
}
