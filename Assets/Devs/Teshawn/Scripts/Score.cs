using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int finalScore;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        finalScore = PlayerPrefs.GetInt("score", 0);
    }

    private void Start()
    {
        scoreText.text = finalScore.ToString();
    }
}
