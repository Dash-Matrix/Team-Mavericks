using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        /*if(other.gameObject.tag == "Ground" || other.gameObject.tag == "BG")
        {
            FinishUp();
        }*/
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Movement>().Hit();
        }
    }
}
