using UnityEngine;

namespace Tanks.UI
{
    public class UIDirectionControl : MonoBehaviour
    {
        public bool useRelativeRotation = true;

        private Quaternion relativeRotation;

        private void Start()
        {
            relativeRotation = transform.parent.localRotation;
        }

        private void Update()
        {
            if (useRelativeRotation)
            {
                transform.rotation = relativeRotation;
            }
        }
    }
}
