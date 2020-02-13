using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    private bool isAttacking = false;
    private bool isDashing = false;
    private bool isDashCooltime = false;

    private void Move()
    {
        Vector2 moveVec = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) moveVec += Vector2.left;
        if (Input.GetKey(KeyCode.RightArrow)) moveVec += Vector2.right;

        Move(moveVec);
    }

    //대쉬 거리 조정 필요
    private float dash_time = 0.125f;
    private float dash_spd_start = 30f;
    private float dash_spd_end = 0f;
    private float dash_postDelay = 0.125f;
    private void Dash()
    {
        if (isDashing) return;
        if (isDashCooltime) return;

        if (Input.GetKey(KeyCode.LeftShift)) StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        StopAttackRoutine();

        isDashing = true;
        isDashCooltime = true;

        float timer = 0f;
        while(timer <= dash_time)
        {
            timer += Time.deltaTime;

            Vector2 moveVec = Vector2.right;
            if (direction == Direction.LEFT)
                moveVec = Vector2.left;
            else moveVec = Vector2.right;

            float dash_spd = Mathf.Lerp(dash_spd_start, dash_spd_end, timer / dash_time);

            transform.Translate(moveVec * MyTime.deltaTime * TimeScale * dash_spd);

            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(dash_postDelay);

        isDashCooltime = false;
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)) base.Jump();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animationStatus = AnimationStatus.ATTACK;

            Attack1();
        }
    }

    private void Init_Player()
    {
        creatureTag = CreatureTag.PLAYER;

        attack1Prefab.Init(attack1_duration, attack1_damage, false, this);
    }

    [Header("AttackSetting")]
    public Attack attack1Prefab;

    private float attack1_duration = 0.2f;
    private float attack1_preDelay = 0f;
    private float attack1_postDelay = 0f;
    private float attack1_damage = 0f;

    [SerializeField]
    public List<SpriteAnimationEntity> attack1_spriteAnimation;

    private void Attack1()
    {
        attack1Routine = StartCoroutine(Attack1Routine());
    }

    Coroutine attack1Routine;
    private IEnumerator Attack1Routine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attack1_preDelay);

        float timer = attack1_duration;

        attack1Prefab.gameObject.SetActive(true);

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        foreach (var anim in attack1_spriteAnimation)
        {
            renderer.sprite = anim.sprite;

            yield return new WaitForSeconds(anim.duration);

            timer -= anim.duration;
        }

        yield return new WaitForSeconds(timer);     //혹시 애니메이션 지속시간이랑 공격 지속 시간이 다를 경우

        yield return new WaitForSeconds(attack1_postDelay);

        StopAttackRoutine();
    }

    private void StopAttackRoutine()
    {
        if (attack1Routine != null)
        {
            StopCoroutine(attack1Routine);
            attack1Routine = null;
        }

        animationStatus = AnimationStatus.NONE;

        attack1Prefab.gameObject.SetActive(false);
        isAttacking = false;
    }

    protected override void Start()
    {
        base.Start();

        Init_Player();
    }

    protected override void Update()
    {
        base.Update();

        if (isDashing) return;

        Move();
        Jump();
        Dash();

        if (isAttacking) return;

        Attack();
    }

}
