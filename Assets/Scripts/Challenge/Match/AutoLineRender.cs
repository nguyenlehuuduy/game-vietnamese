using System.Collections;
using UnityEngine;

public class AutoLineRender : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int numberOfPoints = 10;
    public float animationSpeed = 0.1f;

    void Start()
    {
        // Khởi tạo Line Renderer và đặt các thuộc tính cơ bản
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numberOfPoints;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Gọi coroutine để vẽ đường từ từ
        StartCoroutine(DrawLineCoroutine());
    }

    IEnumerator DrawLineCoroutine()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            // Logic vẽ đường ở đây, có thể thay đổi tùy thuộc vào yêu cầu của bạn
            float x = i * 0.2f;
            float y = Mathf.Sin(x); // Điều chỉnh logic vẽ tùy thuộc vào nhu cầu của bạn

            // Đặt vị trí của mỗi điểm trong đường
            Vector3 pointPosition = new Vector3(x, y, 0f);
            lineRenderer.SetPosition(i, pointPosition);

            // Chờ một khoảng thời gian trước khi vẽ điểm tiếp theo
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
