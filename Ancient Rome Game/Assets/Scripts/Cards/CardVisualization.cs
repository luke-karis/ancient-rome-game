using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CardVisualization : MonoBehaviour
{
    public Card card;

    public Image artEvent;
    public Image artAdvisor;
    public Image artRight;
    public Image artLeft;
    public TextMeshProUGUI eText;

    public TextMeshProUGUI up;
    public TextMeshProUGUI right;
    public TextMeshProUGUI left;

    void Start()
    {
        LoadCard(card);
    }

    public void LoadCard(Card c)
    {
        //card = c;

        artEvent.sprite = c.cardArtEvent;
        artAdvisor.sprite = c.cardArtAdvisor;
        artRight.sprite = c.cardArtRight;
        artLeft.sprite = c.cardArtLeft;

        eText.text = c.cardText;

        up.text = c.Advice;
        right.text = c.RightOption;
        left.text = c.LeftOption;

    }
    //Directions that affect money
    public int returnMoneyBarRight()
    {
        return card.moneyBarRight;
    }
    public int returnMoneyBarLeft()
    {
        return card.moneyBarLeft;
    }

    //Directinos that affect poulares popularity (keeping pleb as part of variable name)
    public int returnPlebPopularityRight()
    {
        return card.plebPopularityRight;
    }
    public int returnPlebPopularityLeft()
    {
        return card.plebPopularityLeft;
    }

    //Directinos that affect optimates popularity
    public int returnPatricPopularityRight()
    {
        return card.patricPopularityRight;
    }
    public int returnPatricPopularityLeft()
    {
        return card.patricPopularityLeft;
    }

    //Primary sources associated with each direction
    public string returnPrimarySourceEvent()
    {
        return card.primarySourceEvent;
    }
    public string returnPrimarySourceAuthor()
    {
        return card.primarySourceAuthor;
    }
    public string returnPrimarySourceContext()
    {
        return card.primarySourceContext;
    }

    public string returnPathRight()
    {
        return card.rNextPath;
    }
    public string returnPathLeft()
    {
        return card.lNextPath;
    }

    /*
    //Returns the index which refers to which card event will occur next
    public int returnIndexRight()
    {
        return card.rNextIndex;
    }
    public int returnIndexLeft()
    {
        return card.lNextIndex;
    }

    //Sets the index of what card should occur next
    public void setIndexRight(int newIndex)
    {
        card.rNextIndex = newIndex;
    }
    public void setIndexLeft(int newIndex)
    {
        card.lNextIndex = newIndex;
    }
    */
    //Returns whether this direction will change the season
    public bool returnChangeSeasonR()
    {
        return card.changeSeasonR;
    }
    public bool returnChangeSeasonL()
    {
        return card.changeSeasonL;
    }

    //Sets a bool to determine if swiping in a direction will change the season
    public void setChangeSeasonR(bool seasonChange)
    {
        card.changeSeasonR = seasonChange;
    }
    public void setChangeSeasonL(bool seasonChange)
    {
        card.changeSeasonL = seasonChange;
    }

    //Returns if the event is at the end of the year 
    public bool returnEndOfYear()
    {
        return card.endOfYear;
    }

    public bool returnInfo()
    {
        return card.isInformation;
    }
}
