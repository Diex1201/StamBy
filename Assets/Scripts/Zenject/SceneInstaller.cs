using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private ActionBarView actionBarView;
    public override void InstallBindings()
    {
        Container.Bind<IActionBarView>().FromInstance(actionBarView).AsSingle();
        Container.Bind<ActionBarModel>().AsSingle().NonLazy();
    }
}