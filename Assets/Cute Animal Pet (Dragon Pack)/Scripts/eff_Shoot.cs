using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eff_Shoot : MonoBehaviour
{

    [SerializeField]
    float _speed = 500f;
    [SerializeField]
    float _shootWaitTime = 0f;
    [SerializeField]
    GameObject _Bullet;
    [SerializeField]
    Vector3 _StartPos;

    private void Awake()
    {
        _Bullet.SetActive(false);
        //this.transform.position += _StartPos;
    }

    private void Start()
    {

        StartCoroutine("Shoot");
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(_shootWaitTime);
        this.transform.position = Camera.main.GetComponent<AnimalDragonPackCharacterButton>().ShootPoint.transform.position + _StartPos;
        _Bullet.SetActive(true);
        GetComponent<Rigidbody>().AddForce(transform.forward * _speed, ForceMode.Impulse);
        Destroy(gameObject, 3);
    }
}
