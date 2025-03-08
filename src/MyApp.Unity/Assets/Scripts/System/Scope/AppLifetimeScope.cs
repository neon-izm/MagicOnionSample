using VContainer;
using VContainer.Unity;

public class AppLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // ChatPresenterをシングルトンとして登録
        builder.Register<IChatPresenter, ChatPresenter>(Lifetime.Singleton);
        // ChatViewはシーン内のコンポーネントなので、VContainerが自動的にInjectします
        builder.RegisterComponentInHierarchy<ChatView>();
    }
}
