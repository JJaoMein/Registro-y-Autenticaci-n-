using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class HttpAuthHandler : MonoBehaviour
{
    [SerializeField] string serverApiURL;
    [SerializeField] TMP_InputField userNameInput;
    [SerializeField] TMP_InputField passwordInput;


    // Start is called before the first frame update

    void Start()
    {

    }


    public void SingUp()
    {
        User user = new User();
        user.username = userNameInput.text;
        user.password = passwordInput.text;
        Debug.Log(user.username);
        Debug.Log(user.password);


        string postData = JsonUtility.ToJson(user);

        StartCoroutine(SigningUp(postData));
    }


    IEnumerator SigningUp(string postData)
    {
        UnityWebRequest www = UnityWebRequest.Put(serverApiURL + "/api/usuarios", postData);
        www.method = "POST";
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler);

            if (www.responseCode == 200)
            {
                AuthJsonData jsonData = JsonUtility.FromJson<AuthJsonData>(www.downloadHandler.text);
                Debug.Log(jsonData.usuario.username + "Se regitro con id: " + jsonData.usuario._id);
            }
            else
            {
                string mensaje = "Status: " + www.responseCode;
                mensaje += "\ncontent-type:" + www.GetResponseHeader("content-type");
                mensaje += "\nError: " + www.error;
                Debug.Log(mensaje);
            }
        }
    }

    public class User
    {
        public string _id;
        public string username;
        public string password;
        public int score;

        public User(){}
           
        public User(string username, string password) 
        {
            this.username = username;
            this.password = password;
        }
    }

    public class AuthJsonData
    {
        public User usuario;
        public string token;
    }

}
