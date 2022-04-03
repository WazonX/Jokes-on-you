using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JokesDisplay : MonoBehaviour
{

    public GameObject screenQuestion;
    public GameObject jokeA;
    public GameObject jokeB;
    public GameObject jokeC;
    public static string newQuestion;
    public static string newJokeA;
    public static string newJokeB;
    public static string newJokeC;
    public static bool updateJoke = false;

    // Start is called before the first frame update
    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {
        
        if (updateJoke == false)
        {

            updateJoke = true;
            StartCoroutine(PushTextOnScreen());

        }

    }

    IEnumerator PushTextOnScreen()
    {

        yield return new WaitForSeconds(0.25f);
        screenQuestion.GetComponent<Text>().text = newQuestion;
        jokeA.GetComponent<Text>().text = newJokeA;
        jokeB.GetComponent<Text>().text = newJokeB;
        jokeC.GetComponent<Text>().text = newJokeC;

    }

}
