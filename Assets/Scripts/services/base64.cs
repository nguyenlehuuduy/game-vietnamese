using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class base64 : MonoBehaviour
{
    public RawImage imageRenderer1;
    public RawImage imageRenderer2;
    public RawImage imageRenderer3;

    private string apiUrl = "http://localhost:3000/admin/get-image/653f85a627181b4e3ae2d98b"; 

    private void Start()
    {
        StartCoroutine(LoadUserData());
        
    }

    private IEnumerator LoadUserData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading user data: " + www.error);
            }
            else
            {
                string userdata = www.downloadHandler.text;
                Debug.Log("User data" + userdata);
                ImageData Image = JsonUtility.FromJson<ImageData>(userdata);
                
                string image1 = Image.obj.image1;
                string image2 = Image.obj.image2;
                string image3 = Image.obj.image3;

                if (!string.IsNullOrEmpty(image1))
                {
                    Texture2D texture1 = Base64ToTexture2D(image1);
                    if (texture1 != null)
                    {
                        Material material1 = new Material(Shader.Find("UI/Default")); // Tạo một Material mới để đối tượng hiển thị lên màn hình
                        material1.mainTexture = texture1; // material1 sẽ sử dụng texture1 làm hình ảnh chính để vẽ hoặc hiển thị  trên đối tượng của giao diện
                        imageRenderer1.material = material1; // Gán Material mới cho RawImage
                    }
                    else
                    {
                        Debug.LogError("Failed to convert base64 to Texture2D.");
                    }
                }
                if (!string.IsNullOrEmpty(image2))
                {
                    Texture2D texture2 = Base64ToTexture2D(image2);
                    if (texture2 != null)
                    {
                        Material material2 = new Material(Shader.Find("UI/Default")); 
                        material2.mainTexture = texture2;
                        imageRenderer2.material = material2;
                    }
                    else
                    {
                        Debug.LogError("Failed to convert base64 to Texture2D.");
                    }
                }

                if (!string.IsNullOrEmpty(image3))
                {
                    Texture2D texture3 = Base64ToTexture2D(image3);
                    if (texture3 != null)
                    {
                        Material material3 = new Material(Shader.Find("UI/Default")); // Tạo một Material mới
                        material3.mainTexture = texture3;
                        imageRenderer3.material = material3; // Gán Material mới cho RawImage
                    }
                    else
                    {
                        Debug.LogError("Failed to convert base64 to Texture2D.");
                    }
                }
            }
        }
    }

    private Texture2D Base64ToTexture2D(string base64)
    {
        try
        {
            byte[] bytes = System.Convert.FromBase64String(base64);
            Texture2D texture = new Texture2D(1, 1);
            if (texture.LoadImage(bytes))
            {
                return texture;
            }
            else
            {
                Debug.LogError("Failed to load image bytes into Texture2D.");
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error converting base64 to Texture2D: " + ex.Message);
            return null;
        }
    }
}

[System.Serializable]
public class ImageData
{
    public ImageDataObject obj;
}

[System.Serializable]
public class ImageDataObject
{
    public string image1;
    public string image2;
    public string image3;
}
