using Owner.Script.Signals;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(this.Container);
        this.Container.DeclareSignal<LoseGameSignal>();
        this.Container.DeclareSignal<ReloadResourceSignal>();
        this.Container.DeclareSignal<AddScoreSignal>();
        this.Container.DeclareSignal<LoadItem>();
        this.Container.DeclareSignal<UseItemSignal>();
        this.Container.DeclareSignal<ShowPopupSignal>();
        this.Container.DeclareSignal<ClosePopup>();
    }
}