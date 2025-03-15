using UnityEngine;
using UnityEngine.UI;
using R3;
using VContainer;
using System;
using System.Threading.Tasks;

public class ChatView : MonoBehaviour
{
    [SerializeField] private Text chatText;
    [SerializeField] private Button joinOrLeaveButton;
    [SerializeField] private Text joinOrLeaveButtonText;
    [SerializeField] private Button sendMessageButton;
    [SerializeField] private InputField input;
    //[SerializeField] private InputField reportInput;
    //[SerializeField] private Button sendReportButton;
    [SerializeField] private Button disconnectButton;
    //[SerializeField] private Button exceptionButton;
    //[SerializeField] private Button unaryExceptionButton;
    [SerializeField] private Text labelRtt;

    [Inject] private IChatPresenter chatPresenter;

    private IDisposable chatTextSubscription;
    private IDisposable rttSubscription;

    [Inject]
    public void Construct(IChatPresenter chatPresenter)
    {
        this.chatPresenter = chatPresenter;
    }
    private void Start()
    {
        if(chatPresenter == null)
        {
            Debug.LogError("chatPresenter is null");
            return;
        }
        
        // ChatPresenterのReactivePropertyを購読してUI更新
        chatTextSubscription = chatPresenter.ChatText
            .Subscribe(text => chatText.text = text);
        rttSubscription = chatPresenter.Rtt
            .Subscribe(ms => labelRtt.text = $"RTT: {ms:#,0}ms");

        // ボタンイベントの登録
        joinOrLeaveButton.onClick.AddListener(async () =>
        {
            await chatPresenter.JoinOrLeaveAsync(input.text);
            UpdateUIState();
        });
        sendMessageButton.onClick.AddListener(async () =>
        {
            await chatPresenter.SendMessageAsync(input.text);
            input.text = "";
        });
        
        disconnectButton.onClick.AddListener(async () =>
        {
            await chatPresenter.DisconnectAsync();
        });
        /*
        sendReportButton.onClick.AddListener(async () =>
        {
            await chatPresenter.SendReportAsync(reportInput.text);
            reportInput.text = "";
        });
        exceptionButton.onClick.AddListener(async () =>
        {
            await chatPresenter.GenerateExceptionAsync();
        });
        unaryExceptionButton.onClick.AddListener(async () =>
        {
            await chatPresenter.UnaryGenerateExceptionAsync();
        });
        */

        // 初期UI状態
        UpdateUIState();

        // Presenterの初期化開始
        _ = chatPresenter.InitializeAsync();
    }

    private void UpdateUIState()
    {
        bool isJoined = chatPresenter.IsJoined;
        sendMessageButton.interactable = isJoined;
        joinOrLeaveButtonText.text = isJoined ? "Leave the room" : "Enter the room";
        // 必要に応じて他のUI要素も更新
    }

    private void OnDestroy()
    {
        chatTextSubscription?.Dispose();
        rttSubscription?.Dispose();
    }
}
