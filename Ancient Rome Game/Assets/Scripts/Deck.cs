using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    //Used to set the length of basePoolArray
    public int numEvents;

    //Bool to tell if the game is over or not, when its true trigger the event review 
    public bool gameOver = false;

    //Bool that is used to tell if the event reivew stage is triggered
    //used for switching to ER during the game
    private bool eventReview = false;
    public GameObject backFromERButton;

    //The temple background which needs to be toggled on or off because its not on the canvas
    public GameObject background;

    //Used for getting the next card index in the basePoolArray
    //public int nextIndex;
    //public string nextPath = "Cards/0_IE_Tutorial_Welcome";
    public string nextPath;

    //This array holds the addresses for all the events that aren't specific election events
    private string[] basePoolArray;
    public List<string> basePoolList;

    //These arrays hold the addresses for election events only
    private string[] qPoolArray;
    private List<string> qPoolList;
    private string[] pPoolArray;
    private List<string> pPoolList;
    private string[] cPoolArray;
    private List<string> cPoolList;

    //Holds the four seasons and is indexed into to display the current season
    private string[] calendarArray;

    //Int sued to index into the calendar array
    public int calenderIndex = 0;

    //Holds all the consul names to be displayed each in game year
    private string[] consulArray;

    //Int used to index into the Consul Array
    private int consulIndex = 0;

    //Text objects to display the date and consul anme 
    public Text dateText;
    public Text consulText;

    //Variables the represent how big each card pool will be 
    private int basePoolSize = 40;  //This needs to be increased as I add more cards!!!
    private int quaestorPoolSize = 8;
    private int praetorPoolSize = 4;
    private int consulPoolSize = 4;

    //Variables that reflect the amount of popularity needed to win the election
    public float qLevel = 60f;
    public float pLevel = 75f;
    public float cLevel = 90f;

    //This will keep track of the number of cards displayed (from all decks)
    public int cardCounter = 0;

    //Keeps track of where in the current deck cards are being displayed from
    private int deckCounter = 0;

    /*
    * This will keep track where the player is on the cursus honorum
    * It will need to be updated later (if more offices are created) but for now:
    * 0 = never held office
    * 1 = successfully elected quaestor
    * 2 = successfully elected quaestor, and praetor
    * 3 = successfully elected quaestor, praetor, and consul
    */
    public int experience = 0;

    //Keeps track of the age of the player since many offices have age restrictions
    //Not sure what age I am going to start this at
    public int age = 29;

    //Keeps track of how many in game years the player has been playing
    public int yearsPlaying = 0;

    public float plebValue = 0;
    public float patricValue = 0;
    public float popValue = 0;

    //Keeps track of how long the player has been on election break 
    //an election break is how long they have to wait before they can run for the next office 
    private int electionBreakCounter = 0;

    //Int representing the in game year in BCE
    private int year = 66;
    
    //Gameobjects used to instantiate a card
    public GameObject newCard;
    private GameObject cardGameObj;

    //Instances of class used for card functions
    private CardVisualization cardViz;

    //Instance of class used to track events
    public DecisionReview decisionR;

    //Instance of class used to control card behavior
    public CardMovement cardMovement;

   //Instance of class used to control UI
    public UI uiClass;

    //Holds Gameobject of gameScene so that it can be disabled when the game ends 
    public GameObject gameScene;

    //Button that takes you from the game scene to the election scene
    public GameObject electionButton;

    //Text objects for Event Review
    public TextMeshProUGUI primarSourceText;
    public TextMeshProUGUI decisionReviewDateText;
    public TextMeshProUGUI decisionReviewConsulName;

    //Decision Review Game Object
    public GameObject decisionReviewGO;

    //Sets the event Review swiping to be true
    public GameObject decisionReviewSwipe;

    //Object for skip tutorial button
    public GameObject skipTutButton;

    //Text boxes for rank and age
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI ageText;

    //Variable to make sure something runs once in Update
    public bool runOnce = false;

    //Variable to make sure statement runs once in yearManager
    private bool runOnceYear = false;

    //Variable for if the game is running
    public bool gameRunning = true;

    //Checks if the election terms are over
    public bool qTermOver = false;
    public bool pTermOver = false;
    public bool cTermOver = false;

    //Used to tell if the player is serving in elected office
    public bool servingOffice = false;

    //Used to tell if the player is on an election break
    public bool electionBreak = false;

    //Used to tell if the player has been elected to one of these offices
    public bool electedQuaestor = false;
    public bool electedPraetor = false;
    public bool electedConsul = false;

    //Checks if the season should be changed based on the direction that was swiped
    public bool changeSeasonR;
    public bool changeSeasonD;  
    public bool changeSeasonL;

    //Direction of player swipe
    public int direction;

    //used in decision review to display different types of text
    public bool primarySource = true;
    public bool primarySAuthor = false;
    public bool primarySContext = false;
    public bool runOnceUI = true;

    //counts number of decision events for the event review
    public int decisionEventCount = 0;

    //Creates card based on asset path that is passed to it, used for displaying cards in the main game
    public void instantiateCard(string assetPath)
    {
        //Creates new card object based on cardvisualization class
        cardViz = newCard.GetComponent<CardVisualization>();
        cardViz.card = (Card)Resources.Load(assetPath, typeof(Card)); 

        //Instantiates card at sets parent 
        cardGameObj = Instantiate(newCard, new Vector3(0, 0, 0), Quaternion.identity);
        cardGameObj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        //Calls cardMovement class to set correct images and text to be displayed
        cardMovement.slideSetActive(cardMovement.eventText, cardMovement.upText, cardMovement.rightText, cardMovement.downText, cardMovement.leftText, cardMovement.artEvent, cardMovement.artAdvisor, cardMovement.artRight, cardMovement.artDown, cardMovement.artLeft);

        //Tracks events that are not in the tutorial
        if (cardViz.returnInfo() == false && assetPath != "Cards/Tutorial/3_EventIntro" && assetPath != "Cards/Tutorial/4_Advisor")
        {
            decisionR.trackCard(assetPath);
            decisionEventCount++;
        }
    }

    //Makes arrays for different card pools in the game
    //Each card pool comes from a text file of string path names
    public void makeCardPools()
    {
        //This array is used for the majority of the game, it holds all non election events
        basePoolArray = new string[basePoolSize];
        TextAsset basePool = (TextAsset)Resources.Load("TextFiles/BaseCardPaths", typeof(TextAsset));
        basePoolArray = basePool.text.Split(new string[] {"\r\n","\n"}, System.StringSplitOptions.None);
        basePoolList = new List<string>(basePoolArray);

        //These arrays hold election events for being quaestor, praetor, and consul
        qPoolArray = new string[quaestorPoolSize];
        TextAsset qPool = (TextAsset)Resources.Load("TextFiles/QuaestorCardPaths", typeof(TextAsset));
        qPoolArray = qPool.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
        qPoolList = new List<string>(qPoolArray);

        pPoolArray = new string[praetorPoolSize];
        TextAsset pPool = (TextAsset)Resources.Load("TextFiles/PraetorCardPaths", typeof(TextAsset));
        pPoolArray = pPool.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
        pPoolList = new List<string>(pPoolArray);

        cPoolArray = new string[consulPoolSize];
        TextAsset cPool = (TextAsset)Resources.Load("TextFiles/ConsulCardPaths", typeof(TextAsset));
        cPoolArray = cPool.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
        cPoolList = new List<string>(cPoolArray);
    }

    //This method instantiates a card for the decision review part of the game
    //It creates each event starting with the first event encountered
    public void displayCard()
    {
        cardViz = newCard.GetComponent<CardVisualization>();
        cardViz.card = Resources.Load(decisionR.cardTracker[decisionR.drCounter]) as Card;

        cardGameObj = Instantiate(newCard, new Vector3(0, 198, 0), Quaternion.identity); 
        cardGameObj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        //Sets the date, consul, and primary source for the current event
        decisionReviewDateText.text = decisionR.seasonTracker[decisionR.drCounter] + " " + decisionR.yearTracker[decisionR.drCounter] + " BCE";
        decisionReviewConsulName.text = decisionR.consulNameTracker[decisionR.drCounter];
        primarSourceText.text = cardViz.returnPrimarySourceEvent();

        cardMovement = cardGameObj.GetComponent<CardMovement>();
        
        //disables the swiping ability that is in card movement
        cardMovement.disabledObject.SetActive(false);
    }

    //This method displays the current year, season and consul 
    public void yearManager()
    {
        dateText.text = " ";
        consulText.text = " ";

        if(!runOnceYear)
        {
            //This checks if the direction matches with the corresponding change season boolean, if true the season is changed
            if ((direction == 2 && changeSeasonR) || (direction == 3 && changeSeasonD) || (direction == 4 && changeSeasonL)) calenderIndex++;
            //Checks if there is a card and if the card has the endOfYear box checked
            if (cardGameObj != null && cardViz.returnEndOfYear() == true)
            {
                year--;
                consulIndex++;
                calenderIndex = 0;
                yearsPlaying++;
                age++;
            }

            //Tracks the season, consuls, and year for the event review
            if(!cardViz.returnInfo()) decisionR.trackDate(calendarArray[calenderIndex], consulArray[consulIndex], year);
            runOnceYear = true;
        }

        //The game starts in the summer so this starts it at the correct point
        if (cardCounter == 1) calenderIndex = 2;

        dateText.text = calendarArray[calenderIndex] + ", " + year.ToString() + " BCE";
        consulText.text = consulArray[consulIndex];

        ageText.text = "Age: " + age;
    }

    //This method allows the player to skip the tutorial
    public void skipTutorial()
    {
        //Destroy current card
        GameObject currentCard = GameObject.Find("Card Template(Clone)");
        Destroy(currentCard);
        skipTutButton.SetActive(false);
        instantiateCard("Cards/66_BCE/0.0_Summer_GameIntro");
    }

    public void loadEventReview()
    {
        eventReview = true;
        backFromERButton.SetActive(true);
    }

    public void loadGameFromER()
    {
        eventReview = false;
        runOnce = true;
        backFromERButton.SetActive(false);
    }

    //Runs when the game is started
    void Start()
    {
        nextPath = "Cards/Tutorial/0_IE_Welcome";
        makeCardPools();
        calendarArray = new string[] {"Winter", "Spring", "Summer", "Fall"};
        consulArray = new string[11];

        TextAsset consul = (TextAsset)Resources.Load("TextFiles/ConsulNames", typeof(TextAsset));
        consulArray = consul.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

        //sets the number of events in the game to the lengths of the basePoolArray
        numEvents = basePoolList.Count;

        ageText.text = "Age: " + age;
    }

    private bool qBool = true;

    private int qCheck = 0;
    public int electedOfficeNum = 0;

    //Runs every frame
    private void Update()
    {
        //Runs when there is no card (begining of the game or when a card is deleted) and the main game is running
        if (!gameOver && cardGameObj == null)
        {
            Debug.Log(nextPath);

            //After tutorial sets the skip button to not be active
            if (cardCounter > 14) skipTutButton.SetActive(false);

            //Start to quaestor election
            if(nextPath == "Cards/65_BCE/2___Winter_IE_1LR" && !uiClass.electionQ) uiClass.startElection(1); 

            //This if and else if makes the player be on a "election break" for a year after serving office
            //being on an election break just prevents the player from running for office 
            if (electionBreak && electionBreakCounter < 5)
            {
                electionBreakCounter++;
            }
            else if(electionBreak && electionBreakCounter == 5)
            {
                electionBreak = false;
                electionBreakCounter = 0;
            }

            //Checks if the player has served for their year of elected office 
            if (servingOffice && deckCounter > electedOfficeNum) 
            {
                Debug.Log("Done with office");
                deckCounter = 0;
                electionBreak = true;
                servingOffice = false;
                experience++;
                rankText.text = "Senator";
            }

            //This if statement makes it so the player cannot run for office until they have played for a "game" year
            //they also must not be on an election break and have less than 3 experience (means they haven't held all offices)
            //The electiosn are enabled by what the player is allowed to run for 
            if (yearsPlaying >= 1 && experience < 3 && !electionBreak)
            {
                //Debug.Log("Elections are open!");
                if (experience == 0 && !electedQuaestor && age >= 30) uiClass.enableElections(uiClass.qLock, uiClass.qButton);
                else if (experience == 1 && !electedPraetor && age >= 40) uiClass.enableElections(uiClass.pLock, uiClass.pButton);
                //Rn the game ends when you finish term as consul but if I extend it I will want to 
                else if (experience == 2 && age >= 42) uiClass.enableElections(uiClass.cLock, uiClass.cButton);
            }

            //This is be triggered in the summer (when elections happen)
            //depeding on what they are running for this method checks if they have achieved the requisite level of support
            //if they win it calls election helper 
            if (uiClass.election && calenderIndex == 2)
            {
                //Runs if the player wins the election for questor by getting >= the minimum amount of support
                if (uiClass.electionQ)
                {
                    Debug.Log("Won Election!");
                    uiClass.electionOver(uiClass.qLock, uiClass.qButton, uiClass.qElectionLevel, 1);
                    electedQuaestor = true;

                }
                //Runs if the player wins the election for praetor by getting >= the minimum amount of support
                else if (uiClass.checkElectionWon(pLevel) && uiClass.electionP)
                {
                    uiClass.electionOver(uiClass.pLock, uiClass.pButton, uiClass.pElectionLevel, 2);
                    electedPraetor = true;
                }
                //Runs if the player wins the election for consul by getting >= the minimum amount of support
                else if (uiClass.checkElectionWon(cLevel) && uiClass.electionC)
                {
                    uiClass.electionOver(uiClass.cLock, uiClass.cButton, uiClass.cElectionLevel, 3);
                    electedConsul = true;
                }
                //Runs if the player hasn't won their election
                else
                {
                    Debug.Log("You did not win a position");
                    if (uiClass.electionQ) uiClass.electionOver(uiClass.qLock, uiClass.qButton, uiClass.qElectionLevel, 1);
                    else if (uiClass.electionP) uiClass.electionOver(uiClass.pLock, uiClass.pButton, uiClass.pElectionLevel, 2);
                    else if (uiClass.electionC) uiClass.electionOver(uiClass.cLock, uiClass.cButton, uiClass.cElectionLevel, 3);
                }
            }
            //Checks if the player has been elected to be quaestor and starts the sequence of quaestor events
            else if (electedQuaestor && qBool && nextPath == "quaestor") 
            {
                servingOffice = true;
                deckCounter = 0;
                qBool = false;
                rankText.text = "Quaestor";
            }
            //Checks if the user just finished serving as consul and ends the game 
            if (electedConsul && !servingOffice) gameOver = true;
            else
            {
                //If statement here for checks if you have been elected quaestor and are serving office
                if (nextPath == "Cards/65_BCE/8___Summer_IE_4_6_7_Yes" && !uiClass.checkElectionWon(qLevel)) nextPath = "Cards/65_BCE/8___Summer_IE_4_6_7_No";

                //Checks if the player has been elected questor and hasn't started
                if (electedQuaestor && servingOffice && deckCounter == 0) instantiateCard("Cards/64_BCE_Quaestorship/Q0___IE");
                //checks if the player is serving as quesator
                else if (electedQuaestor && servingOffice && deckCounter > 0) instantiateCard(nextPath);
                //Runs for when the player is not serving in elected office
                else
                {
                    instantiateCard(nextPath);
                }

                runOnceYear = false;
                deckCounter++;
                cardCounter++;
            }
        }
        else if (!gameOver && cardGameObj != null) yearManager();

        //Runs once when event review starts then runs again for each card
        //event review is triggered by the game being over
        if ((eventReview && !runOnce) || (gameOver && cardGameObj == null && !runOnce))
        {
            background.SetActive(false);
            decisionReviewSwipe.SetActive(true);
            decisionReviewGO.SetActive(true);

            gameScene.SetActive(false);
            electionButton.SetActive(false);

            gameRunning = false;
            runOnce = true;

            displayCard();

        }
        //Runs to dsiplay the primary source or author or context depending on what the user selects
        else if (gameOver && cardGameObj != null && runOnceUI)
        {
            if (primarySource) primarSourceText.text = cardViz.returnPrimarySourceEvent();
            else if (primarySAuthor) primarSourceText.text = cardViz.returnPrimarySourceAuthor();
            else if (primarySContext) primarSourceText.text = cardViz.returnPrimarySourceContext();
            runOnceUI = false;
        }
        //Destroys the current card on decision review and then sets variables so that the else if statement 
        //above will run again.
        else if (decisionR.swipe == true)
        {
            Destroy(cardGameObj);
            runOnce = false;
            decisionR.swipe = false;
        }    
    }
}
