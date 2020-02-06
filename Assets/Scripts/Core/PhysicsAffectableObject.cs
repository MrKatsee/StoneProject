using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsAffectableObject : MonoBehaviour
{
    //모든 물리 연산이 아닌, Y축 물리 연산만 담당한다.
    //대표적으로 점프, 낙하
    //Vector2로 하는 건 혹시 모르니까
    private LayerMask _layerMask;

    private Collider2D _col;
    private Vector2 _colSize;

    private static Vector2 _gravity = new Vector2(0f, -9.8f);

    [Header("Physics")]
    public float gravityScale = 1f;
    public Vector2 velocity = Vector2.zero;
    public float timeScale = 1f;

    public void AddStun(float time)
    {
        if (stunRoutine != null) StopCoroutine(stunRoutine);

        stunRoutine =  StartCoroutine(StunRoutine(time));
    }

    Coroutine stunRoutine;
    private IEnumerator StunRoutine(float time)
    {
        timeScale = 0f;

        yield return new WaitForSeconds(time);

        timeScale = 1f;
    }

    public void AddVelocity(Vector2 v)
    {
        velocity += v;
    }

    private void YAxisMove()
    {
        Vector2 moveVec = (velocity + (_gravity * gravityScale * MyTime.deltaTime * timeScale)) * MyTime.deltaTime;
        bool? checkResult = YAxisCollisionCheck();
        if (checkResult == true)
        {
            if (moveVec.y < 0f)
            {
                velocity = Vector2.zero;
                return;
            }
        }
        else if (checkResult == false)
        {
            if (moveVec.y > 0f)
            {
                velocity = Vector2.zero;
                return;
            }
        }
        velocity += _gravity * gravityScale * MyTime.deltaTime * timeScale;
        transform.Translate(moveVec);
    }

    [SerializeField]
    private Collider2D _floorCollider = null;

    //바닥으로 인식한 것은, 바닥으로 작용할 때까지(플레이어와 떨어질 때까지) 갖고 있음
    //true라면, 아래에 발판, false라면, 위에 천장
    private bool? YAxisCollisionCheck()
    {
        /*
        if (floorCollider != null)
        {
            if (!floorCollider.IsTouching(col))
            {
                floorCollider = null;
            }
            else
                return true;
        }
        */
        float distance = _colSize.y * 0.5f;

        Vector2 dir = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distance, _layerMask);
        if (hit)
        {
            if (hit.distance <= distance)
            {
                transform.Translate(new Vector2(0f, distance - hit.distance));
            }

            _floorCollider = hit.collider;
            return true;
        }

        dir = Vector2.up;
        hit = Physics2D.Raycast(transform.position, dir, distance, _layerMask);
        if (hit)
        {
            return false;
        }

        return null;
    }

    public virtual void Init()
    {
        velocity = Vector2.zero;
        gravityScale = 1f;

        _col = GetComponent<Collider2D>();
        _colSize = _col.bounds.size;

        _layerMask = ~(1 << LayerMask.NameToLayer("PhysicalAffectable") | 1 << LayerMask.NameToLayer("NonPhysicalAffectable"));
        gameObject.layer = LayerMask.NameToLayer("PhysicalAffectable");

        _physicsAvailable = true;
    }

    protected virtual void Start()
    {

    }

    private bool _physicsAvailable = false;
    protected virtual void Update()
    {
        if (_physicsAvailable)
            YAxisMove();
    }
}
