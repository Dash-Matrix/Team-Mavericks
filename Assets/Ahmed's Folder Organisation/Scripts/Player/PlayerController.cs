using DG.Tweening;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] private GameObject TouchControls;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform target;

    [Space(30), Header("Gun Stuff")]
    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    private Transform bulletFrom;
    private Transform bulletTo;
    [SerializeField]
    private float bulletDelay;
    [SerializeField]
    private float bulletSpeed;
    RaycastHit hit;
    GameObject NewBullet;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        transform.LookAt(new Vector3(target.position.x, 0,0));
    }
    public void StartShooting(bool _shoot)
    {
        if (_shoot)
        {
            StartCoroutine(Shoot());
        }
        else
        {
            StopAllCoroutines();
        }

    }
    public IEnumerator Shoot()
    {
        for(; ; )
        {

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if (Physics.Raycast(ray, out hit))
            {
                bulletTo = hit.transform;

                float _speed;
                _speed = Vector3.Distance(bulletFrom.position, hit.point) / bulletSpeed;

                NewBullet = Instantiate(Bullet, bulletFrom.position, Quaternion.identity);

                NewBullet.GetComponent<Bullet>().Shooted(hit.point, _speed);
            }
            yield return new WaitForSeconds(bulletDelay);
        }
    }
    public void PlayerDieWin(bool won)
    {
        if (won)
        {
            anim.SetTrigger("Won");
            transform.DOMoveY(0.20f, 0.1f).SetEase(Ease.Linear);
            StartCoroutine(NiceWin());
        }
        else
        {
            anim.SetTrigger("Died");
            transform.DOMoveY(0.06f, 0.1f).SetEase(Ease.Linear);
            StartCoroutine(NiceDie());
        }
    }
    IEnumerator NiceDie()
    {
        StopCoroutine(Shoot());
        target.gameObject.SetActive(false);
        GetComponent<PlayerController>().enabled = false;
        GetComponent<LeanFingerDown>().enabled = false;
        GetComponent<LeanFingerUp>().enabled = false;
        TouchControls.SetActive(false);
        AudioManager.instance._Lose();

        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(0);
    }
    IEnumerator NiceWin()
    {
        AudioManager.instance._Win();

        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(0);
    }
}
