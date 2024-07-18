using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour, IHealthy, IDamageable
{
    #region PlayerComponents
    [SerializeField] private new Rigidbody rigidbody;
    public Rigidbody RigidBody { get => rigidbody; }
    [SerializeField] private new CapsuleCollider collider;
    public CapsuleCollider Collider { get => collider; }
    [SerializeField] private Transform mainCamera;
    public Transform MainCamera{ get => mainCamera; }
    [SerializeField] private Transform orientation;
    public Transform Orientation { get => orientation; }
    [SerializeField] private RightArmController rightArmController;
    public RightArmController RightArmController { get => rightArmController; }
    [SerializeField] private LeftArmController leftArmController;
    public LeftArmController LeftArmController { get => leftArmController; }
    public StateMachine StateMachine { get; private set; }
    public InputsController InputsController { get; private set; }
    public CoyoteTimer Coyote { get; private set; }
    public PlayerSoundController SoundController { get; private set; }
    public PlayerCameraManager CameraManager { get; private set; }
    #endregion
    #region States
    public IdleState IdleState { get; private set; }
    public WalkState WalkState { get; private set; }
    public RunState RunState { get; private set; }
    public FreeFallState FreeFallState { get; private set; }
    public JumpState JumpState { get; private set; }
    public CrouchIdleState CrouchIdleState { get; private set; }
    public CrouchState CrouchState { get; private set; }
    public SlideState SlideState { get; private set; }
    public WallRunState WallRunState { get; private set; }
    public DeathState DeathState { get; private set; }
    #endregion
    #region PlayerParametrs
    [Header("Gravity Settings")]
    public RaycastHit groundRay;
    [SerializeField] private float jumpTime;
    public float JumpTime { get => jumpTime; }
    [SerializeField] private float jumpForce;
    public float JumpForce { get => jumpForce; }
    [SerializeField] private float groundRadius;
    public float GroundRadius { get => groundRadius; }
    [SerializeField] private float groundRayCastMaxDistance;
    public float GroundRayCastMaxDistance { get => collider.center.y + groundRayCastMaxDistance - groundRadius; }
    [SerializeField] private float roofRayCastMaxDistance;
    public float RoofRayCastMaxDistance { get => collider.center.y + groundRayCastMaxDistance - groundRadius;}
    [SerializeField] private float moveDrag;
    public float MoveDrag { get => moveDrag; }
    [SerializeField] private float fallDrag;
    public float FallDrag { get => fallDrag; } 

    [Header("Move Settings")]
    public LayerMask groundMask;
    public float desiredMoveSpeed;
    [SerializeField] private float stairMaxHeight;
    public float StairMaxHeight { get => stairMaxHeight; set => stairMaxHeight = value; }
    [SerializeField] private float stairSmooth;
    public float StairSmooth { get => stairSmooth; }
    [SerializeField] private float airForce;
    public float AirForce { get => airForce; }
    [SerializeField] private float moveForce;
    public float MoveForce { get => moveForce; } 
    [SerializeField] private float walkSpeed;
    public float WalkSpeed { get => walkSpeed; }
    [SerializeField] private float runSpeed;
    public float RunSpeed { get => runSpeed; }
    [Header("Crouch Settings")]
    [SerializeField] private float crouchSpeed;
    public float CrouchSpeed { get => crouchSpeed; }
    [SerializeField] private float crouchAnimationMultiply;
    public float CrouchAnimationMultiply { get => crouchAnimationMultiply; }
    [SerializeField] private float crouchAnimationWeight;
    public float CrouchAnimationWeight { get => crouchAnimationWeight; }
    [SerializeField] private float crouchCameraYPosition;
    public float CrouchCameraYPosition { get => crouchCameraYPosition; }
    [SerializeField] private float crouchColliderHeight;
    public float CrouchColliderHeight { get => crouchColliderHeight; }
    [Header("Crouch Settings")]
    [SerializeField] private float slideForce;
    public float SlideForce { get => slideForce; }
    [SerializeField] private float slideColliderHeight;
    public float SlideColliderHeight { get => slideColliderHeight; }
    [SerializeField] private float slideCameraYPosition;
    public float SlideCameraYPosition { get => slideCameraYPosition; }
    public float DefaultColliderHeight { get; private set; }
    [SerializeField] private float slideAnimationMultiply;
    public float SlideAnimationMultiply { get => slideAnimationMultiply; }
    [SerializeField] private float slideAnimationWeight;
    public float SlideAnimationWeight { get => slideAnimationWeight; }
    [SerializeField] private float slideDrag;
    public float SlideDrag { get => slideDrag; }
    public Vector3 GroundCheckPosition { get => transform.position + collider.center; }
    public float VelocityMagnitude { get => rigidbody.velocity.magnitude; }
    public Vector3 HorizontalVelocity { get => new Vector3(rigidbody.velocity.x, 0.0f, rigidbody.velocity.z); }
    public Vector3 Velocity { get => rigidbody.velocity; }
    private float desiredColliderHeight;
    public float DesiredColliderHeight { get => desiredColliderHeight - stairMaxHeight; set => desiredColliderHeight = value; }

    [SerializeField] private float health;
    public float Health => health;

    [Header("Other Settings")]
    public float walkBobbingFrequency;
    public float walkBobbingAmplitude;
    public float runBobbingFrequency;
    public float runBobbingAmplitude;
    public float crouchBobbingFrequency;
    public float crouchBobbingAmplitude;
    #endregion
    public MonoBehaviour behaviour; 
    private void Awake()
    {
        behaviour = this;
        desiredMoveSpeed = walkSpeed;
        rigidbody = GetComponent<Rigidbody>();
        InputsController = GetComponent<InputsController>();
        collider = GetComponent<CapsuleCollider>();
        Coyote = GetComponent<CoyoteTimer>();
        SoundController = GetComponent<PlayerSoundController>();
        CameraManager = GetComponent<PlayerCameraManager>();

        DefaultColliderHeight = collider.height;
        desiredColliderHeight = DefaultColliderHeight;
        mainCamera = Camera.main.transform;
        StateMachine = new StateMachine();

        IdleState = new IdleState(this, StateMachine, InputsController);
        WalkState = new WalkState(this, StateMachine, InputsController);
        RunState = new RunState(this, StateMachine, InputsController);
        FreeFallState = new FreeFallState(this, StateMachine, InputsController);
        JumpState = new JumpState(this, StateMachine, InputsController);
        CrouchIdleState = new CrouchIdleState(this, StateMachine, InputsController);
        CrouchState = new CrouchState(this, StateMachine, InputsController);
        SlideState = new SlideState(this, StateMachine, InputsController);
        WallRunState = new WallRunState(this, StateMachine, InputsController);
        DeathState = new DeathState(this, StateMachine, InputsController);

        StateMachine.Initialization(IdleState);
    }
    private void Update()
    {
        StateMachine.Update();
    }
    private void FixedUpdate()
    {
        StateMachine.PhysicsUpdate();
    }
    public void SetDrag(float drag)
    {
        rigidbody.drag = drag;
    }
    public void SetGravity(bool gravity)
    {
        rigidbody.useGravity = gravity;
    }
    public void AdjustColliderHeight()
    {
        Vector3 targetHeight = new Vector3(0.0f, DesiredColliderHeight / 2.0f + stairMaxHeight, 0.0f);
        collider.height = Mathf.Lerp(collider.height, DesiredColliderHeight, desiredMoveSpeed * Time.deltaTime);
        collider.center = Vector3.Slerp(collider.center, targetHeight, desiredMoveSpeed * Time.deltaTime);
    }
    public void AddHealth(float value)
    {
        health += value;
    }

    public void Damage(float damage)
    {
        if (health <= 0.0f) return;
        AddHealth(-damage);
        if (health <= 0.0f) 
        {
            StateMachine.ChangeState(DeathState);
        }
    }
    public void TogglePlayer(bool value) 
    {
        rigidbody.freezeRotation = value;
        rightArmController.enabled = value;
        leftArmController.enabled = value;
        CameraManager.enabled = value;
        InputsController.enabled = value;
    }
}
