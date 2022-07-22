using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.UI
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] public string scenePath;
        [SerializeField] public string sceneName;

        public void LoadScene()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
