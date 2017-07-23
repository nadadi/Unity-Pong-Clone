using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    [RequireComponent(typeof(Button))]
    public abstract class DynamicButton : MonoBehaviour
    {
        Button _myButton;

        void Awake()
        {
            _myButton = GetComponent<Button>();
            _myButton.onClick.AddListener(AssignFunction);
        }

        public abstract void AssignFunction();
    }
}

