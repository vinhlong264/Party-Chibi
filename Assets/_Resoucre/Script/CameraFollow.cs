using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineCamera vitural;
    private CinemachinePositionComposer CinemachineFollow;
    private Transform pos;

    private void Start()
    {
        vitural = GetComponent<CinemachineCamera>();
        if(vitural != null)
        {
            CinemachineFollow = vitural.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachinePositionComposer;
        }    
    }

    public void SetUpCamera(Transform pos)
    {
        vitural.Follow = pos;
    }
}
