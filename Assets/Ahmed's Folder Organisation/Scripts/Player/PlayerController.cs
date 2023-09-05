using Cinemachine;
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
    [SerializeField]
    private Transform bulletTo;
    [SerializeField]
    private float bulletDelay;
    [SerializeField]
    private float bulletSpeed;
    RaycastHit hit;
    GameObject NewBullet;
    [SerializeField]
    private float magFillTimer;
    [SerializeField]
    private DOTweenAnimation CameraShake;
    public int Mag;
    [HideInInspector] public int mageStorage;
    private bool canShoot = true;
    private bool running = false;

    [SerializeField] GameObject TouchCOntrols;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem _hitParticle;

    private void Awake()
    {
        instance = this;

        mageStorage = Mag;

    }

    private void Start()
    {

        UIManager.Instance.UpdateMagNumber(Mag);
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
    public void ShootHere(bool _shoot)
    {
        if (_shoot && canShoot && Mag > 0)
        {
            canShoot = false;
            StopAllCoroutines();
            running = false;
            StartCoroutine(_Shoot());
        }
        else if(!_shoot && Mag < mageStorage)
        {
            FillMage();
        }
    }
    private IEnumerator _Shoot()
    {
        yield return new WaitForSeconds(0.2f);

        float _speed;
        _speed = Vector3.Distance(bulletFrom.position, bulletTo.position) / bulletSpeed;

        Handheld.Vibrate();

        Mag--;
        UIManager.Instance.UpdateMagNumber(Mag);
        Instantiate(muzzleFlash, bulletFrom.position, Quaternion.identity);
        NewBullet = Instantiate(Bullet, bulletFrom.position, Quaternion.identity);

        NewBullet.GetComponent<Bullet>()._hitParticle = _hitParticle;
        NewBullet.GetComponent<Bullet>().Shooted(bulletTo.position, _speed);
        CameraShake.RecreateTweenAndPlay();

        yield return new WaitForSeconds(bulletDelay);
        canShoot = true;

    }
    void FillMage()
    {
        StartCoroutine(FilMag());
    }
    IEnumerator FilMag()
    {
        yield return new WaitForSeconds(magFillTimer);
        if (canShoot && !running)
        {
            running = true;
            for (; Mag < mageStorage;)
            {
                yield return new WaitForSeconds(magFillTimer);
                Debug.Log("Run");
                AudioManager.instance._MagFill();
                Mag++;

                UIManager.Instance.UpdateMagNumber(Mag);
            }
            running = false;
        }
    }
    public void PlayerDieWin(bool won)
    {
        StopAllCoroutines();

        TouchCOntrols.SetActive(false);

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
        StopCoroutine(_Shoot());
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
