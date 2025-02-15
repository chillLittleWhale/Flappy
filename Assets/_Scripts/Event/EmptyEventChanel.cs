using UnityEngine;

namespace AjaxNguyen.Event
{
    public readonly struct Empty{}

    [CreateAssetMenu(menuName = "Event/EmptyEventChanel")]
    public class EmptyEventChanel : AbstractEventChanel<Empty>
    {
        
    }
}
