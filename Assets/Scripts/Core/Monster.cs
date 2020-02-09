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
    }

    //임시
    protected override void Update()
    {
        base.Update();

        if (!isPatternPlaying)
        {
            StartCoroutine(Routine());
        }
    }

    //임시
    IEnumerator Routine()
    {
        patterns[0].PatternPlay();

        yield return new WaitUntil(() => !isPatternPlaying);
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
                    p.gameObject.name = $"Pattern_{c++}";
                }
            }
            else {
                c = monster.patterns.Count + 1;
            }

            pattern = new GameObject("Pattern_" + c).AddComponent<Pattern>();
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
