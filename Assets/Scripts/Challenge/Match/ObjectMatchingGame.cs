/* using UnityEngine;

// Yêu cầu phải có 2 component này, nếu không thì sẽ tự tạo

public class ObjectMatchingGame : MonoBehaviour
{
    private LineRenderer lineRenderer; // vẽ đường nối
    [SerializeField] private int matchId;
    private bool isDragging; // check xem người chơi có đang kéo thả hay không 
    private Vector3 endPoint; // lưu trữ vị trí cuối cùng khi người dùng thả ra
    private ObjectMatchForm objectMatchForm; // kiểm ra xem nó có match đúng với id mà mình đã vạch ra

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }
    private bool allMatchesCompleted = false; // Biến để theo dõi trạng thái của việc match

    private void Update()
    {
        if (!allMatchesCompleted) // Kiểm tra xem đã match hết tất cả đối tượng chưa
        {
            if (Input.GetMouseButtonDown(0)) // Kiểm tra xem người chơi có bấm màn hình cảm ứng hay chưa
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0f;
                    lineRenderer.SetPosition(0, mousePosition);
                }
            }
            if (isDragging)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                lineRenderer.SetPosition(1, mousePosition);
                endPoint = mousePosition;
            }
            if (Input.GetMouseButtonUp(0)) // kiểm tra xem người chơi đã thả cảm ứng ra chưa
            {
                isDragging = false;
                RaycastHit2D hit = Physics2D.Raycast(endPoint, Vector2.zero);
                if (hit.collider != null && hit.collider.TryGetComponent(out objectMatchForm) && matchId == objectMatchForm.Get_ID())
                {
                    CheckAllMatches(); // Kiểm tra xem đã match hết tất cả đối tượng chưa
                }
                else
                {
                    lineRenderer.positionCount = 0;
                }

                lineRenderer.positionCount = 2;
            }
        }
    }

    private void CheckAllMatches()
    {
        // Kiểm tra xem tất cả các đối tượng có matchId đã được match chưa
        bool allMatches = true;

        // Lặp qua tất cả các đối tượng để kiểm tra match
        foreach (var matchableObject in FindObjectsOfType<ObjectMatchingGame>())
        {
            if (matchableObject.matchId != matchableObject.objectMatchForm.Get_ID())
            {
                allMatches = false; // Nếu có ít nhất một đối tượng không khớp, đặt biến này thành false và thoát khỏi vòng lặp
                break;

            }
        }

        if (allMatches)
        {
            Debug.Log("Correct Form!");
            allMatchesCompleted = true; // Nếu đã match hết tất cả đối tượng, đặt biến này thành true để ngừng kiểm tra và thông báo "Correct Form!"
            this.enabled = false;
        }
        else
        {
        }
    }

} */

using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class ObjectMatchingGame : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private int matchId;
    private bool isDragging;
    private Vector3 endPoint;
    private ObjectMatchForm objectMatchForm;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                lineRenderer.SetPosition(0, mousePosition);
            }
        }
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            lineRenderer.SetPosition(1, mousePosition);
            endPoint = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            RaycastHit2D hit = Physics2D.Raycast(endPoint, Vector2.zero);
            if (hit.collider != null && hit.collider.TryGetComponent(out objectMatchForm) && matchId == objectMatchForm.Get_ID())
            {
                Debug.Log("Correct Form!");
                this.enabled = false;
            }
            else
            {
                lineRenderer.positionCount = 0;
            }

            lineRenderer.positionCount = 2;
        }
    }
}