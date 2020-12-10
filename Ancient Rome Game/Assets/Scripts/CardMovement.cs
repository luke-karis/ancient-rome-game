using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class CardMovement : MonoBehaviour
{
    public float cardSpeed = 10.0f;
    //public Animator animator;

    //Not sure if these are used, seems like just variables from deck are used
    public float plebValue = 0;
    public float patricValue = 0;
    public float popValue = 0;

    //public int nextIndex;
    
    //Used to disable the card's ability to be swiped
    public GameObject disabledObject;
    public bool isDisabled = false;

    public GameObject eventText;
    public GameObject upText;
    public GameObject rightText;
    public GameObject downText;
    public GameObject leftText;

    public GameObject artEvent;
    public GameObject artAdvisor;
    public GameObject artRight;
    public GameObject artDown;
    public GameObject artLeft;

    private int upCount;
    private int rightCount;
    private int downCount;
    private int leftCount;

    //Direction the card is swiped
    public int direction;

    public GameObject newCard;
    private CardVisualization cardViz;
    private UI ui;

    public Deck deck;
    public DecisionReview decisionR;

    public Slider moneySlider;
    public Slider popularitySlider;
    public Slider plebSlider;
    public Slider patricSlider;

    
    public AudioSource audioSource;
    public AudioClip popCheer;
    public AudioClip popBoo;
    public AudioClip optClap;
    public AudioClip optJeer;
    public AudioClip moneyUp;
    public AudioClip moneyDown;
    

    private bool runOnce = false;
    private bool runOnceOnLoad = false;
    private bool runOnceSeason = false;

    /*
    public void loadElements()
    {
        decisionR = GameObject.Find("DecisionReview").GetComponent<DecisionReview>();
        deck = GameObject.Find("Deck").GetComponent<Deck>();
        ui = GameObject.Find("UIController").GetComponent<UI>();
        if (deck.gameRunning) moneySlider = GameObject.Find("Canvas/GameScene/Money Slider").GetComponent<Slider>();
    }
    */
    //This code is used by the different slide functions to activate and deactivate text 
    public void slideSetActive(GameObject active, GameObject deactive1, GameObject deactive2, GameObject deactive3, GameObject deactive4, GameObject artActive, GameObject artDeactive1, GameObject artDeactive2, GameObject artDeactive3, GameObject artDeactive4)
    {
        active.SetActive(true);

        deactive1.SetActive(false);
        deactive2.SetActive(false);
        deactive3.SetActive(false);
        deactive4.SetActive(false);

        artActive.SetActive(true);

        artDeactive1.SetActive(false);
        artDeactive2.SetActive(false);
        artDeactive3.SetActive(false);
        artDeactive4.SetActive(false);
    }
    public void resetCounts(int increment, int reset1, int reset2, int reset3)
    {
        increment++;
        reset1 = 0;
        reset2 = 0;
        reset3 = 0;
    }
    public void doubleTap()
    {
        slideSetActive(eventText, upText, rightText, downText, leftText, artEvent, artAdvisor, artRight, artDown, artLeft);
        upCount = 0;
        rightCount = 0;
        leftCount = 0;
        downCount = 0;
    }
    public void slideUp()
    {
        cardViz = newCard.GetComponent<CardVisualization>();

        if (cardViz.returnInfo() == false)
        {
            slideSetActive(upText, eventText, rightText, downText, leftText, artAdvisor, artEvent, artRight, artDown, artLeft);

            rightCount = 0;
            leftCount = 0;
            downCount = 0;
        }
    }
    public void slideRight()
    {
        slideSetActive(rightText, eventText, upText, downText, leftText, artRight, artEvent, artAdvisor, artDown, artLeft);

        rightCount++;
        leftCount = 0;
        downCount = 0;
        upCount = 0;

        cardViz = newCard.GetComponent<CardVisualization>();
        //Debug.Log(cardViz.returnInfo());

        if (rightCount > 1 || cardViz.returnInfo() == true)
        {
            disabledObject.SetActive(false);
            direction = 2;
        }
    }
    
    public void slideDown()
    {
        cardViz = newCard.GetComponent<CardVisualization>();

        if (!cardViz.returnInfo())
        {
            slideSetActive(eventText, upText, rightText, downText, leftText, artEvent, artAdvisor, artRight, artDown, artLeft);
            upCount = 0;
            rightCount = 0;
            leftCount = 0;
            downCount = 0;
        }
    }
    
    public void slideLeft()
    {
        slideSetActive(leftText, eventText, rightText, downText, upText, artLeft, artEvent, artAdvisor, artRight, artDown);

        leftCount++;
        upCount = 0;
        rightCount = 0;
        downCount = 0;

        cardViz = newCard.GetComponent<CardVisualization>();

        //Debug.Log(cardViz.returnInfo());

        if (leftCount > 1 || cardViz.returnInfo() == true)
        {
            disabledObject.SetActive(false);
            direction = 4;
        }
    }

    public void setSliderVales(int choice)
    {
        cardViz = newCard.GetComponent<CardVisualization>();

        //cound do volume * cardViz.returnPlebPopularity

        if (choice == 2)
        {
            moneySlider.value += cardViz.returnMoneyBarRight();
            //deck.nextIndex = cardViz.returnIndexRight();
            deck.nextPath = cardViz.returnPathRight();
            /*
            if (cardViz.returnPlebPopularityRight() > 0) deck.audioSource.PlayOneShot(deck.popCheer, 0.7F);
            else if (cardViz.returnPlebPopularityRight() < 0) deck.audioSource.PlayOneShot(deck.popBoo, 0.7F);
            else if (cardViz.returnPatricPopularityRight() > 0) deck.audioSource.PlayOneShot(deck.optClap, 0.7F);
            else if (cardViz.returnPatricPopularityRight() < 0) deck.audioSource.PlayOneShot(deck.optJeer, 0.7F);
            else if (cardViz.returnMoneyBarRight() > 0) deck.audioSource.PlayOneShot(deck.moneyUp, 0.7F);
            else if (cardViz.returnMoneyBarRight() < 0) deck.audioSource.PlayOneShot(deck.moneyDown, 0.7F);
            */

            if (cardViz.returnPlebPopularityRight() > 0) audioSource.PlayOneShot(popCheer, 0.5F);
            else if (cardViz.returnPlebPopularityRight() < 0) audioSource.PlayOneShot(popBoo, 0.5F);
            else if (cardViz.returnPatricPopularityRight() > 0) audioSource.PlayOneShot(optClap, 0.5F);
            else if (cardViz.returnPatricPopularityRight() < 0) audioSource.PlayOneShot(optJeer, 0.5F);
            else if (cardViz.returnMoneyBarRight() > 0) audioSource.PlayOneShot(moneyUp, 0.7F);
            else if (cardViz.returnMoneyBarRight() < 0) audioSource.PlayOneShot(moneyDown, 0.7F);

            if (ui.election)
            {
                popularitySlider.value += cardViz.returnPatricPopularityRight() + cardViz.returnPlebPopularityRight();
                deck.popValue = popularitySlider.value;
                deck.plebValue += cardViz.returnPlebPopularityRight();
                deck.patricValue += cardViz.returnPatricPopularityRight();
            }
            else
            {
                plebSlider.value += cardViz.returnPlebPopularityRight();
                patricSlider.value += cardViz.returnPatricPopularityRight();
                deck.plebValue = plebSlider.value;
                deck.patricValue = patricSlider.value;
            }
        }
       
        else if (choice == 4)
        {
            moneySlider.value += cardViz.returnMoneyBarLeft();
            //deck.nextIndex = cardViz.returnIndexLeft();
            deck.nextPath = cardViz.returnPathLeft();

            //deck.audioSource.PlayOneShot(deck.optJeer);
            /*
            if (cardViz.returnPlebPopularityLeft() > 0) audioSource.PlayOneShot(deck.popCheer, 0.7F);
            else if (cardViz.returnPlebPopularityLeft() < 0) deck.audioSource.PlayOneShot(deck.popBoo, 0.7F);
            else if (cardViz.returnPlebPopularityLeft() > 0) deck.audioSource.PlayOneShot(deck.optClap, 0.7F);
            else if (cardViz.returnPlebPopularityLeft() < 0) deck.audioSource.PlayOneShot(deck.optJeer, 0.7F);
            else if (cardViz.returnPlebPopularityLeft() > 0) deck.audioSource.PlayOneShot(deck.moneyUp, 0.7F);
            else if (cardViz.returnPlebPopularityLeft() < 0) deck.audioSource.PlayOneShot(deck.moneyDown, 0.7F);
            */
            if (cardViz.returnPlebPopularityLeft() > 0) audioSource.PlayOneShot(popCheer, 0.5F);
            else if (cardViz.returnPlebPopularityLeft() < 0) audioSource.PlayOneShot(popBoo, 0.5F);
            else if (cardViz.returnPatricPopularityLeft() > 0) audioSource.PlayOneShot(optClap, 0.5F);
            else if (cardViz.returnPatricPopularityLeft() < 0) audioSource.PlayOneShot(optJeer, 0.5F);
            else if (cardViz.returnMoneyBarLeft() > 0) audioSource.PlayOneShot(moneyUp, 0.7F);
            else if (cardViz.returnMoneyBarLeft() < 0) audioSource.PlayOneShot(moneyDown, 0.7F);

            if (ui.election)
            {
                popularitySlider.value += cardViz.returnPatricPopularityLeft() + cardViz.returnPlebPopularityLeft();
                deck.popValue = popularitySlider.value;
                deck.plebValue += cardViz.returnPlebPopularityLeft();
                deck.patricValue += cardViz.returnPatricPopularityLeft();
            }
            else
            {
                plebSlider.value += cardViz.returnPlebPopularityLeft();
                patricSlider.value += cardViz.returnPatricPopularityLeft();
                deck.plebValue = plebSlider.value;
                deck.patricValue = patricSlider.value;
            }
        }
    }

    public void runOneTime(int choice)
    {
        if (!runOnce)
        {
            setSliderVales(choice);
            runOnce = true;
            //decisionR.trackChoice(choice);
            if (choice == 2)
            {
                deck.changeSeasonR = cardViz.returnChangeSeasonR();
                deck.direction = choice;
            }
            /*
            else if (choice == 3)
            {
                deck.changeSeasonD = cardViz.returnChangeSeasonD();
                deck.direction = choice;
            }
            */
            else if (choice == 4)
            {
                deck.changeSeasonL = cardViz.returnChangeSeasonL();
                deck.direction = choice;
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        //loadElements();
        decisionR = GameObject.Find("DecisionReview").GetComponent<DecisionReview>();
        deck = GameObject.Find("Deck").GetComponent<Deck>();
        ui = GameObject.Find("UIController").GetComponent<UI>();

        
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        optClap = (AudioClip)Resources.Load("Sounds/audience_clapping_1", typeof(AudioClip));
        optJeer = (AudioClip)Resources.Load("Sounds/dissaproval", typeof(AudioClip));
        popCheer = (AudioClip)Resources.Load("Sounds/cheer2", typeof(AudioClip));
        popBoo = (AudioClip)Resources.Load("Sounds/boo", typeof(AudioClip));
        moneyUp = (AudioClip)Resources.Load("Sounds/cash_register", typeof(AudioClip));
        moneyDown = (AudioClip)Resources.Load("Sounds/coins_on_glass", typeof(AudioClip));
        
        if (deck.gameRunning) moneySlider = GameObject.Find("Canvas/GameScene/Money Slider").GetComponent<Slider>();
    }
    
    private void Update()
    {
        if (deck.gameRunning)
        {
            if (ui.election && !runOnceOnLoad)
            {
                popularitySlider = GameObject.Find("Canvas/GameScene/PopularitySlider").GetComponent<Slider>();
                popularitySlider.value = deck.plebValue + deck.patricValue;
                runOnceOnLoad = true;
                //Debug.Log("Pop: " + popularitySlider.value + " Pleb: " + plebValue + " Patric: " + patricValue + " UDATE IF");
            }
            else if(!ui.election && !runOnceOnLoad)
            {
                plebSlider = GameObject.Find("Canvas/GameScene/PlebianSlider").GetComponent<Slider>();
                patricSlider = GameObject.Find("Canvas/GameScene/PatricianSlider").GetComponent<Slider>();
                //Debug.Log("Pop: " + popValue + " Pleb: " + plebValue + " Patric: " + patricValue + " UDATE ");
            }
        }

        if (Input.GetKeyDown("up") && !deck.gameOver) slideUp();
        else if (Input.GetKeyDown("right") && !deck.gameOver) slideRight();
        else if (Input.GetKeyDown("down") && !deck.gameOver) slideDown();
        else if (Input.GetKeyDown("left") && !deck.gameOver) slideLeft();

        float dist = Vector2.Distance (new Vector2(0,0), transform.position);
        //Might want to adjust numbers later for where the card is destroyed
        if (direction == 1)
        {
            transform.Translate(new Vector2(0, cardSpeed * Time.deltaTime * 60));
            runOneTime(1);
            //if (transform.position.y > 950) Destroy(gameObject);
            //if (dist > 800f) Destroy(gameObject);
        }
        else if (direction == 2)
        {
            //transform.Translate(new Vector2(cardSpeed * Time.deltaTime * 60, 0));
            runOneTime(2);
            Destroy(gameObject);
            if (cardViz.returnPathRight() == "END") deck.gameOver = true;
            //if (cardViz.returnIndexRight() == 999) deck.gameOver = true;

            //if (transform.position.x > 1000) Destroy(gameObject);
            //if (dist > 600f) Destroy(gameObject);
        }
        /*
        else if (direction == 3)
        {
            transform.Translate(new Vector2(0, -cardSpeed * Time.deltaTime * 60));
            runOneTime(3);
            Destroy(gameObject);
            if (cardViz.returnIndexRight() == 999) deck.gameOver = true;
            //if (transform.position.y < -250) Destroy(gameObject);
            //if (dist > 600f) Destroy(gameObject);
        }
        */
        else if(direction == 4)
        {
            //transform.Translate(new Vector2(-cardSpeed * Time.deltaTime * 60, 0));
            runOneTime(4);
            Destroy(gameObject);
            if (cardViz.returnPathLeft() == "END") deck.gameOver = true;
            //if (cardViz.returnIndexRight() == 999) deck.gameOver = true;

            //if (transform.position.x < -220) Destroy(gameObject);
            //if (dist > 600f) Destroy(gameObject);
        }
    }
}
