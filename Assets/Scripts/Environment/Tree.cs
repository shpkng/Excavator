using System;
using UnityEngine;


public class Tree : MonoBehaviour, ICollectible
{
    [SerializeField] private bool choppedDown = false;
    [SerializeField] private Rigidbody rb;

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Tool"))
        {
            return;
        }

        if (!choppedDown)
        {
            rb.isKinematic = false;
            rb.AddForceAtPosition(other.relativeVelocity, other.contacts[0].point, ForceMode.Impulse);
            choppedDown = true;
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