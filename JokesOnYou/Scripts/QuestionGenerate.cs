using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionGenerate : MonoBehaviour
{
    public static bool displayingQuestion = false;
    public static string actualJoke;
    public int questionNumber;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
        if (displayingQuestion == false)
        {
            displayingQuestion = true;
            questionNumber = Random.Range(1,5);
            if (questionNumber == 1) 
            { 

                JokesDisplay.newQuestion = "Choose one of the jokes";
                JokesDisplay.newJokeA = "A. I heard women love a man in uniform. Can’t wait to start working at McDonald’s";
                JokesDisplay.newJokeB = "B. Joke B";
                JokesDisplay.newJokeC = "C. Joke C";

                actualJoke = "A";

            }
            if (questionNumber == 2)
            {


                JokesDisplay.newQuestion = "Choose one of the jokes";
                JokesDisplay.newJokeA = "A. I heard women love a man in uniform. Can’t wait to start working at McDonald’s";
                JokesDisplay.newJokeB = "B. Toad";
                JokesDisplay.newJokeC = "C. Joke C";

                actualJoke = "B";

            }
            if (questionNumber == 3)
            {


                JokesDisplay.newQuestion = "Choose one of the jokes";
                JokesDisplay.newJokeA = "A. I heard women love a man in uniform. Can’t wait to start working at McDonald’s";
                JokesDisplay.newJokeB = "B. djwaijdlisajjdliwa";
                JokesDisplay.newJokeC = "C. Joke C";

                actualJoke = "C";

            }
            if (questionNumber == 4)
            {


                JokesDisplay.newQuestion = "Choose one of the jokes";
                JokesDisplay.newJokeA = "A. I heard women love a man in uniform. Can’t wait to start working at McDonald’s";
                JokesDisplay.newJokeB = "B. Link";
                JokesDisplay.newJokeC = "C. Joke C";

                actualJoke = "A";

            }

            // all Jokes go above this line
            JokesDisplay.updateJoke = false;

        }

    }
}
