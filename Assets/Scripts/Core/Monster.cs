using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Monster : Creature
{
    public List<Pattern> patterns = new List<Pattern>();

    private void Update()
    {
        if (!patterns[0].isPatternPlaying)
        {
            StartCoroutine(Routine());
        }
    }

    IEnumerator Routine()
    {
        patterns[0].PatternPlay();

        yield return new WaitUntil(() => !patterns[0].isPatternPlaying);
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
            int count = monster.patterns.Count + 1;
            pattern = new GameObject("Pattern_" + count).AddComponent<Pattern>();
            pattern.transform.parent = monster.transform;
            pattern.monster = monster;
            monster.patterns.Add(pattern);
        }
    }
}
