using Fusion;
using TMPro;
using UnityEngine;

public class MesseageContent : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI messageTxt;

    public void setUpMessage(string nameCharacter , string message)
    {
        if (nameTxt == null || messageTxt == null) return;

        nameTxt.text = nameCharacter;
        messageTxt.text = message;
    }
}
