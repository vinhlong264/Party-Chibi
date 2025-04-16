using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField messageInput;
    [SerializeField] private Button sendMessage;

    private void Start()
    {
        sendMessage.onClick.AddListener(() => OnHandlerChat());
    }

    private void OnHandlerChat()
    {
        if (messageInput.text == string.Empty) return;

        ChatManager.Instance.SendChat(messageInput.text);
        messageInput.text = string.Empty;
    }
}
