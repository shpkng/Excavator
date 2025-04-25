using UnityEngine;

public class BSBSystemTool : MonoBehaviour
{
    public InteractionType allowedInteractions = InteractionType.None;

    // 不管有没有目标都可以执行，等执行的时候再去找目标
    // 如果有UI需要提示交互，就在派生类里写
    public virtual void Execute()
    {
    }
}