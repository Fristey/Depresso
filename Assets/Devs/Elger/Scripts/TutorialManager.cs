using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class Tutorial
    {
        [Header("General")]
        public string identifier;
        public bool hasPlayed;
        public string[] texts;
        public int textIndex = 0;
    }

    [SerializeField] private List<Tutorial> tutorials = new List<Tutorial>();
    [SerializeField] private List<Tutorial> backlogTutorials = new List<Tutorial>();

    private Tutorial curTurtorial;
    [SerializeField] private TMP_Text textDisplay;

    public static TutorialManager instance;
    

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Use "identiefier" to start a specific tutorial. If another tutorial is started it will enter the backlog and be played after
    /// </summary>
    /// <param name="identifier"></param>
    public virtual void StartTutorial(string identifier)
    {
        Tutorial tempTutorial = tutorials.Find(x => x.identifier.Contains(identifier));

        if (tempTutorial != null && !tempTutorial.hasPlayed) 
        {
            if (curTurtorial == null)
            {
                GameManager.Instance.SetGameState(GameStates.tutorial);

                curTurtorial = tempTutorial;
                Debug.Log(curTurtorial.texts[curTurtorial.textIndex]);
                textDisplay.text = curTurtorial.texts[curTurtorial.textIndex];
            }
            else 
            {
                backlogTutorials.Add(tempTutorial);
            }
        }


    }

    /// <summary>
    /// fill nextStep if a specific step is wanted else will just play next step. Identiefier required
    // / </summary>
    /// <param name="finishedStep"></param>
    public virtual void StepFinished( string identifier, int nextStep = default(int))
    {
        if (curTurtorial != null && !curTurtorial.hasPlayed && identifier == curTurtorial.identifier)
        {
            if (curTurtorial.textIndex < curTurtorial.texts.Length - 1)
            {
                if (nextStep != default(int))
                {
                    curTurtorial.textIndex = nextStep;
                    textDisplay.text = curTurtorial.texts[curTurtorial.textIndex];
                }
                else
                {
                    curTurtorial.textIndex++;
                    textDisplay.text = curTurtorial.texts[curTurtorial.textIndex];
                }
            }
            else
            {
                curTurtorial.hasPlayed = true;
                textDisplay.text = string.Empty;
                curTurtorial = null;

                if(backlogTutorials.Count > 0) 
                {
                    backlogTutorials.RemoveAll(t => t.hasPlayed == true);

                    if (backlogTutorials.Count > 0)
                    {
                        StartTutorial(backlogTutorials[0].identifier);
                    }
                } else
                {
                    GameManager.Instance.ReturnGameState();
                }
            }

        }
    }
}

