using TMPro;
using UnityEngine;

public class UiLabelName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nametxt;

    private Transform camePos;

    private void Awake()
    {
        camePos = Camera.main.transform;
        nametxt.text = string.Empty;
    }

    private void LateUpdate()
    {
        nametxt.transform.rotation = Quaternion.LookRotation(camePos.forward);
    }

    public void SetUpLabelname(string labelname)
    {
        nametxt.text = labelname;
    }
}
