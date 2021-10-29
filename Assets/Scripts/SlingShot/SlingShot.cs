using UnityEngine;

public class SlingShot : MonoBehaviour
{
    [SerializeField] LineRenderer[] HandlesLineRenderers;
    [SerializeField] Transform[] HandleAnchorTrnsforms;
    [SerializeField] DragHandle DragHandle;

    [SerializeField] Transform AimerTransform;

    [SerializeField] Rigidbody car;
    [SerializeField] float startPower = 5;
    [SerializeField] float powerMult = 10f;

    [Space]

    [SerializeField]
    Vector3 dragHandleStartPositionOffset;

    bool canShoot = true;
 
    private float[] LineLengths;
    private Vector3 _offset;

    public float GetVelocity()
    {
        return Vector3.Distance(DragHandle.transform.position, car.transform.position) * powerMult;
    }

    public void MakeShot()
    {
        car.AddForce(car.transform.forward * startPower * powerMult, ForceMode.Impulse);
        canShoot = false;

        AimerTransform.gameObject.SetActive(false);
    }

    public float GetAngle()
    {
        var angle = Vector3.Angle((car.transform.position - DragHandle.transform.position).normalized, Vector3.right);

        if (DragHandle.transform.position.y > AimerTransform.position.y)
        {
            angle = angle * -1;
        }

        return angle;
    }

    private void Start()
    {
        LineLengths = new float[2];

        for (var i = 0; i < HandlesLineRenderers.Length; i++)
        {
            HandlesLineRenderers[i].SetPosition(0, HandleAnchorTrnsforms[i].position);
            HandlesLineRenderers[i].SetPosition(1, DragHandle.transform.position);
            HandlesLineRenderers[i].startWidth = 0.15f;
            HandlesLineRenderers[i].endWidth = 0.05f;
        }

        GameManager.Instance.OnGameReset += () =>
        {
            canShoot = true;
            AimerTransform.gameObject.SetActive(true);
        };
    }

    private void OnEnable()
    {
        DragHandle.OnDragHandleReleaseEvent += DragHandle_OnDragHandleReleaseEvent;
    }

    private void OnDisable()
    {
        DragHandle.OnDragHandleReleaseEvent -= DragHandle_OnDragHandleReleaseEvent;
        GameManager.Instance.OnGameReset -= () =>
        {
            canShoot = true;
            AimerTransform.gameObject.SetActive(true);
        };
    }

    private void OnDestroy()
    {
        DragHandle.OnDragHandleReleaseEvent -= DragHandle_OnDragHandleReleaseEvent;
    }

    private void DragHandle_OnDragHandleReleaseEvent()
    {
        Debug.Log("Shoot");
        MakeShot();
    }

    private void Update()
    {
        UpdateLines();

        if (Vector3.Distance(DragHandle.transform.position, car.transform.position) < 15f)
        {
            startPower = Vector3.Distance(DragHandle.transform.position, GameManager.Instance.StartPos) * 3f;
        }
    }

    private void UpdateLines()
    {
        for (var i = 0; i < HandlesLineRenderers.Length; i++)
        {
            HandlesLineRenderers[i].SetPosition(1, DragHandle.transform.position);
            HandlesLineRenderers[i].SetPosition(0, HandleAnchorTrnsforms[i].position);

            HandlesLineRenderers[i].GetComponent<LineRenderer>().startWidth = 0.15f / LineLengths[i];
            HandlesLineRenderers[i].GetComponent<LineRenderer>().endWidth = 0.05f / LineLengths[i];

            LineLengths[i] = Vector3.Distance(DragHandle.transform.position, HandleAnchorTrnsforms[i].position);

            if (LineLengths[i] <= 0.85f)
            {
                LineLengths[i] = 0.85f;
            }
        }
    }

    private Vector3 GetShotDirection()
    {
        return car.transform.forward;
    }
}