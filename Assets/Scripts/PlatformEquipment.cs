using UnityEngine;

public class PlatformEquipment : MonoBehaviour
{
    public InteractionType allowedInteractions = InteractionType.None;

    protected void Init(Excavator platform)
    {
        // this.platform = platform;
    }

    public void Mount()
    {
    }

    public void Unmount()
    {
    }

    public virtual bool IsPositionReachable(Vector3 position)
    {
        return false;
    }

    public virtual void Execute(Vector3 position)
    {
    }
}