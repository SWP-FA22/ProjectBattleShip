using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(this.Container);
        this.Container.DeclareSignal<LoseGameSignal>();
    }
}