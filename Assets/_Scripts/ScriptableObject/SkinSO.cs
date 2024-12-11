using UnityEngine;

namespace AjaxNguyen.Core.SO
{

[CreateAssetMenu(fileName = "NewSkin", menuName = "New SO/Skin")]
public class SkinSO : ScriptableObject
{
    public int skinID;
    public string skinName;
    public Sprite skinIcon; 
    public bool isUnlocked; 
    public int unlockCost; 
}

}
