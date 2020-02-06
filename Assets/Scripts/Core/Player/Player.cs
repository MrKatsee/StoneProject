using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    private bool isAttacking = false;
    private bool isDashing = false;

    private void Move()
    {
        Vector2 moveVec = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) moveVec += Vector2.left;
        if (Input.GetKey(KeyCode.RightArrow)) moveVec += Vector2.right;

        Move(moveVec);
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)) base.Jump();
    }

    private void Attack()
    {
        if (Input.GetKey(KeyCode.A)) Attack1();
    }

    public GameObject attackPrefab;

    private float attack1_duration = 0.2f;
    private float attack1_preDelay = 0f;
    private float attack1_postDelay = 0f;

    [SerializeField]
    public List<SpriteAnimationEntity> attack1_spriteAnimation;

    private void Attack1()
    {
        StartCoroutine(Attack1Routine());
    }

    private IEnumerator Attack1Routine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attack1_preDelay * timeScale);

        float timer = attack1_duration;

        attackPrefab.SetActive(true);

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        foreach (var anim in attack1_spriteAnimation)
        {
            renderer.sprite = anim.sprite;

            yield return new WaitForSeconds(anim.duration * timeScale);

            timer -= anim.duration;
        }

        yield return new WaitForSeconds(timer * timeScale);     //혹시 애니메이션 지속시간이랑 공격 지속 시간이 다를 경우

        yield return new WaitForSeconds(attack1_postDelay * timeScale);

        attackPrefab.SetActive(false);

        isAttacking = false;
    }

    protected override void Update()
    {
        base.Update();

        if (isAttacking) return;

        Attack();
        Move();
        Jump();
    }

}
