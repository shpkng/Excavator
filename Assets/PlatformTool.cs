using System;
using UnityEngine;

public class PlatformTool : MonoBehaviour
{
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