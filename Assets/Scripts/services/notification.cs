using UnityEngine;
using UnityEngine.UI;

public class notification : MonoBehaviour
{
    public Text notificationText;
    public Image notificationImage;
    public float notificationDisplayTime = 2.0f;

    private bool isDisplayingNotification = false;
    private float notificationTimer = 0.0f;

    public void ShowNotification(string message, Sprite image)
    {
        notificationText.text = message;
        notificationImage.sprite = image;
        isDisplayingNotification = true;
        notificationTimer = 0;
    }

    private void Update()
    {
        if (isDisplayingNotification)
        {
            notificationTimer += Time.deltaTime;

            if (notificationTimer >= notificationDisplayTime)
            {
                isDisplayingNotification = false;
                notificationText.text = "";
                notificationImage.sprite = null;
            }
        }
    }
}
