using System;
using UnityEngine;

namespace AjaxNguyen.Core.UI
{

    [Serializable]
    public enum PanelType
    {
        Loading,  //0
        MainMenu, Setting, PlayerInfor,  // 1 2 3
        SkinSelect, MapSelect, Gacha, Shop,  // 4 5 6 7
        Ranking, Reward, Inbox, Quest, DailyLogin, // 8 9 10 11
        Authen, // 12
        ErrorPopup //13
    }

    public class Panel : MonoBehaviour
    {
        [SerializeField] PanelType id;
        [SerializeField] bool isInitialized = false;
        [SerializeField] bool isOpening = false;
        [SerializeField] RectTransform container = null;

        public PanelType ID { get { return id; } }
        public bool IsInitialized { get { return isInitialized; } }
        public bool IsOpening { get { return isOpening; } }

        public virtual void Awake()
        {
            container = transform.Find("Container").GetComponent<RectTransform>();
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
            if (isInitialized) Initialize();
            if (isOpening) return;

            transform.SetAsLastSibling();  // cho xuống cuối để nó được render cao nhất
            container.gameObject.SetActive(true);
            OnShow();
            isOpening = true;
        }

        public virtual void Close()
        {
            if (isInitialized) Initialize();
            if (!isOpening) return;

            container.gameObject.SetActive(false);
            isOpening = false;
        }

        protected virtual void OnShow() { } // thay thế cho OnEnable của Panel
    }
}
