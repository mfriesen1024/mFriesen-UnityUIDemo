using System;
using TMPro;
using UnityEngine;

enum UpdateType { add, set }

public class UIManager : MonoBehaviour
{
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI scoreObj;
    string scoreText = "";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        scoreObj.text = scoreText;
    }

    public void ScoreUpdate(object input)
    {
        // Convert to enum and int
        object[] newInput = (object[])input;
        UpdateType type = (UpdateType)newInput[0];
        int value = (int)newInput[1];

        switch (type)
        {
            case UpdateType.add: score += value; break;
            case UpdateType.set: score = value; break;
            default: throw new NotImplementedException($"{type} is not implemented in ScoreUpdate");
        }
    }
}