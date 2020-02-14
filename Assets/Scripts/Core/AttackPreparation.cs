using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPreparation : MonoBehaviour
{
    public Monster _monster;
    public Pattern _pattern;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        gameObject.layer = LayerMask.NameToLayer("NonPhysicalAffectable");
    }

    public void Init(Monster monster, Pattern pattern)
    {
        _monster = monster;
        _pattern = pattern;
    }

    protected virtual void OnTriggerEnter2D(Collider2D c)
    {
        //인식 범위 안으로 들어오면 패턴 인덱스를 몬스터에게 줌
        if (c.tag == "Player")
        {
            _monster.Attack(_pattern.patternIndex);
        }
    }
}
