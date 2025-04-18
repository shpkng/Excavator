using UnityEngine;

// 挖掘臂
public class BSBSystem : PlatformTool
{
    [SerializeField] private Transform boom, stick, bucket;
    private Excavator platform;
    private int carrying = 0;

    public void Init(Excavator platform)
    {
        this.platform = platform;
    }

    public override bool IsPositionReachable(Vector3 position)
    {
        return false;
    }

    public override void Execute(Vector3 position)
    {
    }
}