using System.Collections;
using System.Collections.Generic;
using AjaxNguyen.Core.UI;
using AjaxNguyen.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flappy.Core.Manager
{
    public class PanelManager : PersistentSingleton<PanelManager>
    {
        private Dictionary<PanelType, Panel> panels = new Dictionary<PanelType, Panel>();
        private bool isInitialized = false;
        private Canvas[] canvas = null;


        protected override void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += OnSceneLoaded;
            Initialize();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            isInitialized = false;
            Initialize(); // Cập nhật lại Dictionary khi scene load
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; // Hủy đăng ký khi bị destroy
        }

        private void Initialize()
        {
            if (isInitialized) return;
            isInitialized = true;

            panels.Clear();
            canvas = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            if (canvas == null)
            {
                Debug.LogWarning("No canvas found in scene");
                return;
            }

            for (int i = 0; i < canvas.Length; i++)
            {
                Panel[] panelList = canvas[i].GetComponentsInChildren<Panel>(true);
                if (panelList != null)
                {
                    for (int j = 0; j < panelList.Length; j++)
                    {
                        if (!panels.ContainsKey(panelList[j].ID)) //!string.IsNullOrEmpty(panelList[j].ID) && 
                        {
                            panelList[j].Initialize();
                            panels.Add(panelList[j].ID, panelList[j]);
                        }
                    }
                }
            }
        }

        public Panel GetPanel(PanelType panelID)
        {
            return Instance.panels.ContainsKey(panelID) ? Instance.panels[panelID] : null;
        }

        public void OpenPanel(PanelType panelID)
        {
            Panel panel = GetPanel(panelID);
            if (panel != null)
            {
                panel.Open();
            }
            else
            {
                Debug.LogWarning("Panel with ID: " + panelID + " not found.");
            }
        }

        public void ClosePanel(PanelType panelID)
        {
            Panel panel = GetPanel(panelID);
            if (panel != null)
            {
                panel.Close();
            }
            else
            {
                Debug.LogWarning("Panel with ID: " + panelID + " not found.");
            }
        }

        public void CloseAllPanels()
        {
            foreach (Panel panel in Instance.panels.Values)
            {
                panel.Close();
            }
        }

        public static bool IsPanelOpen(PanelType panelID)
        {
            if (Instance.panels.ContainsKey(panelID))
            {
                return Instance.panels[panelID].IsOpening;
            }

            else
            {
                Debug.LogWarning("Panel with ID: " + panelID + " not found.");
                return false;
            }
        }

        public void ShowErrorPopup(ErrorPopup.Action action = ErrorPopup.Action.None, string message = "lmao")
        {
            ErrorPopup errorPopup = (ErrorPopup)GetPanel(PanelType.ErrorPopup);
            errorPopup.Open(action, message);
        }
    }
}
