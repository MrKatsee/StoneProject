using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    protected override void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            c.GetComponent<Player>().GetDamage(_damage);
            gameObject.SetActive(false);
        }
    }
}
