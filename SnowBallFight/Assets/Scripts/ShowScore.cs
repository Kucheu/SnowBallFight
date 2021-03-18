using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowScore : MonoBehaviour
{
    [SerializeField]
    TMP_Text blueScoreText, redScoreText;

    Score score;

    private void Start()
    {
        score = FindObjectOfType<Score>();
    }


    private void Update()
    {
        if(score ==  null)
        {
            score = FindObjectOfType<Score>();
            if(score == null) return;
        }
        blueScoreText.text = score.blueScore.ToString();
        redScoreText.text = score.redScore.ToString();

    }
}
