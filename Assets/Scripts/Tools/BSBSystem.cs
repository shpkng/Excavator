using Shapes;
using UnityEngine;

public enum Mode
{
    Bucket = 0,
    Breaker = 1,
    Sucker = 2,
    ToolModeCount
}

// 挖掘臂
public class BSBSystem : PlatformEquipment
{
    [SerializeField] private Transform boom, stick;
    private int carrying = 0;

    private Camera mainCamera;

    [SerializeField] private BSBSystemTool toolBucket,
        toolSucker,
        toolBreaker;

    private BSBSystemTool currentTool;

    [SerializeField] private Mode mode;
    [SerializeField] private float speedRot, speedPitch, speedBucketRot;
    [SerializeField] private float lengthBoom, lengthStick;
    [SerializeField] private float maxAngle;
    [SerializeField] private float lengthMax, lengthMin;
    [SerializeField] private float height;
    [SerializeField] private float rangeMin, rangeMax;
    [SerializeField] private Disc disc;
    [SerializeField] private Transform mouseIndicator;

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
        Set();
    }

    [ContextMenu("Set")]
    private void Set()
    {
        lengthMax = lengthBoom + lengthStick;
        lengthMin = lengthBoom - lengthStick;
        rangeMax = Mathf.Sqrt(lengthMax * lengthMax - height * height);
        rangeMin = Mathf.Sqrt(lengthMin * lengthMin - height * height);
        disc.AngRadiansStart = (90 - maxAngle) * Mathf.Deg2Rad;
        disc.AngRadiansEnd = (90 + maxAngle) * Mathf.Deg2Rad;
    }

    private void MouseInput()
    {
        var deltaTime = Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
        {
            height = Mathf.Clamp(height - 0.5f * deltaTime, -1, 1);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            height = Mathf.Clamp(height + 0.5f * deltaTime, -1, 1);
        }

        var transformDown = -transform.up;

        // 设置圆盘位置
        disc.transform.localPosition = transform.localPosition + height * transformDown;
        // 设置圆盘旋转
        disc.transform.localEulerAngles = new Vector3(90, 0, 0);
        disc.Radius = (rangeMax + rangeMin) / 2;
        disc.Thickness = rangeMax - rangeMin;

        // 鼠标位置
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(transformDown, disc.transform.position);
        plane.Raycast(ray, out var enter);
        var mousePoint = ray.GetPoint(enter);
        var distanceToMousePoint = (mousePoint - transform.position).magnitude;
        mouseIndicator.position = mousePoint;

        var rayToCalculateShadow = new Ray(mousePoint, transformDown);
        var shadowPlane = new Plane(transformDown, transform.position);
        // enter 可以是负数
        shadowPlane.Raycast(rayToCalculateShadow, out enter);
        // 鼠标位置在挖掘臂原点平面上的投影
        var shadowPoint = rayToCalculateShadow.GetPoint(enter);
        var directionToShadowPoint = shadowPoint - transform.position;
        var angle = Mathf.Clamp(Vector3.SignedAngle(transform.parent.forward, directionToShadowPoint, transform.up),
            -maxAngle,
            maxAngle);
        transform.localEulerAngles = new Vector3(0, angle, 0);

        // 挖掘臂终端到原点距离
        var length = Vector3.Distance(mousePoint, transform.position);
        length = Mathf.Clamp(length, rangeMin, rangeMax);

        var angleDistanceForward = Mathf.Atan(height / distanceToMousePoint) * Mathf.Rad2Deg;
        var angleBoomDistance = Mathf.Acos((lengthBoom * lengthBoom + length * length - lengthStick * lengthStick) /
                                           (2 * lengthBoom * length)) * Mathf.Rad2Deg;
        var angleBoomStick = Mathf.Acos((lengthBoom * lengthBoom + lengthStick * lengthStick - length * length) /
                                        (2 * lengthBoom * lengthStick)) * Mathf.Rad2Deg;
        // Debug.Log($"{angleBoomDistance},{angleDistanceForward},{angleBoomStick}");
        boom.localEulerAngles = new Vector3(-(angleBoomDistance - angleDistanceForward), 0, 0);
        stick.localEulerAngles = new Vector3(-angleBoomStick, 0, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            mode = (Mode)(((int)mode + 1) % (int)Mode.ToolModeCount);
        }

        if (true)
        {
            MouseInput();
        }
        else
        {
            JoystickInput();
        }
    }

    private void JoystickInput()
    {
    }
}