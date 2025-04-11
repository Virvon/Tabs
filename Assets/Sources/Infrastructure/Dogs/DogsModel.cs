using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Infrastructure.ServerRequests;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Sources.Infrastructure.Dogs
{
    public class DogsModel : IInitializable
    {
        private readonly ServerRequestQueue _requestQueue;
        private readonly List<ServerRequest> _requests = new();

        private List<DogCell> _cells;

        public DogsModel(ServerRequestQueue requestQueue)
        {
            _requestQueue = requestQueue;
        }

        public event Action<IReadOnlyList<DogCell>> Setuped;
        public event Action<string, string> InfoLoaded; 

        public async void Initialize()
        {
            using (UnityWebRequest webRequest =
                   UnityWebRequest.Get("https://dogapi.dog/api/v2/breeds"))
            {
                ServerRequest request = new(webRequest);
                _requests.Add(request);
                
                string jsonResponse = await _requestQueue.EnqueueRequest(request);
                
                Debug.Log("succsess " + jsonResponse);
                BreedsResponse breedsResponse = JsonUtility.FromJson<BreedsResponse>(jsonResponse);
                Debug.Log(breedsResponse.data.Length);

                _cells = breedsResponse.data.Select(value => new DogCell(value.attributes.name, value.id, _requestQueue)).ToList();
            }

            foreach (DogCell dogCell in _cells)
            {
                dogCell.LoadStarted += LoadDog;
                dogCell.LoadFinished += OnLoadFinished;
            }
            
            Setuped?.Invoke(_cells);
        }

        private void OnLoadFinished(DogCell dogCell, string description)
        {
            InfoLoaded?.Invoke(dogCell.Name, description);
        }

        private void LoadDog(DogCell dogCell)
        {
            
        }

        [Serializable]
        private class BreedsResponse
        {
            public Breed[] data;
    
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