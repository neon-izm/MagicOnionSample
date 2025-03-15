using UnityEngine;
using VContainer;
using VContainer.Unity;

public class AppLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IChatPresenter,ChatPresenter>(Lifetime.Singleton);
        
    }
}
