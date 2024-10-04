

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField fullNameInput;
    public Dropdown dayDropdown;
    public Dropdown monthDropdown;
    public Dropdown yearDropdown;
    public Toggle maleToggle;
    public Toggle femaleToggle;
    public Button ShowPassButton;
    public Sprite showPasswordIcon;
    public Sprite hidePasswordIcon;
    public Text messageText;
    public Image imageMessage;
    public Button registerButton;
    public bool isPasswordVisible = false;
    private bool isAnimatingBuon = false;
    public Animator maleAnimator;

    private void Start()
    {
        imageMessage.gameObject.SetActive(false);
        ConnectInternet connectInternet = new ConnectInternet();
        {
            connectInternet.CheckedInternet((callback) =>
            {
                if(callback == 1)
                {
                    messageText.text = "Hãy kết nối với Internet trước khi đăng ký tài khoản";
                }
                else
                {
                    registerButton.onClick.AddListener(UserRegistration);
                }
            });
        }
    }
    public void UserRegistration()
    {
        string username = usernameInput.text;
        if (!IsUsernameValid(username))
        {
            return;
        }
        string password = passwordInput.text;
        if (!IsPasswordValid(password))
        {
            return;
        }

        string name = fullNameInput.text;
        if(!IsFullnameValid(name))
        {
            return;
        }
        string dateOfBirth = GetDateOfBirth();
        string gender = GetGender();

        StartCoroutine(SendRegistrationRequest(username, password, name, dateOfBirth, gender));
    }

    //Load to Scene Login Server
    public void LoadScene()
    {
        SceneManager.LoadScene("Login_Server");
    }

    // Hàm kiểm tra thông tin nhập username
    private bool IsUsernameValid(string username)
    {
        if (string.IsNullOrEmpty(username))
        {   
            isAnimatingBuon = true;
            messageText.text = "Tên đăng nhập dùng không được bỏ trống";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator",100f);
            return false;
        }
        if (char.IsDigit(username[0]))
        {
            messageText.text = "Tên đăng nhập không bắt đầu bằng số.";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
            return false;
        }
        // Kiểm tra chiều dài
        if (username.Length < 6 || username.Length > 20)
        {
            messageText.text = "Tên đăng nhập không quá ngắn dưới 6 kí tự và dài hơn 20 kí tự.";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
            return false;
        }

        // Kiểm tra không chứa dấu cách và dấu
        if (username.Contains(" "))
        {
            messageText.text = "Tên đăng nhập dùng không chứa khoảng trắng";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
            return false;
        }
        if ( username != RemoveDiacritics(username))
        {
            messageText.text = "Tên đăng nhập dùng không chứa dấu câu.";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
            return false;
        }
        // Kiểm tra không bắt đầu bằng số


        return true;
    }

    private void DisableAnimator()
    {
        maleAnimator.enabled = false;
        maleAnimator.Play("AnimChao");
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
    // Hàm kiểm tra mật khẩu
    private bool IsPasswordValid(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            messageText.text = "Mật khẩu không được bỏ trống";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
            return false;
        }
        if (password.Contains(" ") || password != RemoveDiacritics(password))
        {
            messageText.text = "Mật khẩu không chứa khoảng trắng.";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
            return false;
        }
        // Kiểm tra chiều dài
        if (password.Length < 6 || password.Length > 20)
        {
            messageText.text = "Mật khẩu không quá ngắn dưới 6 kí tự và dài hơn 20 kí tự.";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
            return false;
        }
        //Kiểm tra dấu cách 
        
        // Kiểm tra xem mật khẩu có ít nhất 1 ký tự hoa và 1 ký tự thường hay không
        if (!Regex.IsMatch(password, "(?=.*[A-Z])(?=.*[a-z])"))
        {
            messageText.text = "Mật khẩu phải chứa ít nhất 1 ký tự hoa và 1 ký tự thường.";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
            return false;
        }
      

        return true;
    }
    // Xử lí Tên
    private bool IsFullnameValid(string name)
    {
        // Kiểm tra xem fullname có rỗng hoặc null không
        if (string.IsNullOrEmpty(name))
        {
            messageText.text = "Họ và Tên người dùng không được bỏ trống";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
            return false;
        }
        // Kiểm tra xem fullname không chứa chữ số
        if (Regex.IsMatch(name, @"\d"))
        {
            messageText.text = "Họ và Tên người dùng không chứa chữ số";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
            return false;
        }

        return true;
    }
    // Xử lí Ngày sinh cho người dùng
    private string GetDateOfBirth() 
    {
        int day = dayDropdown.value; // +1 vì giá trị trong Dropdown bắt đầu từ 0
        int month = monthDropdown.value;
        int year = int.Parse(yearDropdown.options[yearDropdown.value].text);
        //tốn 2h đồng hồ để sửa chỉ vì THIẾU THÔNG MINH :)))    
        string dateOfBirth = string.Format("{0:D2}/{1:D2}/{2:D4}", day, month, year);
        return dateOfBirth;
    }
    //Xử lí Giới tính 
    private string GetGender()
    {
        if (maleToggle.isOn)
        {
            return "Nam";
        }
        else if (femaleToggle.isOn) 
        {
            return "Nữ";
        }
        else
        {
            return "";
        }
       
    }
    //Xử lí Đăng ký cho người dùng

    IEnumerator SendRegistrationRequest(string username,string password,string name, string dateOfBirth,string gender)
    {
        yield return CheckUsernameAvailability(username);
        if (usernameAvailable)
        {
            string url = "http://localhost:3000/admin/add-new-user";

            User register = new User();
            register.username = username;
            register.password = password;
            register.name = name;
            register.birth = dateOfBirth;
            register.sex = gender;


            string json = "{\"username\":\"" + register.username + "\",\"password\":\"" + register.password + "\",\"name\":\"" + register.name + "\",\"birth\":\"" + register.birth + "\",\"sex\":\"" + register.sex + "\"}";
            UnityWebRequest www = UnityWebRequest.PostWwwForm(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                messageText.text = "Đăng ký thành công!";
                imageMessage.gameObject.SetActive(true);
                StartCoroutine(HideMessageAfterDelay(3f));
                maleAnimator.Play("AnimNhay");
                Invoke("DisableAnimator", 100f);
                Debug.Log(json);    
            }
            else
            {
                messageText.text = "Lỗi: " + www.error;
                imageMessage.gameObject.SetActive(true);
                StartCoroutine(HideMessageAfterDelay(3f));
                maleAnimator.Play("AnimBuon_anim");
                Invoke("DisableAnimator", 100f);
            }
        }
        else
        {
            messageText.text = "Tên người dùng đã có đã tồn tại";
            imageMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(3f));
            maleAnimator.Play("AnimBuon_anim");
            Invoke("DisableAnimator", 100f);
        }
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        messageText.text = ""; 
        imageMessage.gameObject.SetActive(false);
    }
    private bool usernameAvailable = false;
    // Kiểm tra tên người dùng 
    private IEnumerator CheckUsernameAvailability(string username)
    {
        string url = "http://localhost:3000/admin/check-user";

        WWWForm form = new WWWForm();
        form.AddField("username", username);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (www.downloadHandler.text == "available")
                {
                    usernameAvailable = true;
                }
                else if (www.downloadHandler.text == "not-available")
                {
                    messageText.text = "Tên người dùng đã tồn tại. Vui lòng chọn tên người dùng khác.";
                    imageMessage.gameObject.SetActive(true);
                    StartCoroutine(HideMessageAfterDelay(3f));
                    maleAnimator.Play("AnimBuon_anim");
                    Invoke("DisableAnimator", 100f);
                }
                else
                {
                    messageText.text = "Có vẻ như bạn đang gặp vấn đề về server.";
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

    //Show password
    public void ShowPassword()
    {
        isPasswordVisible = !isPasswordVisible;
        if (isPasswordVisible)
        {
            passwordInput.contentType = InputField.ContentType.Standard;
            ShowPassButton.image.sprite = showPasswordIcon;
        }
        else
        {
            passwordInput.contentType = InputField.ContentType.Password;
            ShowPassButton.image.sprite = hidePasswordIcon;
        }
        passwordInput.ForceLabelUpdate();

    }

}
