using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.CompositionRoot;
using Sources.Infrastructure.ServerRequests;
using UnityEngine;
using UnityEngine.Networking;

namespace Sources.Infrastructure.Dogs
{
    public class DogCell
    {
        public readonly string Name;

        private readonly string _id;
        private readonly ServerRequestQueue _requestQueue;

        private bool _isLoaded;
        private ServerRequest _request;

        public DogCell(string name, string id, ServerRequestQueue requestQueue)
        {
            Name = name;
            _id = id;
            _requestQueue = requestQueue;

            _isLoaded = false;
        }

        public event Action<DogCell> LoadStarted;
        public event Action<DogCell, string> LoadFinished;

        public void LoadDogInfo()
        {
            if(_isLoaded)
                return;

            
            LoadStarted?.Invoke(this);
            Load().Forget();
        }

        private async UniTaskVoid Load()
        {
            _isLoaded = true;

            using (UnityWebRequest webRequest =
                   UnityWebRequest.Get($"https://dogapi.dog/api/v2/breeds/{_id}"))
            {
                _request = new(webRequest);
                
                string jsonResponse = await _requestQueue.EnqueueRequest(_request);
                
                Debug.Log("succsess " + jsonResponse);

                BreedResponse breedResponse = JsonUtility.FromJson<BreedResponse>(jsonResponse);
                Debug.Log(breedResponse.data.attributes.description);
                
                LoadFinished?.Invoke(this, breedResponse.data.attributes.description);
            }
            
            _isLoaded = false;
        }
        
        [Serializable]
        private class BreedResponse
        {
            public Breed data;
    
            [Serializable]
            public class Breed
            {
                public string id;
                public string type;
                public Attributes attributes;
        
                [Serializable]
                public class Attributes
                {
                    public string name;
                    public string description;
                }
            }
        }
    }
}