using UnityEngine;

public class Battery : MonoBehaviour, ICollectible
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Tool"))
        {
            return;
        }

        var plier = other.gameObject.GetComponent<PlierTool>();
        if (!plier)
        {
            return;
        }

        plier.AddElement(this);
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("Tool"))
        {
            return;
        }

        var plier = other.gameObject.GetComponent<PlierTool>();
        if (!plier)
        {
            return;
        }

        plier.RemoveElement(this);
    }
}