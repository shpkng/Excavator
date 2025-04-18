using System.Linq;
using UnityEngine;

public class CapsuleGeometry
{
    public Vector3 start;
    public Vector3 end;
    public float radius;
}

public class Excavator : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 1;
    private CapsuleGeometry[] geometries;
    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float acceleration = 2;
    [SerializeField] private float drag = 1;

    [SerializeField] private PlatformTool mainTool;

    private int layerMask = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        geometries = GetComponents<CapsuleCollider>().Select(i => new CapsuleGeometry
        {
            start = i.center - (i.height * 0.5f - i.radius) * Vector3.right,
            end = i.center + (i.height * 0.5f - i.radius) * Vector3.right,
            radius = i.radius
        }).ToArray();
        layerMask = LayerMask.GetMask("Terrain");
    }

    private int contacts = 0;

    private void FixedUpdate()
    {
        // var heightVec = 0.5f * transform.right;

        contacts = geometries.Count(g => Physics.CheckCapsule(transform.TransformPoint(g.start),
            transform.TransformPoint(g.end), g.radius * 1.05f, layerMask));
        // var e = geometries[0];
        // foreach (var raycastHit in Physics.CapsuleCastAll(transform.TransformPoint(e.start),
        //              transform.TransformPoint(e.end),
        //              e.radius, Vector3.forward, 0))
        // {
        //     Debug.Log(raycastHit.collider.name);
        // }
    }

    private void Update()
    {
        // 不管任何情况，速度都会衰减
        var delta = drag * Time.deltaTime;
        if (delta <= Mathf.Abs(speed))
        {
            speed -= Mathf.Sign(speed) * delta;
        }
        else
        {
            speed = 0;
        }

        if (contacts == 0)
        {
            // Debug.Log("No contacts");
            return;
        }

        // Debug.Log(contacts);

        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        speed = Mathf.Clamp(speed + v * (0.5f * contacts) * acceleration * Time.deltaTime, -maxSpeed, maxSpeed);

        var direction = new Vector3(0, 0, speed * Time.deltaTime);

        transform.Rotate(transform.up, h * Mathf.Sign(v) * 30 * Time.deltaTime);

        transform.Translate(direction, Space.Self);
    }
}