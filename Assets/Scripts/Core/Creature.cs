using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    protected float _hp = 10f;
    protected float _atk = 1f;
    protected float _spd = 3f;

    protected float _curHp;
    protected float _curAtk;
    protected float _curSpd;

    protected virtual void Move(Vector2 moveVec)
    {
        transform.Translate(moveVec * MyTime.deltaTime * _curSpd);
    }

    protected bool _isJumpable = false;
    protected void Jump()
    {

    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Update()
    {

    }

    public void Init()
    {
        _curHp = _hp;
        _curAtk = _atk;
        _curSpd = _spd;
    }
}
