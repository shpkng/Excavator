using System;
using UnityEngine;

[Flags]
public enum InteractionType
{
    None = 0,
    Break = 0x1,
    Carry = 0x10,
    GetLiquid = 0x100, // 吸取液体，如水、石油
    Flush = 0x1000,
    Dock = 0x10000, // 停靠在元素上，如传送带、电梯🛗
}

/// <summary>
/// 场景中的可交互物体
/// </summary>
public class EnvElement : MonoBehaviour
{
    [SerializeField] private Transform[] interactionPoints;
    public InteractionType allowedInteractions = InteractionType.None;

    public virtual void Interact(InteractionType interactionType)
    {
        if (!allowedInteractions.HasFlag(interactionType))
        {
            Debug.LogError("Interaction type not allowed");
            return;
        }
    }
}