using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private const string poor = "Get a conscience.";
    private const string average = "It was your first job. Mistakes were expected.";
    private const string good = "You did very good. Continue like this and we'll allow you to reap celebrities.";
    private const string perfect = "Incredible results! We were not expecting you to perform this well. Here's a new scythe.";
    private const string _toalPeolpe = "Total amount of people: <color=white>{0}</color>";
    private const string _guiltyPeople = "Guilty people: <color=white>{0}</color>";
    private const string _innocentPeople = "Innocent people: <color=white>{0}</color>";
    private const string _correctlyJudged = "Correctly judged: <color=green>{0}</color>";
    private const string _correctlyRewarded = "Correctly Rewarded: <color=green>{0}</color>";
    private const string _correctlyCondemned = "Corectly condemned: <color=green>{0}</color>";
    private const string _incorrectlyJudged = "Incorrectly judged: <color=red>{0}</color>";
    private const string _incorrectlyRewarded = "Incorrectly Rewarded: <color=red>{0}</color>";
    private const string _incorrectlyCondemned = "Incorectly condemned: <color=red>{0}</color>";

    [SerializeField] Text results = default;
    [SerializeField] Text message = default;
    [SerializeField] GameObject soulReaperGameGO = default;
    SoulReaperGame srg;

    int totalPeople;
    int guiltyPeople;
    int innocentPeople;
    int correctlyJudged;
    int correctlyRewarded;
    int correctlyCondemned;
    int incorrectlyJudged;
    int incorrectlyRewarded;
    int incorrectlyCondemned;

    Person[] people;
    List<string> resultList;

    public void Run()
    {
        srg = soulReaperGameGO.GetComponent<SoulReaperGame>();
        people = srg.People;
        totalPeople = srg.People.Length;
        guiltyPeople = GetGuiltyPeople();
        innocentPeople = totalPeople - guiltyPeople;
        correctlyJudged = GetCorrectlyJudged();
        correctlyRewarded = GetCorrectlyRewarded();
        correctlyCondemned = GetCorrectlyCondemned();
        incorrectlyJudged = GetIncorrectlyJudged();
        incorrectlyRewarded = GetIncorrectlyRewarded();
        incorrectlyCondemned = GetIncorrectlyCondemned();

        resultList = new List<string>
        {
            string.Format(_toalPeolpe, totalPeople),
            string.Format(_guiltyPeople, guiltyPeople),
            string.Format(_innocentPeople, innocentPeople),
            string.Format(_correctlyJudged, correctlyJudged),
            string.Format(_correctlyRewarded, correctlyRewarded),
            string.Format(_correctlyCondemned, correctlyCondemned),
            string.Format(_incorrectlyJudged, incorrectlyJudged),
            string.Format(_incorrectlyRewarded, incorrectlyRewarded),
            string.Format(_incorrectlyCondemned, incorrectlyCondemned)
        };
    }

    public void DisplayResults()
    {
        string conclusion;

        switch (correctlyJudged)
        {
            case int n when (n < 4):
                conclusion = poor;
                break;
            case int n when (n > 3 && n < 7):
                conclusion = average;
                break;
            case int n when (n > 6 && n < 10):
                conclusion = good;
                break;
            case int n when (n == 10):
                conclusion = perfect;
                break;
            default:
                conclusion = null;
                break;
        }

        message.text = conclusion;
        results.text = string.Join("\n", resultList);
    }

    private int GetCorrectlyJudged()
    {
        int correctlyJudged = 0;
        foreach (Person person in people)
        {
            if ((person.IsGuiltyPlayer && person.IsGuilty) || (!person.IsGuiltyPlayer && !person.IsGuilty))
            {
                correctlyJudged++;
            }
        }
        return correctlyJudged;
    }

    private int GetIncorrectlyCondemned()
    {
        int incorrectlyCondemned = 0;
        foreach (Person person in people)
        {
            if (person.IsGuiltyPlayer && !person.IsGuilty)
            {
                incorrectlyCondemned++;
            }
        }
        return incorrectlyCondemned;
    }

    private int GetIncorrectlyRewarded()
    {
        int incorrectlyRewarded = 0;
        foreach (Person person in people)
        {
            if (!person.IsGuiltyPlayer && person.IsGuilty)
            {
                incorrectlyRewarded++;
            }
        }
        return incorrectlyRewarded;
    }

    private int GetIncorrectlyJudged()
    {
        int incorrectlyJudged = 0;
        foreach (Person person in people)
        {
            if ((person.IsGuiltyPlayer && !person.IsGuilty) || (!person.IsGuiltyPlayer && person.IsGuilty))
            {
                incorrectlyJudged++;
            }
        }
        return incorrectlyJudged;
    }

    private int GetCorrectlyCondemned()
    {
        int correctlyCondemned = 0;
        foreach (Person person in people)
        {
            if (person.IsGuilty && person.IsGuiltyPlayer)
            {
                correctlyCondemned++;
            }
        }
        return correctlyCondemned;
    }

    private int GetCorrectlyRewarded()
    {
        int correctlyRewarded = 0;
        foreach (Person person in people)
        {
            if (!person.IsGuilty && !person.IsGuiltyPlayer)
            {
                correctlyRewarded++;
            }
        }
        return correctlyRewarded;
    }

    public int GetGuiltyPeople()
    {
        int guiltyPeople = 0;

        foreach (Person person in srg.People)
        {
            if (person.IsGuilty)
            {
                guiltyPeople++;
            }
        }
        return guiltyPeople;
    }
}
