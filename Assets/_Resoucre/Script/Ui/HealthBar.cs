using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        EventChanel.Register(KeyEvent.HealthBar, UpdateSlider);
    }

    private void OnDestroy()
    {
        EventChanel.UnRegister(KeyEvent.HealthBar , UpdateSlider);
    }

    private void UpdateSlider(object[] value)
    {
        Debug.Log(value[0]);
        Debug.Log(value[1]);

        healthSlider.maxValue = (float)value[0];
        healthSlider.value = (float)value[1];

        if(healthSlider.value <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
