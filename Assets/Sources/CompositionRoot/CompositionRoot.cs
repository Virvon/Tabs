using System;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;
using Sources.Infrastructure.ServerRequests;
using Sources.UI.TabsMediator;

public class CompositionRoot : MonoBehaviour
{
    private Mediator _mediator;

    [Inject]
    private void Construct(Mediator mediator) =>
        _mediator = mediator;

    private void Start() =>
        _mediator.ShowStartTab();
}
