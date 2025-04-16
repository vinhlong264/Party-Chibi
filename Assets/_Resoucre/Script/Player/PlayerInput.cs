using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputCharacter _input;
    public InputCharacter _InputCharacter { get => _input; }

   
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = Camera.main.transform.forward * vertical + Camera.main.transform.right * horizontal;
        dir.y = 0;
        dir.Normalize();
        _input.moveDirection = dir;

        if(_input.moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(dir.x , dir.z) * Mathf.Rad2Deg;
            Quaternion toRotation = Quaternion.Euler(0, angle, 0);
            _input.LookRotation = Quaternion.Lerp(_input.LookRotation, toRotation, 0.2f);
        }

        _input.isRun |= Input.GetKey(KeyCode.LeftShift);
        _input.isJump |= Input.GetKeyDown(KeyCode.Space);
        _input.isAttack |= Input.GetKeyDown(KeyCode.Mouse0);
    }

    public void ResetInput()
    {
        _input.moveDirection = Vector3.zero;
        _input.LookRotation = Quaternion.identity;
        _input.isRun = false;
        _input.isJump = false;
        _input.isAttack = false;
    }
}

public struct InputCharacter
{
    public Vector3 moveDirection;
    public Quaternion LookRotation;
    public bool isRun;
    public bool isAttack;
    public bool isJump;
}
