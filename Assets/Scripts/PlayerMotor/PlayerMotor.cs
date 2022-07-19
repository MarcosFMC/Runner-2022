
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

     public Vector3 moveVector;
    [HideInInspector] public float verticalVelocity;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public int currentLane;
    public bool hasDied = false;


    public float distanceBeetweenLanes = 3.0f;
    public float baseRunSpeed = 5.0f;
    public float baseSidewaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;


    public CharacterController controller;
    private BaseState _state;
    public Animator _anim;

    private bool isPaused;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _state = GetComponent<RunningState>();
        _state.Construct();
        isPaused = true; 
    }

    private void Update()
    {
        if(!isPaused)
          UpdateMotor();
       

    }

    private void UpdateMotor()
    {
        isGrounded = controller.isGrounded;

        moveVector = _state.ProcessMotion();

        _state.Transition();

        _anim?.SetBool("IsGrounded", isGrounded);
        _anim?.SetFloat("Speed", Mathf.Abs(moveVector.z));

        controller.Move(moveVector * Time.deltaTime);
    }

    public float SnapToLane()
    {
        float r = 0.0f;

        if(transform.position.x != (currentLane * distanceBeetweenLanes))
        {
            float deltaToDesiredPosition = (currentLane * distanceBeetweenLanes) - transform.position.x;

            r = (deltaToDesiredPosition > 0) ? 1 : -1;
            r *= baseSidewaySpeed;

            float actualDistance = r * Time.deltaTime;

            if (Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition))        
                r = (deltaToDesiredPosition * (1 / Time.deltaTime));                          
        }
        else
        {
            r = 0f;
        }

        return r;
    }
    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }

    public void ChangeState(BaseState s)
    {
        _state.Destruct();
        _state = s;
        _state.Construct();
    }

    public void ApplyGravity()
    {
       verticalVelocity -= gravity * Time.deltaTime;
        if (verticalVelocity < -terminalVelocity)
            verticalVelocity = -terminalVelocity;
    }

    public void PausePlayer()
    {
        isPaused = true;
    }

    public void ResumePlayer()
    {
        isPaused = false;
    }

    public void ResetPlayer()
    {
        currentLane = 0;
       transform.position = Vector3.zero;
       _anim?.SetTrigger("Idle");
       PausePlayer();
        ChangeState(GetComponent<RunningState>());
    }
    public void RespawnPlayer()
    {
        ChangeState(GetComponent<RespawnState>());
        GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);
        if (hitLayerName == "Death" && !hasDied)
            ChangeState(GetComponent<DeathState>());
    }
}
