using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

public class Report : MonoBehaviour
{
    public InputField reportInput;

    public void SubmitReport()
    {
        string reportContent = reportInput.text;

        if (User.currentUser != null)
        {
            string userId = User.currentUser._id;
            StartCoroutine(SendReport(reportContent, userId));
        }
        else
        {
            Debug.LogError("Người dùng không hợp lệ. Đăng nhập trước khi gửi báo cáo.");
        }
    }

    IEnumerator SendReport(string reportContent, string userId)
    {
        string url = "http://localhost:3000/admin/add-new-report";

        ReportModel report = new ReportModel
        {
            reportcontent = reportContent,
            idUser = userId
        };

        string json = JsonConvert.SerializeObject(report);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Lỗi khi gửi báo cáo: " + www.error);
            }
            else
            {
                Debug.Log("Báo cáo đã được gửi thành công");
            }
        }
    }
}

