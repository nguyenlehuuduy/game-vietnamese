    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Networking;
    using System.Collections;
    using UnityEngine.SceneManagement;
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Login1 : MonoBehaviour
    {
    public InputField usernameInput;
    public InputField passwordInput;
    public Button showPasswordButton;
    public Sprite showPasswordIcon;
    public Sprite hidePasswordIcon;
    public Text messageText;
    public Image imageMessage;
    public bool isPasswordVisible = false;

    public bool isLoginVisible = false;
    private void Start()
    {
        imageMessage.gameObject.SetActive(false);
    }
    public void Login()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        ConnectInternet connectInternet = new ConnectInternet();
        {
            connectInternet.CheckedInternet((callback) =>
            {
                if(callback == 1)
                {
                    //sendLoginRequestOffline();
                    messageText.text = "Hãy kết nối Internet trước khi đăng nhập";
                    imageMessage.gameObject.SetActive(true);
                    StartCoroutine(HideMessageAfterDelay(3f));
                }
                else
                {   
                    StartCoroutine(CheckLogin(username,password));
                }
            });
        }

    }
    public void ShowHidePass()
    {
    isPasswordVisible = !isPasswordVisible;
    if (isPasswordVisible)
    {
        passwordInput.contentType = InputField.ContentType.Standard;
        showPasswordButton.image.sprite = showPasswordIcon;
    }
    else
    {
        passwordInput.contentType = InputField.ContentType.Password;
        showPasswordButton.image.sprite = hidePasswordIcon;

    }
    passwordInput.ForceLabelUpdate();
    }

    // Post data 
    private IEnumerator CheckLogin(string username, string password)
    {
       
    yield return CheckAPI(username, password);
    string url_login = "http://localhost:3000/admin/login";
    // Tiếp tục nếu không có lỗi
    WWWForm form = new WWWForm();
    form.AddField("username", username);
    form.AddField("password", password);

    using (UnityWebRequest www = UnityWebRequest.Post(url_login, form))
    {
        yield return www.SendWebRequest();
            string responseJson = www.downloadHandler.text;
            User user = JsonConvert.DeserializeObject<User>(responseJson);
            User.currentUser = user;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                if (string.IsNullOrEmpty(username))
                {
                    messageText.text = "Tên đăng nhập đang trống";
                }
                else
                {
                    messageText.text = "Mật khẩu đang trống";
                }

                imageMessage.gameObject.SetActive(true);
                StartCoroutine(HideMessageAfterDelay(3f));
                yield break; // Kết thúc hàm nếu có lỗi
            }
            if (user != null && user.username == username && user.password == password)
            {
                messageText.text = "Login successful";
              
                SceneManager.LoadScene("Home-map");
            }
            else
            {
                messageText.text = "Tên đăng nhập hoặc mật khẩu chưa chính xác.";
                imageMessage.gameObject.SetActive(true);
                StartCoroutine(HideMessageAfterDelay(3f));
            }
    }
    }
    // Hàm xử lí khi đăng nhập Online
    private IEnumerator CheckAPI(string username, string birth)
    {
        string url = "http://localhost:3000/admin/checkuserandbirth";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("birth", birth);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text == "available")
                {
                    isLoginVisible = true;

                }
                else if (www.downloadHandler.text == "not-available")
                {
                    messageText.text = "Tên người dùng hoặc ngày sinh không chính xác";
                    imageMessage.gameObject.SetActive(true);
                    StartCoroutine(HideMessageAfterDelay(3f));
                }

            }
            else
            {
                messageText.text = "Không thể kết nối đến máy chủ";
                imageMessage.gameObject.SetActive(true);
                StartCoroutine(HideMessageAfterDelay(3f));
            }
        }
    }
    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Đợi trong 3 giây
        messageText.text = ""; // Xóa nội dung thông báo sau khi đợi xong
        imageMessage.gameObject.SetActive(false);
    }
 }
