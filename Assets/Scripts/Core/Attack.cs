using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float _damage;
    private bool _defendable;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        gameObject.layer = LayerMask.NameToLayer("NonPhysicalAffectable");
    }

    public void Init(float duration, float damage, bool defendable)
    {
        _damage = damage;
        _defendable = defendable;

        StartCoroutine(AttackRoutine(duration));
    }

    private IEnumerator AttackRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        DestroyCallback();
    }

    private void DestroyCallback()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            c.GetComponent<Player>().GetDamage(_damage);
            DestroyCallback();
        }
    }
}