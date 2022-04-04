using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButtons : MonoBehaviour
{
    public int willCount = 100;
    public int crowdCount = 100;

    public GameObject jokeA;
    public GameObject jokeB;
    public GameObject jokeC;

    public GameObject currentScore;

    private void Update()
    {

        currentScore.GetComponent<Text>().text = "audience satisfaction: " + crowdCount + ", Was satisfaction: " + willCount;

    }


    public void JokeA()
    {

        if (QuestionGenerate.actualJoke == "A")
        {

            willCount += 10;
            crowdCount += 10;


        } else 
        {

            willCount -= 10;
            crowdCount -= 10;

        }
        jokeA.GetComponent<Button>().enabled = false;
        jokeB.GetComponent<Button>().enabled = false;
        jokeC.GetComponent<Button>().enabled = false;
        StartCoroutine(NextQuestion());
    }

    public void JokeB()
    {

        if (QuestionGenerate.actualJoke == "B")
        {

            willCount += 10;
            crowdCount += 10;


        }
        else
        {

            willCount -= 10;
            crowdCount -= 10;

        }
        jokeA.GetComponent<Button>().enabled = false;
        jokeB.GetComponent<Button>().enabled = false;
        jokeC.GetComponent<Button>().enabled = false;
        StartCoroutine(NextQuestion());


    }
    public void JokeC()
    {

        if (QuestionGenerate.actualJoke == "C")
        {

            willCount += 10;
            crowdCount += 10;


        }
        else
        {

            willCount -= 10;
            crowdCount -= 10;

        }
        jokeA.GetComponent<Button>().enabled = false;
        jokeB.GetComponent<Button>().enabled = false;
        jokeC.GetComponent<Button>().enabled = false;
        StartCoroutine(NextQuestion());

    }

    IEnumerator NextQuestion()
    {

        yield return new WaitForSeconds(2);



        jokeA.GetComponent<Button>().enabled = true;
        jokeB.GetComponent<Button>().enabled = true;
        jokeC.GetComponent<Button>().enabled = true;
        QuestionGenerate.displayingQuestion = false;

    }

}
