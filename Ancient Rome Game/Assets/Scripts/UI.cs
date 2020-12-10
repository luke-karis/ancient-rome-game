using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Slider popularitySlider;

    public GameObject gameButton;
    public GameObject electionButton;
    public GameObject gameScene;
    public GameObject electionScene;
    public GameObject card;

    public Deck deck;

    public GameObject popSlider;
    public GameObject plebSlider;
    public GameObject patricSlider;

    public GameObject background;

    //The buttons that you click on to start elections
    //Might need to make these GameObjects into buttons
    public GameObject qButton;
    public GameObject pButton;
    public GameObject cButton;

    public GameObject qLock;
    public GameObject pLock;
    public GameObject cLock;

    //Buttons that will mark how much popularity the player needs to win the election
    public GameObject qElectionLevel;
    public GameObject pElectionLevel;
    public GameObject cElectionLevel;

    public GameObject popup;
    public GameObject consulPopup;
    public GameObject praetorPopup;
    public GameObject quaestorPopup;

    public GameObject cPopButton;
    public GameObject pPopButton;
    public GameObject qPopButton;

    public bool election = false;
    public bool electionQ = false;
    public bool electionP = false;
    public bool electionC = false;

    //public bool qElectionWon = false;
    //public bool pElectionWon = false;
    //public bool cElectionWon = false;

    public bool gameRunning = true;

    public void primarySource()
    {
        deck.primarySource = true;
        deck.primarySAuthor = false;
        deck.primarySContext = false;
        deck.runOnceUI = true;
    }

    public void primarySourceAuthor()
    {
        deck.primarySource = false;
        deck.primarySAuthor = true;
        deck.primarySContext = false;
        deck.runOnceUI = true;
    }

    public void primarySourceContext()
    {
        deck.primarySource = false;
        deck.primarySAuthor = false;
        deck.primarySContext = true;
        deck.runOnceUI = true;
    }

    public void loadGame()
    {
        SceneManager.LoadScene(1);
        //background.SetActive(true);
    }

    public void loadConsulPopup()
    {
        popup.SetActive(true);
        consulPopup.SetActive(true);
        cPopButton.SetActive(false);
        pPopButton.SetActive(false);
        qPopButton.SetActive(false);
    }

    public void loadPraetorPopup()
    {
        popup.SetActive(true);
        praetorPopup.SetActive(true);
        cPopButton.SetActive(false);
        pPopButton.SetActive(false);
        qPopButton.SetActive(false);
    }

    public void loadQuaestorPopup()
    {
        popup.SetActive(true);
        quaestorPopup.SetActive(true);
        cPopButton.SetActive(false);
        pPopButton.SetActive(false);
        qPopButton.SetActive(false);
    }

    public void closePopup()
    {
        popup.SetActive(false);
        consulPopup.SetActive(false);
        praetorPopup.SetActive(false);
        quaestorPopup.SetActive(false);

        cPopButton.SetActive(true);
        pPopButton.SetActive(true);
        qPopButton.SetActive(true);
    }

    public void loadMenu()
    {
        SceneManager.LoadScene(0);
        //background.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void loadElection()
    {
        //electionButton.SetActive(false);
        gameScene.SetActive(false);
        card.SetActive(false);
        //background.SetActive(false);

        gameButton.SetActive(true);
        electionScene.SetActive(true);

        gameRunning = false;
    }

    public void loadSameGame()
    {
        //electionButton.SetActive(true);
        gameScene.SetActive(true);
        card.SetActive(true);

        gameButton.SetActive(false);
        electionScene.SetActive(false);

        gameRunning = true;
    }

    public void startElection(int electionNum)
    {
        //Quaestor election
        if (electionNum == 1)
        {
            Debug.Log("Election Q!");
            popSlider.SetActive(true);
            qElectionLevel.SetActive(true);

            plebSlider.SetActive(false);
            patricSlider.SetActive(false);

            electionQ = true;
            election = true;

            deck.electedOfficeNum = 6;

            //Disable ability to click election button
        }

        //Praetor election
        if (electionNum == 2)
        {
            Debug.Log("Election P!");
            popSlider.SetActive(true);
            pElectionLevel.SetActive(true);

            plebSlider.SetActive(false);
            patricSlider.SetActive(false);

            electionP = true;
            election = true;

            deck.electedOfficeNum = 6; //CHANGE when I make Praetor cards
        }

        //Consul Election
        if (electionNum == 3)
        {
            Debug.Log("Election C!");
            popSlider.SetActive(true);
            cElectionLevel.SetActive(true);

            plebSlider.SetActive(false);
            patricSlider.SetActive(false);

            electionC = true;
            election = true;

            deck.electedOfficeNum = 6; //CHANGE when I make consul cards
        }
    }

    //This will be called when elections are completed and it will undo what is done in start election and enable election
    public void electionOver(GameObject lockSprite, GameObject button, GameObject electionLevel, int electionType)
    {
        popSlider.SetActive(false);
        plebSlider.SetActive(true);
        patricSlider.SetActive(true);
        electionLevel.SetActive(false);

        election = false;
        if (electionType == 1) electionQ = false;
        else if (electionType == 2) electionP = false;
        else if (electionType == 3) electionC = false;

        lockSprite.SetActive(true);
        button.GetComponent<Button>().enabled = false;
    }

    //This will just enable to the Quaestor elections after playing the game for "1 year"
    //It might be a cool idea to later make the player get to a certain popularity level before it unlocks 
    //Might want to make this able to enable all elections as I think I will want to disable them after a serving office
    public void enableElections(GameObject lockSprite, GameObject button)
    {
        lockSprite.SetActive(false);
        button.GetComponent<Button>().enabled = true;
 
    }
    /*
    public void checkElection()
    {
        if (electionQ && popularitySlider.value >= qLevel) qElectionWon = true;
        else if (electionP && popularitySlider.value >= pLevel) pElectionWon = true;
        else if (electionC && popularitySlider.value >= cLevel) cElectionWon = true;
    }
    */
    public bool checkElectionWon(float successLevel)
    {
        if (popularitySlider.value >= successLevel) return true;
        else return false;
    }

    private void Update()
    {
        if(gameRunning) card = GameObject.Find("Canvas/Card Template(Clone)");
    }
}
