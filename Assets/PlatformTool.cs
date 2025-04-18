using System;
using UnityEngine;

public class PlatformTool : MonoBehaviour
{
    protected Excavator platform;

    protected void Init(Excavator platform)
    {
        this.platform = platform;
    }

    public virtual bool IsPositionReachable(Vector3 position)
    {
        return false;
    }

    public virtual void Execute(Vector3 position)
    {
    }
}