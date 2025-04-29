using System;
using DG.Tweening;
using UnityEngine;

public class BucketTool : BSBSystemTool
{
    private static readonly int Blend = Animator.StringToHash("Blend");
    private bool isReleased;

    [SerializeField] private Animator _animator;
    [SerializeField] private int capacity;
    [SerializeField] private int amount;

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

    private void SetAmount(int f)
    {
        amount = f;
        _animator.SetFloat(Blend, 1f * amount / capacity);
    }

    public void Collect(ICollectible collectible)
    {
        switch (collectible)
        {
            case Sand sand:
            {
                if (amount + sand.amount <= capacity)
                {
                    SetAmount(sand.amount + amount);
                }

                Destroy(sand.gameObject);
            }
                break;
        }
    }
}