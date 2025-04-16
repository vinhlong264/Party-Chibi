using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pannelStatus;
    private void OnEnable()
    {
        EventChanel.Register(KeyEvent.Status , OnHandlerStatus);
    }

    private void OnDestroy()
    {
        EventChanel.UnRegister(KeyEvent.Status , OnHandlerStatus);
    }

    private void OnHandlerStatus(object[] value)
    {
        pannelStatus.SetActive(true);
    }
}
