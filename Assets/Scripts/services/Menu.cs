using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject confirmpanel;
    public Button button1;
    public Button button2;
    public Button button3;
    public Text profilename;
    public Text profilesex;
    public Text profileBirth;
    User currentUser = User.currentUser;
    private Color normalColor = Color.gray;
    private Color selectedColor = Color.white;

    private void Start()
    {
        ShowPanel1();
        confirmpanel.gameObject.SetActive(false);
    }
    
    // Các hàm để hiển thị/ẩn panel
    public void ShowPanel1()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);
        // Thiết lập màu sắc cho các button
        SelectButton(button1);
    }

    public void ShowPanel2()
    {
        profilename.text = currentUser.name;
        profilesex.text = currentUser.sex;
        profileBirth.text = currentUser.birth;
        panel1.SetActive(false);
        panel2.SetActive(true);
        panel3.SetActive(false);
        // Thiết lập màu sắc cho các button
        SelectButton(button2);

    }

    public void ShowPanel3()
    {
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(true);
        // Thiết lập màu sắc cho các button
        SelectButton(button3);
    }

    public void Show_Delete()
    {
        confirmpanel.gameObject.SetActive(true);
    }
    public void Close()
    {
        confirmpanel.gameObject.SetActive(false);
    }
    public void Delete_User()
    {
        StartCoroutine(DeleteAccount());
    }
    // Hàm để thiết lập màu sắc cho button
    private void SelectButton(Button selectedButton)
    {
        // Thiết lập màu sắc cho button được chọn
        selectedButton.image.color = selectedColor;

        // Thiết lập màu sắc cho các button không được chọn
        if (selectedButton != button1)
            button1.image.color = normalColor;

        if (selectedButton != button2)
            button2.image.color = normalColor;

        if (selectedButton != button3)
            button3.image.color = normalColor;
    }
    private IEnumerator DeleteAccount()
    {
        string url = "http://localhost:3000/admin/delete-user/" + currentUser._id;

        using (UnityWebRequest www = UnityWebRequest.Delete(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Lỗi khi xóa tài khoản: " + www.error);
            }
            else
            {
                Debug.Log("Xóa tài khoản thành công");
                User.currentUser = null;
                SceneManager.LoadScene("Welcome");
            }
        }
    }

}
