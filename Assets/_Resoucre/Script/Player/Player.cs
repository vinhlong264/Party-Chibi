using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    // References
    private CharacterController controller;
    private NetworkMecanimAnimator anim;
    private PlayerInput input;

    [Header("Movement Infor")]
    private Vector3 currentDir;
    [SerializeField] private float camOfsetZ;
    [SerializeField] private float moveSpeed;

    [Header("Jump infor")]
    //Ground check Variable
    [SerializeField] private Transform groundPos;
    [SerializeField] private LayerMask WhatIsMask;
    [SerializeField] private float radius; // bán kính kiểm tra
    //Gravity
    private float gravity = -9.8f;
    private Vector3 velocity;
    [SerializeField] private float maxJumpHeight;

    [Header("Attack Info")]
    [SerializeField] private bool isTrigger;
    [SerializeField] private Transform weaponHandPos;
    [Networked, OnChangedRender(nameof(RPC_Weapon))]
    public WeaponBase weapon { get; set; }

    //Sysn name
    [Networked, OnChangedRender(nameof(OnChangeNameCharacter))]
    public string nameCharacter { get; set; }

    [SerializeField] private UiLabelName uiLabelName;


    #region State
    private StateManager stateManager;
    public PlayerFactory factory;
    #endregion

    public Vector3 Velocity { get => velocity; }
    public PlayerInput _Input { get => input; }
    public bool IsTrigger { get => isTrigger; }
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            // Init state
            stateManager = new StateManager();
            factory = new PlayerFactory(stateManager, this);

            //Init Component
            input = GetComponent<PlayerInput>();
            controller = GetComponent<CharacterController>();
            anim = GetComponentInChildren<NetworkMecanimAnimator>();

            stateManager.InitState(factory.GetState(PlayerState.IDLE));
            currentDir = transform.position;
            velocity = transform.position;
            nameCharacter = LocalData.name;
        }

        OnChangeNameCharacter();
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority)
        {
            return;
        }

        if (GameManager.instance.gameState != GameState.Playing)
        {
            return;
        }

        stateManager.CurrentState.ExcuteState();
    }

    #region Method Sysn
    public void OnSysnAnimation(string name, bool isTransition)
    {
        anim.Animator.SetBool(name, isTransition);
    }

    private void OnChangeNameCharacter()
    {
        if (uiLabelName == null) return;

        Debug.Log("Change name");
        uiLabelName.SetUpLabelname(nameCharacter, HasInputAuthority);
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_Weapon()
    {
        if (weapon == null) return;

        weapon.SetPostion(weaponHandPos, new Vector3(180, 0, 170));
    }
    public void SetUpCamera()
    {
        CameraFollow folow = FindFirstObjectByType<CameraFollow>();
        if (folow != null)
        {
            folow.SetUpCamera(this.transform);
        }
    }

    #endregion

    #region Handler State
    public void RunHandler()
    {
        controller.Move(input._InputCharacter.moveDirection * moveSpeed * Runner.DeltaTime);
        transform.rotation = input._InputCharacter.LookRotation;
        anim.Animator.SetFloat(constant.HORIZONTAL, GetVariableRun());
    }

    public void AttackHander()
    {
        if (weapon == null) return;

        if (weapon.Handler != null)
        {
            weapon.Handler.ActiveCollider();
        }
        else
        {
            weapon.ExcuteWeapon(weaponHandPos.position, transform.forward);
        }
    }
    public void JumpHandler()
    {
        velocity.x = 0;
        velocity.z = 0;
        velocity.y = Mathf.Sqrt(maxJumpHeight * -2f * gravity);
    }

    public void GravityHandler()
    {
        velocity.y += gravity * Runner.DeltaTime;
        controller.Move(velocity * Runner.DeltaTime);
        anim.Animator.SetFloat(constant.VERTICAL, velocity.y);
    }

    public bool GroundCheck()
    {
        return Physics.CheckSphere(groundPos.position, radius, WhatIsMask);
    }

    public bool CheckInputMove()
    {
        if (input._InputCharacter.moveDirection != Vector3.zero)
        {
            return true;
        }
        input.ResetInput();
        return false;
    }

    public float GetVariableRun()
    {
        if (input._InputCharacter.isRun)
        {
            moveSpeed = 5;
            return 2f;
        }

        moveSpeed = 2f;
        return 1f;
    }

    public InputCharacter GetInput()
    {
        return input._InputCharacter;
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (!HasInputAuthority) return;

        if (weapon != null) return;


        if (other.CompareTag("Weapon"))
        {
            WeaponBase tmp = other.GetComponent<WeaponBase>();
            if (tmp.IsPicked) return;

            if (tmp != null)
            {
                weapon = tmp;
                RPC_Weapon();
            }
        }
    }

    public void AnimTriggerTrue()
    {
        isTrigger = true;
    }

    public void AnimTriggerFalse()
    {
        isTrigger = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundPos.position, radius);
    }
}

public enum PlayerState
{
    IDLE,
    RUN,
    JUMP,
    ATTACK,
    NONE
}

//Struct dùng để đồng bộ dữ liệu
public readonly struct Changed<T> where T : NetworkBehaviour // chỉ cho phép T kế thừa NetworkBehaviour
{
    public readonly T Behaviour;
    public readonly NetworkRunner Runner;
}

