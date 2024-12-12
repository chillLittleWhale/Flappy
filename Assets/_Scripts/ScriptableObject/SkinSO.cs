using UnityEngine;

namespace AjaxNguyen.Core.SO
{

[CreateAssetMenu(fileName = "NewSkin", menuName = "New SO/Skin")]
public class SkinSO : ScriptableObject
{
    public int id;
    public string skinName;
    public Sprite skinIcon; 
    public int unlockCost; 
}

}
