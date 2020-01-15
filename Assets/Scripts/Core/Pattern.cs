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

[System.Serializable]
public class SpriteAnimationEntity
{
    public Sprite sprite;
    public float duration;
}

public class Pattern : MonoBehaviour
{
    //어택타입에 따라 프리팹을 별개로 뒀을 경우
    //어택타입을 선택한 순간, 리소스에서 자동으로 프리팹이 넣어지는 것으로 할 수 있을듯
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
    //이렇게 나누지 말고, 패턴을 역시 공격 판정으로 쓸까?
    //공격 판정 때는 콜라이더 활성화, 아닐 때는 비활성화
    public Monster monster;

    public Attack attackPrefab;     
    public AttackType attackType;
    public ShootType shootType;     //지금 문제 있음 (밀리 시에도 보임)
    public bool defendable;
    public float preDelay;
    public float postDelay;
    public float duration;
    public Vector2 attackSize;
    public Vector2 attackOffset;    //오른쪽 방향을 기준으로 한다
    public float damage;

    [SerializeField]
    public List<SpriteAnimationEntity> spriteAnimation;

    public bool isPatternPlaying = false;   //얘는 감추는 게 좋을 듯

    public void PatternPlay()
    {
        isPatternPlaying = true;

        //얘 고쳐야 함
        Attack attack = Instantiate(attackPrefab, monster.transform.position + (Vector3)attackOffset, Quaternion.identity) as Attack;
        attack.Init(duration, damage, defendable);

        StartCoroutine(AnimationPlay());
    }

    private IEnumerator AnimationPlay()
    {
        SpriteRenderer renderer = monster.GetComponent<SpriteRenderer>();
        foreach (var anim in spriteAnimation)
        {
            renderer.sprite = anim.sprite;

            yield return new WaitForSeconds(anim.duration);
        }

        isPatternPlaying = false;
    }
}

[CustomEditor(typeof(Pattern))]
public class PatternEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var pattern = target as Pattern;

        if (pattern.attackType == AttackType.Ranged)
            pattern.shootType = (ShootType)EditorGUILayout.EnumPopup("  Shoot Type", pattern.shootType);

    }
}

