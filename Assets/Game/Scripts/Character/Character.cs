using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Character : MonoBehaviour
{

    private CharacterController _cc;
    public float moveSpeed;
    [SerializeField]
    private Vector3 movmentVolocity;
    private PlayerInput _playerInput;

    private float _verticalVelocity;


    public float Gravity = -30f;

    Animator _animator;


    public int coin;

    


    //-----------  Enemy

    public bool isPlayer = true;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform playerTarget;



    //--- attack
    private float attackStartTime;
    [SerializeField]
    private float attackSliderSpeed = 0.06f;
    public float attackSliderDuration = 0.4f;


    //---Health

    private Health _health;


    //--DamageCaster
    private DamageCoster _damageCoster;

    public enum CharacterState
    {
        Normal,
        Attacking,
        Dead,
        BeingHit,
        Slide,
        Spawn
    }

    public CharacterState currentState;


    //-- material
    [SerializeField]
    private SkinnedMeshRenderer skinnedMesh;
    private MaterialPropertyBlock propertyBlock;


    //-- Healorb
    public GameObject itemToDrop;


    private Vector3 impactOnCharacter;

    private bool isInvincible;
    private float invincibleDuration = 2f;


    private float attackanimationDuration;


    public float sliderSpeed = 9f;


    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _damageCoster = GetComponentInChildren<DamageCoster>();
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        skinnedMesh.GetPropertyBlock(propertyBlock);

        if (!isPlayer)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            playerTarget = GameObject.FindWithTag("Player").transform;
            SwitchStateTo( CharacterState.Spawn);
        }
        else
        {
            _playerInput = GetComponent<PlayerInput>();
        }

       
    }


    private void CalucteEnemyMovement()
    {
        if (Vector3.Distance(transform.position, playerTarget.position) > _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(playerTarget.position);
            _animator.SetFloat("Speed", 0.2f);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetFloat("Speed", 0);

            SwitchStateTo(CharacterState.Attacking);

        }
    }


    void CalculuteMovmentVolocity()
    {
        if (_playerInput.MouseButtonDown && _cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Attacking);
            return;
        }
        else if (_playerInput.SpaceButtonDown&&_cc.isGrounded)
        {
            SwitchStateTo( CharacterState.Slide);
            return;
        }


        movmentVolocity.Set(_playerInput.HorizontalValue, 0, _playerInput.VerticalValue);
        movmentVolocity.Normalize();
        movmentVolocity = Quaternion.Euler(0, -45, 0) * movmentVolocity;

        _animator.SetFloat("Speed", movmentVolocity.magnitude);

        movmentVolocity = movmentVolocity * Time.deltaTime * moveSpeed;

        if (movmentVolocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movmentVolocity);
        }

        _animator.SetBool("AirBorne", !_cc.isGrounded);

    }

    private void FixedUpdate()
    {

        switch (currentState)
        {
            case CharacterState.Normal:
                if (!isPlayer)
                {
                    CalucteEnemyMovement();
                }
                else
                {
                    CalculuteMovmentVolocity();
                }
                break;

            case CharacterState.Attacking:
                if (isPlayer)
                {
                    
                    if (Time.time < attackStartTime + attackSliderDuration)
                    {
                        float passTime = Time.time - attackStartTime;
                        float rote = passTime / attackSliderDuration;
                        movmentVolocity = Vector3.Lerp(attackSliderSpeed * transform.forward, Vector3.zero, passTime);
                    }

                    if (_playerInput.MouseButtonDown&&_cc.isGrounded)
                    {
                        string currentClipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        attackanimationDuration = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                        if (currentClipName!=""&&attackanimationDuration>0.5f&attackanimationDuration<0.7f)
                        {
                            _playerInput.MouseButtonDown = false;
                            SwitchStateTo( CharacterState.Attacking);

                            //CalculuteMovmentVolocity();
                        }

                    }
                }

                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
               
                break;

            case CharacterState.Slide:
                movmentVolocity = sliderSpeed * Time.deltaTime * transform.forward;

                break;
            case CharacterState.Spawn:
                if (currentSpawnTime>0)
                {
                    currentSpawnTime -= Time.deltaTime;
                }
                else
                {
                    SwitchStateTo( CharacterState.Normal);
                }
                break;
        }


        if (impactOnCharacter.magnitude > 0.2f)
        {
            movmentVolocity = impactOnCharacter * Time.deltaTime;
        }
        impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero, Time.deltaTime * 5);


        if (isPlayer)
        {
            if (!_cc.isGrounded)
            {
                _verticalVelocity = Gravity;
            }
            else
            {
                _verticalVelocity = Gravity * 0.3f;
            }

            movmentVolocity += _verticalVelocity * Time.deltaTime * Vector3.up;

            _cc.Move(movmentVolocity);
            movmentVolocity = Vector3.zero;

           
        }
        else
        {
            if (currentState != CharacterState.Normal)
            {
                _cc.Move(movmentVolocity);
                movmentVolocity = Vector3.zero;
            }
        }
    }




    public void SwitchStateTo(CharacterState newState)
    {

        if (isPlayer)
        {
            _playerInput.ClearCache();

        }

        //--Exit
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                if (_damageCoster != null)
                {
                    DisableDamageCaster();
                }
                if (isPlayer)
                {
                    
                    GetComponent<PlayerVFXManager>().StopBlade();
                }
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
                break;
            case CharacterState.Slide:
                break;
            case CharacterState.Spawn:
                isInvincible = false;
                break;
        }


        //--Enter
        switch (newState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                _animator.SetTrigger("Attack");
                if (isPlayer)
                {
                    RotateToCourse();
                    attackStartTime = Time.time;
                }
                if (!isPlayer)
                {
                    Quaternion quater = Quaternion.LookRotation(playerTarget.position - transform.position);
                    transform.rotation = quater;
                }
                break;

            case CharacterState.Dead:
                _cc.enabled = false;
                _animator.SetTrigger("Dead");
                StartCoroutine(MaterialDissolve());
                if (!isPlayer)
                {
                   var skin= GetComponentInChildren<SkinnedMeshRenderer>();
                    skin.gameObject.layer = 0;
                }
                break;

            case CharacterState.BeingHit:
                _animator.SetTrigger("BeingHit");
                if (isPlayer)
                {   
                    isInvincible = true;
                    StartCoroutine(DelayCancelInvincible());
                }
                break;

            case CharacterState.Slide:
                _animator.SetTrigger("Slide");
                break;

            case CharacterState.Spawn:
                isInvincible = true;
                currentSpawnTime = spawnDuration;

                StartCoroutine(MaterialAppear());
                break;
        }

        currentState = newState;



    }

    private IEnumerator DelayCancelInvincible()
    {
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    private void AddImpact(Vector3 attackPos,float force)
    {
        Vector3 impactDir = transform.position - attackPos;
        impactDir.Normalize();
        impactDir.y = 0;
        impactOnCharacter = impactDir * force;
    }

    public void SlideAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void AttackAnimationEnds()
    {
        SwitchStateTo( CharacterState.Normal);
    }

    public void BeingHitAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void ApplyDamage(int damage, Vector3 hitPoint = new Vector3())
    {
        if (isInvincible)
        {
            return;
        }
        if (_health!=null) 
        {
            _health.ApplyDamage(damage);
        }
        if (!isPlayer)
        {
            GetComponent<EnemyVFXManager>().PlayBeingHit(hitPoint);
        }

        StartCoroutine(MaterialBink());

        if (isPlayer)
        {   
            SwitchStateTo( CharacterState.BeingHit);
            AddImpact(hitPoint,10f);
        }
        else
        {
            AddImpact(hitPoint, 2.5f);
        }
    }

    public void EnableDamageCaster()
    {
        if (_damageCoster!=null)
        {
           
            _damageCoster.EnableDamageCaster();
        }
    }

    public void DisableDamageCaster()
    {
        if (_damageCoster != null)
        {
            _damageCoster.DisableDamageCaster();
        }
    }
   

    IEnumerator MaterialBink()
    {
        propertyBlock.SetFloat("_blink", 0.4f);
        skinnedMesh.SetPropertyBlock(propertyBlock);

        yield return new WaitForSeconds(0.2f);
        propertyBlock.SetFloat("_blink", 0);
        skinnedMesh.SetPropertyBlock(propertyBlock);
    }


    IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2);
        float duration = 2f;
        float currentTime = 0;
        float hightStart = 20f;
        float hightTarget = -10f;
        float hight = 0;

        propertyBlock.SetFloat("_enableDissolve",1f);
        skinnedMesh.SetPropertyBlock(propertyBlock);

        while (currentTime<duration)
        {
            currentTime += Time.deltaTime;
            hight = Mathf.Lerp(hightStart, hightTarget, currentTime / duration);
            propertyBlock.SetFloat("_dissolve_height",hight);
            skinnedMesh.SetPropertyBlock(propertyBlock);
            yield return null;
        }

        DropItem();
    }
     
    private void DropItem()
    {
        if (itemToDrop!=null)
        {
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
            Debug.Log(">>>>>>>>>>: ");
        }
    }

    internal void PickUpItem(PickUp pickUp)
    {
        switch (pickUp.type)
        {
            case PickUp.PickUpType.Heal:
                AddHealth(pickUp.value); 
                break;
            case PickUp.PickUpType.Coin:
                AddCoin(pickUp.value);
                break;
            default:
                break;
        }
    }

    private void AddHealth(int heal)
    {
        _health.Addhealth(heal);
        if (GetComponent<PlayerVFXManager>()!=null)
        {
            GetComponent<PlayerVFXManager>().PlayHeal();
        }
    }

    private void AddCoin(int c)
    {
        coin += c;
    }

    public void RotateToTarget()
    {
        if (currentState!= CharacterState.Dead)
        {
            transform.LookAt(playerTarget,Vector3.up);
        }
    }

    float spawnDuration=2f;
    float currentSpawnTime;

    IEnumerator MaterialAppear()
    {
        float dissolveTimeDuration = spawnDuration;
        float currentDissolveTime = 0;
        float disslveHight_start = -10;
        float dissolveHight_target = 20f;
        float dissolveHight;

        propertyBlock.SetFloat("_enableDissolve",1);
        skinnedMesh.SetPropertyBlock(propertyBlock);
        while (currentDissolveTime<dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHight = Mathf.Lerp(disslveHight_start, dissolveHight_target, currentDissolveTime / dissolveTimeDuration);
            propertyBlock.SetFloat("_dissolve_height", dissolveHight);
            skinnedMesh.SetPropertyBlock(propertyBlock);
            yield return null;
        }

        propertyBlock.SetFloat("_enableDissolve", 0);
        skinnedMesh.SetPropertyBlock(propertyBlock);
    }


    private void RotateToCourse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,1000f,1<<10))
        {
            transform.rotation = Quaternion.LookRotation(hit.point - transform.position, Vector3.up);
        }
    }
}
