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
        public MeshRenderer[] meshRenderers;

        public int textIndex = 0;
    }

    [SerializeField] private List<Tutorial> tutorials = new List<Tutorial>();
    [SerializeField] private List<Tutorial> backlogTutorials = new List<Tutorial>();

    private Tutorial curTurtorial;
    [SerializeField] private TMP_Text textDisplay;

    public static TutorialManager instance;

    [SerializeField] private Material outlineMat;


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

                if (curTurtorial.meshRenderers[curTurtorial.textIndex] != null)
                    SetMaterial(curTurtorial.meshRenderers[curTurtorial.textIndex], false);
            }
            else
            {
                backlogTutorials.Add(tempTutorial);
            }
        }


    }

    private void SetMaterial(MeshRenderer meshRenderer, bool addOutline)
    {
        List<Material> materials = new List<Material>();
        Material origin = null;

        if (meshRenderer != null)
        {
            origin = meshRenderer.material;
        }

        if (origin != null && origin != outlineMat)
        {
            materials.Add(origin);
        }

        if (addOutline)
        {
            materials.Add(outlineMat);
        }

        meshRenderer.SetMaterials(materials);
    }

    /// <summary>
    /// fill nextStep if a specific step is wanted else will just play next step. Identiefier required. Returns true if the next step could be called
    // / </summary>
    /// <param name="finishedStep"></param>
    public virtual bool StepFinished(string identifier, int nextStep = default(int))
    {
        if (curTurtorial != null && !curTurtorial.hasPlayed && identifier == curTurtorial.identifier && nextStep > curTurtorial.textIndex)
        {
            if (curTurtorial.textIndex < curTurtorial.texts.Length - 1)
            {
                if (curTurtorial.meshRenderers[curTurtorial.textIndex] != null)
                    SetMaterial(curTurtorial.meshRenderers[curTurtorial.textIndex], false);

                if (nextStep != default(int))
                {
                    curTurtorial.textIndex = nextStep;
                }
                else
                {
                    curTurtorial.textIndex++;
                }

                if (curTurtorial.meshRenderers[curTurtorial.textIndex] != null)
                    SetMaterial(curTurtorial.meshRenderers[curTurtorial.textIndex], false);

                textDisplay.text = curTurtorial.texts[curTurtorial.textIndex];
            }
            else
            {
                curTurtorial.hasPlayed = true;
                textDisplay.text = string.Empty;

                if (curTurtorial.meshRenderers[curTurtorial.textIndex] != null)
                    SetMaterial(curTurtorial.meshRenderers[curTurtorial.textIndex], false);

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
}

