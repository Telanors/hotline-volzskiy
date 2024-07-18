using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.ParticleSystemJobs;
public class AIEnemy : MonoBehaviour, IHealthy, IEntityVision, IDamageable, INoiseSensitive, IStunable
{
    [SerializeField] protected EnemyBodyPart[] bodyParts;
    [SerializeField] protected Transform[] patrolPoints;
    [SerializeField] protected Transform visionAnchor;
    [SerializeField] private float health;
    public float Health => health;
    [SerializeField] private AdvanceWeapon startWeapon;
    public AdvanceWeapon StartWeapon => startWeapon;
    public StateMachine stateMachine { get; protected set; }
    public Animator animator { get; protected set; }
    public NavMeshAgent Agent { get; protected set; }
    public AIWeaponIK WeaponIK { get; protected set; }
    public PlayerSoundController SoundController { get; protected set; }

    public AIPatrolState PatrolState { get; protected set; }
    public AIChasingState ChasingState { get; protected set; }
    public AIStunState StunState { get; protected set; }
    public AIEquipment Equipment { get; protected set; }
    public IEntityVision Vision { get => this; }
    public Transform visionTransform { get => visionAnchor; }
    public Vector3 forward => visionAnchor.forward;
    public Vector3 right => visionAnchor.right;
    public Vector3 up => visionAnchor.up;
    public Vector3 position => visionAnchor.position;
    public LayerMask enviromentMask;

    public Player Player { get; private set; }
    private Coroutine weightCoroutine;
    public GameObject stunEffect;

    public Vector3 PlayerCenterPosition => Player.transform.position + Player.Collider.center;

    public float angleDetectionZone = 60.0f;
    public float distanceDetectionZone = 10.0f;
    public float kitingDistance = 2.0f;
    public float patrolSpeed = 1.75f;
    public float chasingSpeed = 4.5f;
    public float findSpeed = 4.5f;
    public float attackDelay = 1.0f;
    public float firingTime = 0.5f;
    public float attackTimer = 0.0f;

    public event Action<Vector3> OnNoise;

    private void Awake()
    {
        foreach (var part in bodyParts)
        {
            part.parentDamageable = this;
            part.parentStunable = this;
        }
        SoundController = GetComponent<PlayerSoundController>();
        WeaponIK = GetComponent<AIWeaponIK>();
        Equipment = GetComponent<AIEquipment>();
        Equipment.Vision = Vision;
        Agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stateMachine = new StateMachine();
        PatrolState = new AIPatrolState(this, patrolPoints.Select(x => x.position).ToArray(), patrolSpeed);
        ChasingState = new AIChasingState(this, chasingSpeed);
        StunState = new AIStunState(this);
        stateMachine.Initialization(PatrolState);
    }
    private void Start()
    {
        INoiseSensitive.noiseSensitives.Add(new(this, transform));
        Player = FindObjectOfType<Player>();
        if (startWeapon != null)
        {
            Equipment.PickUp(startWeapon);
            animator.SetBool("RangeWeapon", true);
        }
        else
        {
            Equipment.PickUp(Equipment.defaultWeapon);
            kitingDistance = 1.0f;
            attackDelay = 0.4f;
            firingTime = 0.1f;
            animator.SetBool("RangeWeapon", false);
            Equipment.ShotStart.AddListener(() => animator.SetTrigger("Melee"));
        }
    }
    public void Move(Vector3 targetPoint)
    {
        Agent.SetDestination(targetPoint);
    }
    public void Move(NavMeshPath path)
    {
        Agent.SetPath(path);
    }
    public void AddHealth(float value)
    {
        health += value;
    }

    private void Update()
    {
        animator.SetFloat("Speed", Agent.velocity.magnitude);
        if(Agent.velocity.sqrMagnitude != 0.0f)
        {
            SoundController.FootStepPlay();
        }
        stateMachine.Update();
    }
    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
    private void LateUpdate()
    {
        stateMachine.LateUpdate();
    }
    public void Damage(float damage)
    {
        AddHealth(-damage);
        if (Health <= 0.0f)
        {
            INoiseSensitive.noiseSensitives.Remove(new(this, transform));
            StopAllCoroutines();
            RagDollSwitchState(false);
            Equipment.Drop(Vector3.zero);
            foreach (var part in bodyParts)
            {
                Destroy(part);
            }
            Destroy(this);
            Destroy(animator);
            Destroy(Agent);
            Destroy(Equipment);
            Destroy(WeaponIK);
        }
    }
    private void RagDollSwitchState(bool value)
    {
        foreach (var part in bodyParts)
        {
            part.SwitchKinematic(value);
        }
    }
    public void NoiseReact(Vector3 noisePoint)
    {
        OnNoise?.Invoke(noisePoint);
    }

    public void StateWeightChange(string layerName, float targetWeight, float multiply)
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        float currentWeight = animator.GetLayerWeight(layerIndex);
        if (currentWeight == targetWeight) return;
        if (weightCoroutine != null)
        {
            StopCoroutine(weightCoroutine);
        }
        weightCoroutine = StartCoroutine(StateWeightChangeCoroutine(layerIndex, currentWeight, targetWeight, multiply));
    }
    private IEnumerator StateWeightChangeCoroutine(int layerIndex, float currentWeight, float targetWeight, float multiply)
    {
        float timer = 0.0f;
        while (timer <= 1.0f)
        {
            timer += Time.deltaTime * multiply;
            currentWeight = Mathf.Lerp(currentWeight, targetWeight, timer);
            animator.SetLayerWeight(layerIndex, currentWeight);
            yield return null;
        }
        weightCoroutine = null;
    }

    public void Stun()
    {
        if (stateMachine.CurrentState != StunState)
        {
            Instantiate(stunEffect, visionAnchor.position, visionAnchor.rotation, visionAnchor);
            Equipment.Drop((PlayerCenterPosition - transform.position).normalized);
            Equipment.PickUp(Equipment.defaultWeapon);
            kitingDistance = 1.0f;
            attackDelay = 0.4f;
            firingTime = 0.1f;
            animator.SetBool("RangeWeapon", false);
            Equipment.ShotStart.AddListener(() => animator.SetTrigger("Melee"));
            stateMachine.ChangeState(StunState);
        }
    }
}
