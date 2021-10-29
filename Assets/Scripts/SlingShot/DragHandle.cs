using UnityEngine;

public class DragHandle : MonoBehaviour
{
    [SerializeField] float minXPos = -5f;
    [SerializeField] float maxXPos = 5f;

    [SerializeField] float minZPos = -5f;
    [SerializeField] float maxZPos = 5f;

    Vector3 startPos;

    Vector3 startDragPos;
    Vector3 dist;

    bool isDragging = false;

    [SerializeField] Transform car;

    public static event OnDragHandleReleaseDelegate OnDragHandleReleaseEvent;
    public static event System.Action OnDragStartedEvent;

    private void Awake()
    {
        startPos = this.transform.localPosition;
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        EditorInput();
#else
        MobileInput();
#endif
    }

    void EditorInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform == this.transform)
            {
                GameManager.Instance.DragEventText("Is Hit");
                isDragging = true;

                startDragPos = Camera.main.WorldToScreenPoint(transform.position);
                dist = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0.55f, Input.mousePosition.y));

                OnDragStartedEvent?.Invoke();
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            GameManager.Instance.DragEventText("Is Dragging");

            Vector3 lastPos = new Vector3(Input.mousePosition.x, 0.55f, Input.mousePosition.y);
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(lastPos) + dist;

            Vector3 dir = targetPos - transform.position;
            float directionMagnitude = dir.magnitude;
            Vector3.Normalize(dir);

            car.transform.Rotate(car.up, -Vector3.Dot(transform.right, dir) * 0.0025f, Space.World);

            float xMovement = dir.x * directionMagnitude * 0.000001f;
            float zMovement = dir.z * directionMagnitude * 0.000001f;

            // clamp desiredPosition to match movement restrictions
            // find correct x and z values to match your desired clamp values
            Vector3 desiredPosition = new Vector3(transform.position.x + xMovement, transform.position.y, transform.position.z + zMovement);
            //Vector3 desiredClampedPosition = new Vector3(Mathf.Clamp(transform.position.x + xMovement, minXPos, maxXPos), transform.position.y, Mathf.Clamp(transform.position.z + zMovement, minZPos, maxZPos));
            transform.position = desiredPosition;

            if (Vector3.Distance(transform.position, car.transform.position) > 0.05f)
            {
                car.position += new Vector3(xMovement * .9f, 0f, zMovement * .9f);
            }
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            if (OnDragHandleReleaseEvent != null)
            {
                OnDragHandleReleaseEvent.Invoke();
            }

            ResetPosition();

            GameManager.Instance.DragEventText("Is Shooting");
        }
    }

    void MobileInput()
    {
        if (Input.touchCount != 1)
        {
            isDragging = false;
            return;
        }

        //We check if we have more than one touch happening.
        //We also check if the first touches phase is Ended (that the finger was lifted)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit) && hit.transform == this.transform)
                {
                    GameManager.Instance.DragEventText("Is Hit");
                    isDragging = true;

                    startDragPos = Camera.main.WorldToScreenPoint(transform.position);
                    dist = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, 0.55f, touch.position.y));

                    OnDragStartedEvent?.Invoke();
                }
            }

            if (isDragging && touch.phase == TouchPhase.Moved)
            {
                GameManager.Instance.DragEventText("Is Dragging");

                Vector3 lastPos = new Vector3(touch.position.x, 0.55f, touch.position.y);

                Vector3 targetPos = Camera.main.ScreenToWorldPoint(lastPos) + dist;

                Vector3 dir = targetPos - transform.position;
                float directionMagnitude = dir.magnitude;
                Vector3.Normalize(dir);

                car.transform.Rotate(car.up, -Vector3.Dot(transform.right, dir) * 0.0025f, Space.World);

                float xMovement = dir.x * directionMagnitude * 0.000001f;
                float zMovement = dir.z * directionMagnitude * 0.000001f;

                // clamp desiredPosition to match desired movement restrictions
                // find correct x and z values to match your desired clamp values
                Vector3 desiredPosition = new Vector3(transform.position.x + xMovement, transform.position.y, transform.position.z + zMovement);
                //Vector3 desiredClampedPosition = new Vector3(Mathf.Clamp(transform.position.x + xMovement, minXPos, maxXPos), transform.position.y, Mathf.Clamp(transform.position.z + zMovement, minZPos, maxZPos));
                transform.position = desiredPosition;

                if (Vector3.Distance(transform.position, car.transform.position) > 0.05f)
                {
                    car.position += new Vector3(xMovement * .9f, 0f, zMovement * .9f);
                }

            }

            if (isDragging)
            {
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isDragging = false;

                    if (OnDragHandleReleaseEvent != null)
                    {
                        OnDragHandleReleaseEvent.Invoke();
                    }

                    ResetPosition();

                    GameManager.Instance.DragEventText("Is Shooting");
                }
            }
        }
    }

/*    private void OnMouseDown()
    {
        GameManager.Instance.DragEventText("On Mouse Down");
        startDragPos = Camera.main.WorldToScreenPoint(transform.position);
        dist = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0.55f, Input.mousePosition.y));

        OnDragStartedEvent?.Invoke();
    }

    private void OnMouseDrag()
    {
        GameManager.Instance.DragEventText("Is Dragging");

        Vector3 lastPos = new Vector3(Input.mousePosition.x, 0.55f, Input.mousePosition.y);

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(lastPos) + dist;

        Vector3 dir = targetPos - transform.position;
        float directionMagnitude = dir.magnitude;
        Vector3.Normalize(dir);

        car.transform.Rotate(car.up, -Vector3.Dot(transform.right, dir) * 0.0025f, Space.World);

        float xMovement = dir.x * directionMagnitude * 0.000001f;
        float zMovement = dir.z * directionMagnitude * 0.000001f;

        var tempPos = new Vector3(xMovement, 0f, zMovement);
        tempPos.z = Mathf.Clamp(tempPos.z, minZPos, maxZPos);

        transform.position += new Vector3(xMovement, 0f, zMovement);

        if (Vector3.Distance(transform.position, car.transform.position) > 0.05f)
        {
            car.position += new Vector3(xMovement * .9f, 0f, zMovement * .9f);
        }
    }

    private void OnMouseUp()
    {
        if (OnDragHandleReleaseEvent != null)
        {
            OnDragHandleReleaseEvent.Invoke();
        }

        ResetPosition();

        GameManager.Instance.DragEventText("Is Shooting");
    }
*/

    public void ResetPosition()
    {
        this.transform.localPosition = startPos;
    }
}