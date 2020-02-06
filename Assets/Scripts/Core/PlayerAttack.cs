using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack
{
    protected override void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "MonsterAttack")
        {
            Debug.Log("1");

            if (c.GetComponent<Attack>()._defendable)
            {
                Debug.Log("2");

                c.GetComponent<Attack>().monster.AddStun(1f);
                c.GetComponent<Attack>().DestroyCallback();
            }
        }
    }
}
