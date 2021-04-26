using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFunctions : MonoBehaviour
{
    public Ball ball;
    float countDowntoDestroy = 6;

    bool touchedGround = false;

    public bool thrown = false;

    void Start()
    {
        gameObject.name = ball.ballName;
        transform.localScale = ball.size * Vector3.one;
        GetComponent<Rigidbody>().mass = ball.weight;
        GetComponent<MeshRenderer>().material.color = ball.ballColor;
        GetComponent<SphereCollider>().material = ball.PhscMaterial;
    }

    void Update()
    {
        if (thrown)
            countDowntoDestroy -= Time.deltaTime;

        if (countDowntoDestroy < 0)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            touchedGround = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Basket"))
            if (GetComponent<Rigidbody>().velocity.y < 0 /*&& !touchedGround*/)//yerden seken topun basket sayılmasını önler
                FindObjectOfType<GameControl>().basketCount++;
    }
}
