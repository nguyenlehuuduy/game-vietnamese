using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ForgetPass : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField newpassInputField;
    public InputField enteragainpassInputField;
    public Button ChangePassButton;
    public Button showNewPasswordButton;
    public Button showEnterAgainPasswordButton;
    public Sprite showPasswordIcon;
    public Sprite hidePasswordIcon;
    public Dropdown dayDropdown;
    public Dropdown monthDropdown;
    public Dropdown yearDropdown;
    public Image showiconpass;
    public Image showiconpassagain;
    public Text messageText;
    public Image imageMessage;
    private bool isAnimatingBuon = false;
    public Animator maleAnimator;
    public bool isPasswordVisible = false;
    string apiUpdate = "http://localhost:3000/admin/updatepass";
    private void Start()
    {
        newpassInputField.gameObject.SetActive(false);
        enteragainpassInputField.gameObject.SetActive(false);
        ChangePassButton.gameObject.SetActive(false);
        showNewPasswordButton.gameObject.SetActive(false);
        showEnterAgainPasswordButton.gameObject.SetActive(false);
        showiconpass.gameObject.SetActive(false);
        showiconpassagain.gameObject.SetActive(false);
        imageMessage.gameObject.SetActive(false);
        ChangePassButton.onClick.AddListener(ChangePasswordButtonClicked);

        ConnectInternet connectInternet = new ConnectInternet();
        {
            connectInternet.CheckedInternet((callback) =>
            {
                if (callback == 1)
                {
                    messageText.text = "Bạn cần phải có Internet để cập nhật mật khẩu";
                }
                else
                {
                    ChangePassButton.onClick.AddListener(ChangePasswordButtonClicked);
                }
            });
        }
    }
     
    public void ChangePasswordButtonClicked()
    {
        string newPassword = newpassInputField.text;
        string enterpassAgain = enteragainpassInputField.text;
        if (string.IsNullOrEmpty(newPassword))
        {
            messageText.text = "Nhập mật khẩu mới không được bỏ trống";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
        }
        else if (string.IsNullOrEmpty(enterpassAgain))
        {
            messageText.text = "Nhập lại mật khẩu mới không được bỏ trống";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
        }
        else if (newPassword != enterpassAgain)
        {
            messageText.text = "Mật khẩu và nhập lại mật khẩu không giống nhau";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
        }
        else if (newPassword.Length < 6 || newPassword.Length > 20)
        {
            messageText.text = "Mật khẩu không quá ngắn dưới 6 kí tự và dài hơn 20 kí tự.";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
        }
        else if (newPassword.Contains(" "))
        {
            messageText.text = "Mật khẩu không chứa khoảng trắng.";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
        }
        else if (newPassword != RemoveDiacritics(newPassword))
        {
            messageText.text = "Mật khẩu không chứa dấu câu.";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
        }
        else if (!Regex.IsMatch(newPassword, "(?=.*[A-Z])(?=.*[a-z])"))
        {
            messageText.text = "Mật khẩu phải chứa ít nhất 1 ký tự hoa và 1 ký tự thường.";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
        }
        else
        {
            StartCoroutine(UpdatePasswordAndUserInfo(newPassword));
            
        }

        StartCoroutine(HideMessageAfterDelay(3f));
    }
    // Hàm xử lí Show Animation
    private void DisableAnimator()
    {
        maleAnimator.enabled = false;
        maleAnimator.Play("AnimChao");
    }
    //Hàm ẩn image and text message
  
    private IEnumerator UpdatePasswordAndUserInfo(string newPassword)
    {
        //Thực hiện hàm SendPasswordUpdateRequest trước
        yield return StartCoroutine(SendPasswordUpdateRequest(newPassword));
        // Sau khi thực hiện xong SendPasswordUpdateRequest thì sẽ đến InforUserAfterUpdate();
      //  yield return StartCoroutine(InforUserAfterUpdate());
    }

    // Button onclick check
    public void ForgetPassUpdate()
    {
        string username = usernameInputField.text;
        string dateofBirth = GetDateOfBirth(); 
        StartCoroutine(CheckInformationUpdate(username, dateofBirth));
    }
  
    //Hàm cập nhật mật khẩu
    private IEnumerator SendPasswordUpdateRequest(string newPassword)
    {

        string username = usernameInputField.text;

        // Tạo một đối tượng JSON để gửi dữ liệu cập nhật mật khẩu
        User updateData = new User
        {
            username = username,
            newPassword = newPassword
        };

        string jsonData = JsonUtility.ToJson(updateData);

        using (UnityWebRequest www = new UnityWebRequest(apiUpdate, "PUT"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                messageText.text = "Mật khẩu đã được cập nhật thành công";
                imageMessage.gameObject.SetActive(true);
                StartCoroutine(HideMessageAfterDelay(3f));
                maleAnimator.Play("AnimNhay");
            }
            else
            {
                messageText.text = "Lỗi cập nhật mật khẩu.";
                imageMessage.gameObject.SetActive(true);
                StartCoroutine(HideMessageAfterDelay(3f));
                maleAnimator.Play("AnimBuon_anim");
            }
        }
    }

    private bool information = false;
    private IEnumerator CheckInformationUpdate(string username, string birth)
    {
        yield return CheckAPI(username, birth);
        if (information)
        {
            messageText.text = "Đổi mật khẩu nào thôi nào";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimNhay");
            Invoke("DisableAnimator", 100f);
            newpassInputField.gameObject.SetActive(true);
            enteragainpassInputField.gameObject.SetActive(true);
            ChangePassButton.gameObject.SetActive(true);
            showNewPasswordButton.gameObject.SetActive(true);
            showEnterAgainPasswordButton.gameObject.SetActive(true);
            showiconpass.gameObject.SetActive(true);
            showiconpassagain.gameObject.SetActive(true);
        }
        else
        {
            messageText.text = "Tên người dùng hoặc ngày sinh không chính xác";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
        }

    }

    // API kiểm tra thông tin người dùng và sinh nhật
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
                if (www.downloadHandler.text == "not-available")
                {
                    information = true;

                }
                else if (www.downloadHandler.text == "available")
                {
                    messageText.text = "Tên người dùng hoặc ngày sinh không chính xác";
                    imageMessage.gameObject.SetActive(true);
                    StartCoroutine(HideMessageAfterDelay(3f));
                    maleAnimator.Play("AnimBuon_anim");
                    Invoke("DisableAnimator", 100f);
                }
              
            }
            else
            {
                messageText.text = "Không thể kết nối đến máy chủ";
                imageMessage.gameObject.SetActive(true);
                StartCoroutine(HideMessageAfterDelay(3f));
                maleAnimator.Play("AnimBuon_anim");
                Invoke("DisableAnimator", 100f);
            }
        }
    }

    // Hàm cập nhật thông tin của người dùng vào users nếu như có Internet
    /*
    private IEnumerator InforUserAfterUpdate()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Lỗi khi load dữ liệu người dùng: " + www.error);
                messageText.text = "Lỗi mạng";
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                PlayerPrefs.SetString("users", responseJson);
                string jsonData = PlayerPrefs.GetString("users");
                Debug.Log("jsonData: " + jsonData);

            }
        }
    }
    */

    // Xử lí Dropdown cho năm 
    private string GetDateOfBirth()
    {
        int day = dayDropdown.value; 
        int month = monthDropdown.value;
        int year = int.Parse(yearDropdown.options[yearDropdown.value].text);
        string dateOfBirth = string.Format("{0:D2}/{1:D2}/{2:D4}", day, month, year);
        return dateOfBirth;
    }

    //Show - Hide Password
    public void ShowHidenewPass()
    {
        isPasswordVisible = !isPasswordVisible;
        if (isPasswordVisible)
        {
            newpassInputField.contentType = InputField.ContentType.Standard;
            showNewPasswordButton.image.sprite = showPasswordIcon;
        }
        else
        {
            newpassInputField.contentType = InputField.ContentType.Password;
            showNewPasswordButton.image.sprite = hidePasswordIcon;

        }
        newpassInputField.ForceLabelUpdate();
    }

    //Show - Hide Enter Password Again
    public void ShowHidePassAgain()
    {
        isPasswordVisible = !isPasswordVisible;
        if (isPasswordVisible)
        {
            enteragainpassInputField.contentType = InputField.ContentType.Standard;
            showEnterAgainPasswordButton.image.sprite = showPasswordIcon;
        }
        else
        {
            enteragainpassInputField.contentType = InputField.ContentType.Password;
            showEnterAgainPasswordButton.image.sprite = hidePasswordIcon;

        }
        enteragainpassInputField.ForceLabelUpdate();
    }

    // Hàm loại bỏ dấu tiếng Việt
    private string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    //Hàm ẩn thông báo
    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Đợi trong 3 giây
        messageText.text = ""; // Xóa nội dung thông báo sau khi đợi xong
        imageMessage.gameObject.SetActive(false);
    }
    //Hàm Load Scene
    public void LoadScene()
    {
        SceneManager.LoadScene("Login_Server");
    }

}


