﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : PhysicsAffectableObject
{
    [Header("CreatureSetting")]
    public SpriteRenderer renderer;

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


    protected virtual void Move(Vector2 moveVec)
    {
        renderer.flipX = moveVec.x < 0f;

        transform.Translate(moveVec * MyTime.deltaTime * timeScale * _curSpd);
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