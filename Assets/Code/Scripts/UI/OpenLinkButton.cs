using UnityEngine;

namespace Tanks.UI
{
    public class OpenLinkButton : MonoBehaviour
    {
        public string linkUrl = "";

        public void LoadTargetUrl()
        {
            Application.OpenURL(linkUrl);
        }
    }
}