using UnityEngine;

public class Sand : MonoBehaviour, ICollectible
{
    public int amount;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Tool"))
        {
            return;
        }

        var bucket = other.GetComponent<BucketTool>();
        if (!bucket)
        {
            return;
        }

        bucket.Collect(this);
    }
}