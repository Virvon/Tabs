using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Sources.Infrastructure.ServerRequests
{
    public class ServerRequest: IDisposable
    {
        public UnityWebRequest WebRequest { get; }
        public UniTaskCompletionSource<string> CompletionSource { get; }
        public CancellationTokenSource CancellationTokenSource { get; }

        public ServerRequest(UnityWebRequest webRequest)
        {
            WebRequest = webRequest;
            CompletionSource = new UniTaskCompletionSource<string>();
            CancellationTokenSource = new CancellationTokenSource();
        }

        public void Cancel()
        {
            Debug.Log("cancel");
            if (!CompletionSource.Task.Status.IsCompleted())
            {
                Debug.Log("abort");
                WebRequest.Abort();
                CompletionSource.TrySetCanceled();
                CancellationTokenSource.Cancel();
            }
        }

        public void Dispose()
        {
            WebRequest.Dispose();
            CancellationTokenSource.Dispose();
        }
    }
}