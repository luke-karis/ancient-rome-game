using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class DecisionReview : MonoBehaviour
{
    public GameObject newCard;
    private CardVisualization cardViz;
    private GameObject cardGameObj;

    //public CardMovement cardMovement;
    public Deck deck;

    //Think I can delete this
    //private Scene scene;

    public string[] cardTracker;
    //public int[] choiceTracker;
    public string[] seasonTracker;
    public string[] consulNameTracker;
    public int[] yearTracker;

    public int counter = 0;
    public int counterD = 0;
    public int drCounter = 0;

    public bool swipe = false;

    private void Start()
    {
        //cardMovement = new CardMovement();
        //scene = SceneManager.GetActiveScene();

        cardTracker = new string[deck.numEvents + 5];   //This + 5 is a bandaid fix for the addition of quaestor cards
        //choiceTracker = new int[deck.numEvents];
        seasonTracker = new string[deck.numEvents + 5];
        consulNameTracker = new string[deck.numEvents + 5];
        yearTracker = new int[deck.numEvents + 5];
    }
    /*
    public void trackChoice(int direction)
    {
        choiceTracker[counter] = direction;
        counter++;
    }
    */
    public void trackCard(string card)
    {
        cardTracker[counter] = card;
        //seasonTracker[counter] = season;
        //consulNameTracker[counter] = consulName;
        //yearTracker[counter] = year;
        counter++;
    }

    public void trackDate(string season, string consulName, int year)
    {
        seasonTracker[counterD] = season;
        consulNameTracker[counterD] = consulName;
        yearTracker[counterD] = year;
        counterD++;
    }

    public void swipeRight()
    {
        if(drCounter < deck.decisionEventCount - 1)
        { 
            drCounter++;
            swipe = true;
        }
    }
    public void swipeLeft()
    {
        if (drCounter != 0)
        {
            drCounter--;
            swipe = true;
        }
    }

    //Method for testing, can be deleted later
    public void printArrays()
    {
        Debug.Log("Card array: " + cardTracker[0]);
        //Debug.Log("Choice array: " + choiceTracker[0]);
    }
}
