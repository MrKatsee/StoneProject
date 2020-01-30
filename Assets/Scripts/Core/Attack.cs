using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private float _damage;
    [SerializeField]
    private bool _defendable;
    [SerializeField]
    private float _duration;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        gameObject.layer = LayerMask.NameToLayer("NonPhysicalAffectable");
    }

    //Attack Sprite, Animation도 받아야 함
    public virtual void Init(float duration, float damage, bool defendable)
    {
        _duration = duration;
        _damage = damage;
        _defendable = defendable;
    }

    public virtual void OnEnable()
    {
        StartCoroutine(AttackRoutine(_duration));
    }

    protected virtual IEnumerator AttackRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        DestroyCallback();
    }

    public virtual void DestroyCallback()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            c.GetComponent<Player>().GetDamage(_damage);
            gameObject.SetActive(false);
        }
    }
}