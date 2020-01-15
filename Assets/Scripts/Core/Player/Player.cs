using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    private void Move()
    {
        Vector2 moveVec = Vector2.zero;
        if (Input.GetKey(KeyCode.A)) moveVec += Vector2.left;
        if (Input.GetKey(KeyCode.D)) moveVec += Vector2.right;

        Move(moveVec);
    }

    protected override void Update()
    {
        Move();
        Jump();
    }

}
