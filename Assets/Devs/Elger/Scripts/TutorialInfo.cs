using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class TutorialInfo : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private int steps;
    [SerializeField] protected int stepIndex;

    [SerializeField] private bool hasPlayed = false;

    [SerializeField] private List<string> texts = new List<string>();
    private TMP_Text textDisplay;
    protected virtual void StartTutorial()
    {
        if (!hasPlayed)
        {
            GameManager.Instance.SetGameState(GameStates.tutorial);
            textDisplay = GameObject.FindWithTag("TutorialText").GetComponent<TMP_Text>();

            textDisplay.text = texts[stepIndex];
        }
    }

    /// <summary>
    /// finished step parameter must be filled if you want to go to a specific step. If left empty will go to next step
    /// </summary>
    /// <param name="finishedStep"></param>
    protected virtual void StepFinished(int nextStep = default(int))
    {
        if (!hasPlayed)
        {
            if (stepIndex < texts.Count - 1)
            {
                if (nextStep != default(int))
                {
                    stepIndex = nextStep;
                    textDisplay.text = texts[stepIndex];
                }
                else
                {
                    stepIndex++;
                    textDisplay.text = texts[stepIndex];
                }
            }
            else
            {
                hasPlayed = true;
                textDisplay.text = string.Empty;
                GameManager.Instance.ReturnGameState();
            }

        }
    }
}
