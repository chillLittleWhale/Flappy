using System;
using System.Collections;
using System.Collections.Generic;
using AjaxNguyen.Core.SO;
using UnityEngine;

namespace AjaxNguyen.Core
{
    [Serializable]
    public class Skin //: MonoBehaviour
    {
        public SkinSO skinData;
        public bool isUnlocked;

        public Skin(SkinSO data, bool isUnlocked)
        {
            skinData = data;
            this.isUnlocked = isUnlocked;   
        }
    }
}
