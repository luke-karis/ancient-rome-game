using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardText;
    public Sprite cardArtEvent;
    public Sprite cardArtAdvisor;

    public string primarySourceEvent;
    public string primarySourceAuthor;
    public string primarySourceContext;

    public string Advice;

    public string RightOption;
    public Sprite cardArtRight;
    public int moneyBarRight;
    public int plebPopularityRight;
    public int patricPopularityRight;
    public string rNextPath;
    //public int rNextIndex;
    public bool changeSeasonR;

    public string LeftOption;
    public Sprite cardArtLeft;
    public int moneyBarLeft;
    public int plebPopularityLeft;
    public int patricPopularityLeft;
    public string lNextPath;
    //public int lNextIndex;
    public bool changeSeasonL;

    public bool endOfYear;
    public bool isInformation;
}
