using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum AttackType
{
    Melee, Ranged
}

public enum ShootType
{
    Direct, Homing
}

public enum PatternSpawnType
{
    AtMonster, AtAttack
}

[System.Serializable]
public class SpriteAnimationEntity
{
    public Sprite sprite;
    public float duration;
}

public class Pattern : MonoBehaviour
{
    /*
    + 경직치? (타임 스케일 조정치)
    + 파생 공격? (폭발 등의 공격에서 파생되는 공격)
    + 공격 조건? (공격 인식 범위 등)
    + 스턴?
    + 넉백?
    + 지속 공격 여부?
     */
    //공격 명령 - 패턴
    //공격 판정 - 어택
    public Monster monster;

    public Attack attackPrefab;      //투사체 여러 발일 경우 1. 한 패턴으로 처리 2. 여러 패턴으로 처리
    public AttackType attackType;
    //public ShootType shootType;     //지금 문제 있음 (밀리 시에도 보임)
    public bool defendable;
    public float preDelay;
    public float postDelay;
    public float duration;
    public Vector2 attackSize;
    public Vector2 attackOffset;    //오른쪽 방향을 기준으로 한다
    public float damage;



    [SerializeField]
    public List<SpriteAnimationEntity> spriteAnimation;

    public Pattern nextPattern;
    public PatternSpawnType nextPatternType; //다음 패턴 공격 판정이 어디서 생길지

    private void OnEnable()
    {
        attackPrefab.gameObject.SetActive(false);
    }

    public void PatternPlay()
    {
        //일단 지금은 무조건 몬스터 기준으로 생성
        Vector2 monsterPos = monster.transform.position;
        Vector2 offset = Vector2.zero;

        switch (monster.direction)
        {
            case Direction.LEFT:
                offset = new Vector2(-attackOffset.x, attackOffset.y);
                break;
            case Direction.RIGHT:
                offset = new Vector2(attackOffset.x, attackOffset.y);
                break;
        }

        transform.GetChild(0).position = monsterPos + offset;

        StartCoroutine(PatternRoutine());
    }

    private IEnumerator PatternRoutine()
    {
        monster.isPatternPlaying = true;

        yield return new WaitForSeconds(preDelay);
        yield return new WaitUntil(() => monster.timeScale == 1f);

        float timer = duration;

        attackPrefab.gameObject.SetActive(true);

        SpriteRenderer renderer = monster.GetComponent<SpriteRenderer>();
        foreach (var anim in spriteAnimation)
        {
            renderer.sprite = anim.sprite;

            yield return new WaitForSeconds(anim.duration);
            yield return new WaitUntil(() => monster.timeScale == 1f);

            timer -= anim.duration;
        }

        yield return new WaitForSeconds(timer);     //혹시 애니메이션 지속시간이랑 공격 지속 시간이 다를 경우
        yield return new WaitUntil(() => monster.timeScale == 1f);

        yield return new WaitForSeconds(postDelay);
        yield return new WaitUntil(() => monster.timeScale == 1f);

        NextPatternPlay();
        //몬스터에서 Idle로 전환해줘야 함
    }

    private void NextPatternPlay()
    {
        if (nextPattern == null)
        {
            monster.isPatternPlaying = false;
            return;
        }

        switch (nextPatternType)
        {
            case PatternSpawnType.AtMonster:
                nextPattern.PatternPlay();
                break;
        }

    }
}

[CustomEditor(typeof(Pattern))]
public class PatternEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var pattern = target as Pattern;

        //이거에 맞춰서 패턴을 재구성해야함
        if (GUILayout.Button("Apply"))
        {
            if (pattern.attackPrefab != null)
                DestroyImmediate(pattern.attackPrefab.gameObject);

            GameObject prefab = null;
            if (pattern.attackType == AttackType.Melee)
            {
                prefab = Resources.Load("Prefabs/AttackPrefab") as GameObject;
            }
            Attack attack = Instantiate(prefab).GetComponent<Attack>();
            attack.transform.parent = pattern.transform;
            attack.transform.position = pattern.transform.position + (Vector3)pattern.attackOffset;
            attack.transform.localScale = pattern.attackSize;

            pattern.attackPrefab = attack;
            pattern.attackPrefab.Init(pattern.duration, pattern.damage, pattern.defendable);
        }
    }
}

