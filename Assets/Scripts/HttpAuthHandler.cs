using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class HttpAuthHandler : MonoBehaviour
{
    [SerializeField] string serverApiURL;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void SingUp()
    {
        User user = new User();
        user.username = GameObject.Find("InputFieldUsername").GetComponent<InputField>().text;
        user.password = GameObject.Find("InputFieldPassword").GetComponent<InputField>().text;


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
