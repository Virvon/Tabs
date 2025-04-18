using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine.Networking;
using Zenject;

namespace Sources.Infrastructure.ServerRequests
{
    public class ServerRequestQueue : IInitializable, IDisposable
    {
        private readonly Subject<ServerRequest> _requestSubject = new();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private Queue<ServerRequest> _requestQueue = new();
        
        private bool _isProcessing;
        private ServerRequest _currentRequest;

        public void Initialize()
        {
            _requestSubject
                .SelectMany(request => ProcessRequest(request).ToObservable())
                .Subscribe()
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            while (_requestQueue.Count > 0)
            {
                _requestQueue.Dequeue().Cancel();
            }
            _currentRequest?.Cancel();
            _disposables.Dispose();
        }

        public UniTask<string> EnqueueRequest(ServerRequest request)
        {
            _requestQueue.Enqueue(request);
            ProcessNextRequest();
            return request.CompletionSource.Task;
        }
        
        public void CancelRequest(ServerRequest request)
        {
            if (_currentRequest == request)
            {
                _currentRequest.Cancel();
                _currentRequest = null;
                ProcessNextRequest(); 
            }
            else
            {
                var newQueue = new Queue<ServerRequest>();
                while (_requestQueue.Count > 0)
                {
                    var queuedRequest = _requestQueue.Dequeue();
                    if (queuedRequest != request)
                    {
                        newQueue.Enqueue(queuedRequest);
                    }
                    else
                    {
                        queuedRequest.Cancel();
                    }
                }
                _requestQueue = newQueue;
            }
        }

        private void ProcessNextRequest()
        {
            if (_requestQueue.Count == 0)
            {
                _isProcessing = false;
                return;
            }

            ServerRequest nextRequest = _requestQueue.Dequeue();
            _requestSubject.OnNext(nextRequest);
        }

        private async UniTask ProcessRequest(ServerRequest request)
        {
            _currentRequest = request;
            
            try
            {
                await request.WebRequest.SendWebRequest()
                    .ToUniTask(cancellationToken: request.CancellationTokenSource.Token);

                if (request.WebRequest.result == UnityWebRequest.Result.Success)
                    request.CompletionSource.TrySetResult(request.WebRequest.downloadHandler.text);
                else
                    request.CompletionSource.TrySetException(new Exception(request.WebRequest.error));
            }
            catch (OperationCanceledException)
            {
                _currentRequest.CompletionSource.TrySetCanceled();
            }
            finally
            {
                _currentRequest = null;
            }
        }
    }
}