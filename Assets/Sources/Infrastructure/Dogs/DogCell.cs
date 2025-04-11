using System;
using Cysharp.Threading.Tasks;
using Sources.Infrastructure.ServerRequests;
using UnityEngine;
using UnityEngine.Networking;

namespace Sources.Infrastructure.Dogs
{
    public class DogCell
    {
        private const string URL = "https://dogapi.dog/api/v2/breeds/";
        
        public readonly string Name;

        private readonly string _id;
        private readonly ServerRequestQueue _requestQueue;

        private ServerRequest _request;

        public DogCell(string name, string id, ServerRequestQueue requestQueue)
        {
            Name = name;
            _id = id;
            _requestQueue = requestQueue;
        }

        public event Action<DogCell> LoadStarted;
        public event Action<DogCell, string> LoadFinished;
        public event Action LoadCanceled; 

        public void LoadDogInfo()
        {
            if(_request != null)
                return;
            
            LoadStarted?.Invoke(this);
            Load().Forget();
        }
        
        public void CancelLoading()
        {
            if(_request == null)
                return;
            
            _requestQueue.CancelRequest(_request);
            _request = null;
        }

        private async UniTaskVoid Load()
        {
            using (UnityWebRequest webRequest =
                   UnityWebRequest.Get($"{URL}{_id}"))
            {
                _request = new(webRequest);
                
                string jsonResponse = await _requestQueue.EnqueueRequest(_request);

                _request = null;

                BreedResponse breedResponse = JsonUtility.FromJson<BreedResponse>(jsonResponse);
                
                LoadFinished?.Invoke(this, breedResponse.data.attributes.description);
            }
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