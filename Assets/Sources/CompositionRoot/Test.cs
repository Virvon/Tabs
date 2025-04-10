using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Sources.CompositionRoot
{
    public class Test : MonoBehaviour
    {
        
        void Start()
        {
            StartCoroutine(GetDogData());
        }

        IEnumerator GetDogData()
        {
            Debug.Log("test");
            string url = "https://dogapi.dog/api/v2/breeds";
        
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // Отправляем запрос и ждем ответа
                yield return webRequest.SendWebRequest();
            
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    Debug.Log("Received: " + jsonResponse);
                
                    BreedsResponse response = JsonUtility.FromJson<BreedsResponse>(jsonResponse);
                    foreach (Breed breed in response.data)
                    {
                        Debug.Log($"Breed: {breed.attributes.name}, Description: {breed.attributes.description}");
                    }
                }
            }
        }
    }
    [System.Serializable]
    public class Breed
    {
        public string id;
        public string type;
        public Attributes attributes;
    }

    [System.Serializable]
    public class Attributes
    {
        public string name;
        public string description;
    }

    [System.Serializable]
    public class BreedsResponse
    {
        public Breed[] data;
    }

}