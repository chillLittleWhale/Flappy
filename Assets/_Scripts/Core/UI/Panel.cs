using UnityEngine;

namespace AjaxNguyen.Core.UI
{
    public class Panel : MonoBehaviour
    {
        [SerializeField] string id;
        [SerializeField] bool isInitialized = false;
        [SerializeField] bool isOpening = false;

        [SerializeField] RectTransform container = null;

        public string ID {get {return id;}}
        public bool IsInitialized {get {return isInitialized;}}
        public bool IsOpening {get {return isOpening;}}

        public virtual void Awake()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            if (isInitialized) return;

            isInitialized = true;
            Close();
        }

        public virtual void Open()
        {
            if(isInitialized) Initialize();
            if (isOpening) return;

            transform.SetAsLastSibling();  // cho xuống cuối để nó được render cao nhất
            container.gameObject.SetActive(true);
            isOpening = true;
        }

        public virtual void Close()
        {
            if(isInitialized) Initialize();
            if (!isOpening) return;

            container.gameObject.SetActive(false);
            isOpening = false;
        }
    }
}
