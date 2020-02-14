using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Monster : Creature
{
    public List<Pattern> patterns = new List<Pattern>();

    public bool isPatternPlaying = false;

    protected override void Start()
    {
        base.Start();

        creatureTag = CreatureTag.MONSTER;

        //StartCoroutine(PatrolRoutine());
    }

    public void Attack(int index)
    {
        StartCoroutine(AttackRoutine(index));
    }

    private IEnumerator AttackRoutine(int index)
    {
        isPatternPlaying = true;
        animationStatus = AnimationStatus.ATTACK;

        patterns[index].PatternPlay();

        yield return new WaitUntil(() => isPatternPlaying);

        animationStatus = AnimationStatus.NONE;
    }

    //임시
    protected override void Update()
    {
        base.Update();

        /* 공격 테스트용
        if (!isPatternPlaying)
        {
            StartCoroutine(Routine());
        }
        */
    }

    //임시
    /* 공격 테스트용
    IEnumerator Routine()
    {
        patterns[0].PatternPlay();

        yield return new WaitUntil(() => !isPatternPlaying);
    }*/

    IEnumerator PatrolRoutine()
    {
        float timer = 0f;


        while (timer <= 1f)
        {
            Move(Vector2.left);

            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;

        while (timer <= 1f)
        {
            Move(Vector2.right);

            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;

        while (timer <= 1f)
        {
            Move(Vector2.zero);

            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;

        StartCoroutine(PatrolRoutine());
    }
}

[CustomEditor(typeof(Monster))]
public class MonsterEditor : Editor
{
    Pattern pattern = null;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var monster = target as Monster;
        
        if (GUILayout.Button("Add Pattern"))
        {
            bool isCountChanged = false;

            List<int> missingNumbers = new List<int>();
            int num = 0;
            foreach (var p in monster.patterns)
            {
                if (p == null)
                {
                    missingNumbers.Add(num);

                    isCountChanged = true;
                }
                num++;
            }

            int count = 0;
            foreach (var n in missingNumbers)
            {
                monster.patterns.Remove(monster.patterns[n - count++]);     //이 짓을 해야지 안 씹힘
            }

            int c = 1;
            if (isCountChanged)
            {
                foreach (var p in monster.patterns)
                {
                    p.patternIndex = c - 1;
                    p.gameObject.name = $"Pattern_{c++}";
                }
            }
            else {
                c = monster.patterns.Count + 1;
            }

            pattern = new GameObject("Pattern_" + c).AddComponent<Pattern>();
            pattern.patternIndex = c - 1;
            pattern.transform.parent = monster.transform;
            pattern.transform.position = monster.transform.position;
            pattern.monster = monster;
            monster.patterns.Add(pattern);
        }

        if (GUILayout.Button("Apply Change"))
        {
            bool isCountChanged = false;

            List<int> missingNumbers = new List<int>();
            int num = 0;
            foreach (var p in monster.patterns)
            {
                if (p == null)
                {
                    missingNumbers.Add(num);

                    isCountChanged = true;
                }
                num++;
            }

            int count = 0;
            foreach (var n in missingNumbers)
            {
                monster.patterns.Remove(monster.patterns[n - count++]);     //이 짓을 해야지 안 씹힘
            }

            int c = 1;
            if (isCountChanged)
            {
                foreach (var p in monster.patterns)
                {
                    p.gameObject.name = $"Pattern_{c++}";
                }
            }
        }
    }
}
