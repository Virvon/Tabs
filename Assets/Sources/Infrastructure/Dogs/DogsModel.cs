using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Infrastructure.ServerRequests;
using UnityEngine;
using UnityEngine.Networking;

namespace Sources.Infrastructure.Dogs
{
    public class DogsModel
    {
        private readonly ServerRequestQueue _requestQueue;

        private List<DogCell> _cells;
        private ServerRequest _updateListRequest;
        private DogCell _currentLoadingDogCell;

        public DogsModel(ServerRequestQueue requestQueue)
        {
            _requestQueue = requestQueue;
        }

        public event Action<IReadOnlyList<DogCell>> ListUpdated;
        public event Action ListUpdateStarted;
        public event Action<string, string> DogInfoLoaded; 
        
        public async UniTaskVoid UpdateInfo()
        {
            ListUpdateStarted?.Invoke();

            using (UnityWebRequest webRequest =
                   UnityWebRequest.Get("https://dogapi.dog/api/v2/breeds"))
            {
                _updateListRequest = new(webRequest);
                
                string jsonResponse = await _requestQueue.EnqueueRequest(_updateListRequest);
                _updateListRequest = null;
                
                BreedsResponse breedsResponse = JsonUtility.FromJson<BreedsResponse>(jsonResponse);

                _cells = breedsResponse.data.Select(value => new DogCell(value.attributes.name, value.id, _requestQueue)).ToList();
            }

            foreach (DogCell dogCell in _cells)
            {
                dogCell.LoadStarted += LoadDog;
                dogCell.LoadFinished += OnLoadFinished;
            }
            
            ListUpdated?.Invoke(_cells);
        }
        
        public void Stop()
        {
            if (_updateListRequest != null)
            {
                _requestQueue.CancelRequest(_updateListRequest);
                _updateListRequest = null;
            }

            if (_currentLoadingDogCell != null)
            {
                _currentLoadingDogCell.CancelLoading();
                _currentLoadingDogCell = null;
            }

            if (_cells != null)
            {
                foreach (DogCell dogCell in _cells)
                {
                    dogCell.LoadStarted -= LoadDog;
                    dogCell.LoadFinished -= OnLoadFinished;
                }
            
                _cells.Clear();
            }
        }

        private void OnLoadFinished(DogCell dogCell, string description)
        {
            DogInfoLoaded?.Invoke(dogCell.Name, description);
            _currentLoadingDogCell = null;
        }

        private void LoadDog(DogCell dogCell)
        {
            if(_currentLoadingDogCell == dogCell)
                return;

            if (_currentLoadingDogCell != null)
                _currentLoadingDogCell.CancelLoading();

            _currentLoadingDogCell = dogCell;
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