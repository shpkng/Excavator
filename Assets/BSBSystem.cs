using System;
using UnityEngine;

// 挖掘臂
public class BSBSystem : PlatformTool
{
    [SerializeField] private Transform boom, stick, bucket;
    private int carrying = 0;
    private Camera mainCamera;
    private float radiusMin, radiusMax, heightMin, heightMax, rotMin, rotMax;
    private float radius, height, rotation;

    public override bool IsPositionReachable(Vector3 position)
    {
        return false;
    }

    public override void Execute(Vector3 position)
    {
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    }
}