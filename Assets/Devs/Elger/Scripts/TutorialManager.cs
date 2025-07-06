using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class Tutorial
    {
        [Header("General")]
        public string identifier;
        public bool hasPlayed;

        public float maxTutorialTime;

        public int stepIndex = 0;

        public List<Step> steps = new List<Step>();
    }

    [System.Serializable]
    public class Step
    {
        public MeshRenderer objectRenderer;
        public SkinnedMeshRenderer characterRenderer;

        public string text;

        public Material outline;

        [Header("If maxStepTime is left empty time is infinite")]
        public float maxStepTime;
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

                StartCoroutine(TutorialTimer());

                if (curTurtorial.steps[curTurtorial.stepIndex].maxStepTime > 0)
                {
                    StartCoroutine(StepTimer(curTurtorial.stepIndex, curTurtorial.identifier));
                }

                Debug.Log(curTurtorial.steps[curTurtorial.stepIndex].text);
                textDisplay.text = curTurtorial.steps[curTurtorial.stepIndex].text;

                SetMaterial(true);
            }
            else
            {
                backlogTutorials.Add(tempTutorial);
            }
        }
    }

    private void SetMaterial(bool addOutline)
    {
        List<Material> materials = new List<Material>();
        Material origin = null;

        MeshRenderer curMeshRen = curTurtorial.steps[curTurtorial.stepIndex].objectRenderer;
        SkinnedMeshRenderer curSkinMeshRen = curTurtorial.steps[curTurtorial.stepIndex].characterRenderer;

        Material curOutline = curTurtorial.steps[curTurtorial.stepIndex].outline;

        if (curMeshRen != null)
        {
            origin = curMeshRen.material;
        }
        else if (curSkinMeshRen != null)
        {
            origin = curSkinMeshRen.material;
        }

        if (curTurtorial.steps[curTurtorial.stepIndex].outline != null)
        {
            if (origin != null && origin != curTurtorial.steps[curTurtorial.stepIndex].outline)
            {
                materials.Add(origin);
            }

            if (addOutline)
            {
                materials.Add(curOutline);
            }
        }

        if (curMeshRen != null)
        {
            curMeshRen.SetMaterials(materials);
        }
        else if (curSkinMeshRen != null)
        {
            curSkinMeshRen.SetMaterials(materials);
        }
    }

    /// <summary>
    /// fill nextStep if a specific step is wanted else will just play next step. Identiefier required. Returns true if the next step could be called
    // / </summary>
    /// <param name="finishedStep"></param>
    public virtual bool StepFinished(string identifier, int nextStep = default(int))
    {
        if (curTurtorial != null && !curTurtorial.hasPlayed && identifier == curTurtorial.identifier && nextStep == curTurtorial.stepIndex + 1)
        {
            if (curTurtorial.stepIndex < curTurtorial.steps.Count - 1)
            {
                SetMaterial(false);

                if (nextStep != default(int))
                {
                    curTurtorial.stepIndex = nextStep;
                }
                else
                {
                    curTurtorial.stepIndex++;
                }

                if (curTurtorial.steps[curTurtorial.stepIndex].maxStepTime > 0)
                {
                    StartCoroutine(StepTimer(curTurtorial.stepIndex,curTurtorial.identifier));
                }

                SetMaterial(true);

                textDisplay.text = curTurtorial.steps[curTurtorial.stepIndex].text;
            }
            else
            {
                StopAllCoroutines();

                curTurtorial.hasPlayed = true;
                textDisplay.text = string.Empty;

                SetMaterial(false);

                curTurtorial = null;

                if (backlogTutorials.Count > 0)
                {
                    backlogTutorials.RemoveAll(t => t.hasPlayed == true);

                    if (backlogTutorials.Count > 0)
                    {
                        StartTutorial(backlogTutorials[0].identifier);
                    }
                }
                else
                {
                    GameManager.Instance.ReturnGameState();
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator TutorialTimer()
    {
        yield return new WaitForSeconds(curTurtorial.maxTutorialTime);
        if (curTurtorial.hasPlayed)
        {
            StepFinished(curTurtorial.identifier, curTurtorial.steps.Count);
        }
    }

    private IEnumerator StepTimer(int currentTutorialIndex, string currentIdentifier)
    {
        yield return new WaitForSeconds(curTurtorial.steps[curTurtorial.stepIndex].maxStepTime);
        if (curTurtorial.stepIndex == currentTutorialIndex )
        {
            StepFinished(currentIdentifier, currentTutorialIndex + 1);
        }
    }
}

