using System.Collections;
using System.Collections.Generic;
using AjaxNguyen.Core.UI;
using AjaxNguyen.Utility;
using UnityEngine;

namespace AjaxNguyen.Core.Manager
{
    public class PanelManager : PersistentSingleton<PanelManager>
    {
        private Dictionary<string, Panel> panels = new Dictionary<string, Panel>();
        private bool isInitialized = false;
        private Canvas[] canvas = null;


        protected override void Awake()
        {
            base.Awake();
            Initialize();

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
                        if (!string.IsNullOrEmpty(panelList[j].ID) && !panels.ContainsKey(panelList[j].ID))
                        {
                            panelList[j].Initialize();
                            panels.Add(panelList[j].ID, panelList[j]);
                        }
                    }
                }
            }
        }

        public Panel GetPanel(string panelID)
        {
            return Instance.panels.ContainsKey(panelID) ? Instance.panels[panelID] : null;
        }

        public void OpenPanel(string panelID)
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

        public void ClosePanel(string panelID)
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

        public static bool IsPanelOpen(string panelID)
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
            ErrorPopup errorPopup = (ErrorPopup) GetPanel("error");
            errorPopup.Open(action, message);
        }
    }
}
