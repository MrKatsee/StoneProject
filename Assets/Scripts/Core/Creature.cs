using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    RIGHT, LEFT
}

public enum CreatureTag
{
    CREATURE, MONSTER, PLAYER
}

public enum AnimationStatus
{
    NONE, IDLE, MOVE, ATTACK
}

public class Creature : PhysicsAffectableObject
{

    [Header("CreatureStatus")]
    [SerializeField]
    protected float _hp = 10f;
    protected float HP
    {
        get { return _hp; }
        set
        {
            _hp = value;
        }
    }
    [SerializeField]
    protected float _atk = 1f;
    [SerializeField]
    protected float _spd = 3f;

    [SerializeField]
    protected float _curHp;
    [SerializeField]
    protected float _curAtk;
    [SerializeField]
    protected float _curSpd;

    public Direction direction = Direction.RIGHT;
    public bool movable = true;

    public CreatureTag creatureTag = CreatureTag.CREATURE;

    [SerializeField]
    public List<SpriteAnimationEntity> idle_spriteAnimation;

    [SerializeField]
    public List<SpriteAnimationEntity> move_spriteAnimation;

    protected virtual void Move(Vector2 moveVec)
    {
        if (!movable) return;
        if (moveVec == Vector2.zero)
        {
            PlayAnimation(AnimationStatus.IDLE, idle_spriteAnimation);
            return;
        }

        PlayAnimation(AnimationStatus.MOVE, move_spriteAnimation);

        bool isDirectionLeft = moveVec.x < 0f;

        switch (isDirectionLeft)
        {
            case true:
                direction = Direction.LEFT;
                break;
            case false:
                direction = Direction.RIGHT;
                break;
        }

        spriteRenderer.flipX = isDirectionLeft;

        transform.Translate(moveVec * MyTime.deltaTime * TimeScale * _curSpd);
    }

    [SerializeField]
    protected AnimationStatus animationStatus = AnimationStatus.NONE;

    private Coroutine animationRoutine;
    protected void PlayAnimation(AnimationStatus status, List<SpriteAnimationEntity> animation)
    {
        if (animationStatus == AnimationStatus.ATTACK)
        {
            if (animationRoutine != null)
            {
                StopCoroutine(animationRoutine);
                animationRoutine = null;
            }
            return;
        }
        if (status == animationStatus) return;

        animationStatus = status;

        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
            animationRoutine = null;
        }

        if (animation.Count == 0) return;

        animationRoutine = StartCoroutine(PlayAnimationRoutine(animation));
    }
    private IEnumerator PlayAnimationRoutine(List<SpriteAnimationEntity> animation)
    {
        float count = 0f;

        foreach (var anim in animation)
        {
            if (creatureTag == CreatureTag.PLAYER)

            spriteRenderer.sprite = anim.sprite;

            yield return new WaitUntil(() => TimeScale == 1f);
            yield return new WaitForSeconds(anim.duration);
        }

        animationRoutine = StartCoroutine(PlayAnimationRoutine(animation));
    }

    protected virtual void Jump(float vel = 5f)
    {
        if (velocity == Vector2.zero)
        {
            Vector2 vec = new Vector2(0f, vel);
            AddVelocity(vec);
        }
    }

    protected override void Start()
    {
        base.Start();
        Init();
    }

    protected override void Update()
    {
        base.Update();

    }

    public override void Init()
    {
        base.Init();

        _curHp = _hp;
        _curAtk = _atk;
        _curSpd = _spd;
    }

    public virtual void GetDamage(float damage)
    {
        HP -= damage;
    }
}
