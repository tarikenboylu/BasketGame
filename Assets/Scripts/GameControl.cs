using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameControl : MonoBehaviour
{
    public GameObject activeBall;
    public Transform ballPosition;

    public GameObject[] Balls;

    [Header("UIElements")]
    public TextMeshProUGUI ballsCountText;
    public TextMeshProUGUI basketCountText;

    public GameObject swipeImageUI;
    public GameObject rightButton;
    public GameObject leftButton;

    [Header("Ball Info Texts")]
    public TextMeshProUGUI ballInfoText;
    //public TextMeshProUGUI ballSizeText;
    //public TextMeshProUGUI ballWeightText;
    //public TextMeshProUGUI bouncinessText;
    
    public GameObject finishLevelUI;

    [Header("PlayerStats")]
    public int ballsCount = 10;
    public int basketCount = 0;
    public int ballType = 0;
    public bool randomBall = false;

    float timeBtwBalls = 0f;
    public float timeAftLastBall = 5f;
    
    bool noBallsLeft = false;
    public bool rotL = false;
    bool rotR = false;

    [Header("TouchPositions")]//Göndermeden önce sil
    [SerializeField]
    Vector3 firstTouchPosition = Vector3.zero;
    
    [SerializeField]
    Vector3 lastTouchPosition = Vector3.zero;

    void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        ballsCountText.text = "Balls Left : " + ballsCount;
        basketCountText.text = "Basket : " + basketCount;

        if (activeBall != null)
        {
            ballInfoText.text = activeBall.name + "\nSize : " + activeBall.GetComponent<BallFunctions>().ball.size
            + "\nWeight : " + activeBall.GetComponent<BallFunctions>().ball.weight
            + "\nBounciness : " + activeBall.GetComponent<SphereCollider>().material.bounciness;
        }

        if (randomBall)
            ballType = Random.Range(0, Balls.Length);

        if(rotL)
            ballPosition.Rotate(0, 0.1f, 0);
        
        if(rotR)
            ballPosition.Rotate(0, -0.1f, 0);

        if (noBallsLeft)
            timeAftLastBall -= Time.deltaTime;

        if (timeAftLastBall < 0 && ballsCount <= 0)
            FinishLevel();

        if (timeBtwBalls > 0)
            timeBtwBalls -= Time.deltaTime;

        if (timeBtwBalls < 0 && ballsCount > 0)
        {
            activeBall = Instantiate(Balls[ballType], ballPosition.position, Quaternion.identity);
            timeBtwBalls = 0;
        }
    }

    public void StrtButton()
    {
        Time.timeScale = 1;
    }

    public void FinishLevel()
    {
        finishLevelUI.SetActive(true);
        ballsCountText.gameObject.SetActive(false);
        basketCountText.gameObject.SetActive(false);
        swipeImageUI.SetActive(false);
        rightButton.SetActive(false);
        leftButton.SetActive(false);
        finishLevelUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Attığınız basket sayısı = " + basketCount;
    }

    public void ThrowBall()
    {
        Vector3 playerForce = lastTouchPosition - firstTouchPosition;
        activeBall.GetComponent<Rigidbody>().isKinematic = false;
        activeBall.GetComponent<Rigidbody>().AddForce((Camera.main.transform.forward * 55 + Vector3.up * 75) * playerForce.y / 80);
        activeBall.GetComponent<BallFunctions>().thrown = true;
        activeBall = null;

        if (ballsCount > 0)
        {
            timeBtwBalls = 1f;
            ballsCount--;
        }
        if (ballsCount <= 0)
            noBallsLeft = true;

        //Debug.Log("Throw" + playerForce.x + " " + playerForce.y);

        firstTouchPosition = Vector3.zero;
        lastTouchPosition = Vector3.zero;
    }

    public void GetFirstTouchPosition()
    {
        if (Input.touchCount > 0)
            firstTouchPosition = Input.GetTouch(0).position;
    }
    
    public void GetLastTouchPosition()
    {
        if (Input.touchCount > 0)
            lastTouchPosition = Input.GetTouch(0).position;

        if (firstTouchPosition.y < lastTouchPosition.y && activeBall != null)
            ThrowBall();
    }
    
    public void RotateLeft()
    {
        rotL = !rotL;
    }

    public void RotateRight()
    {
        rotR = !rotR;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
