using System;
using System.Collections.Generic;
using DG.Tweening;
using R3;
using UnityEngine;

public class BucketTool : BSBSystemTool
{
    private bool isReleased;

    private HashSet<EnvElement> elements;
    private IDisposable scanDisposable;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Execute();
        }
    }

    public override void Execute()
    {
        if (isReleased)
        {
            transform.localEulerAngles = new Vector3(-135, 0, 0);
            transform.DOLocalRotate(new Vector3(-45, 0, 0), 1);
        }
        else
        {
            transform.localEulerAngles = new Vector3(-45, 0, 0);
            transform.DOLocalRotate(new Vector3(-135, 0, 0), 1);
        }

        isReleased = !isReleased;
    }
   
    private void OnEnable()
    {
        scanDisposable?.Dispose();
        scanDisposable = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(Scan);
    }

    private void OnDisable()
    {
        scanDisposable.Dispose();
        scanDisposable = null;
    }

    // 扫描可交互物体并更新UI
    private void Scan(Unit c)
    {
    }
} 