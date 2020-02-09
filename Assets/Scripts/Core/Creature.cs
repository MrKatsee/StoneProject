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



public class Creature : PhysicsAffectableObject
{

    [Header("CreatureStatus")]
    protected float _hp = 10f;
    protected float HP
    {
        get { return _hp; }
        set
        {
            _hp = value;
        }
    }
    protected float _atk = 1f;
    protected float _spd = 3f;

    protected float _curHp;
    protected float _curAtk;
    protected float _curSpd;

    public Direction direction = Direction.RIGHT;
    public bool movable = true;

    public CreatureTag creatureTag = CreatureTag.CREATURE;

    protected virtual void Move(Vector2 moveVec)
    {
        if (!movable) return;
        if (moveVec.x == 0f)
            return;

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
