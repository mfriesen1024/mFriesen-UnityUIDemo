using System;
using TMPro;
using UnityEngine;

enum UpdateType { add, set, random }

public class UIManager : MonoBehaviour
{
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI scoreObj;
    string scoreText = "";

    System.Random random = new();

    // Start is called before the first frame update
    void Start()
    {
        FormatText();
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreObj != null) { scoreObj.text = scoreText; }
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
            case UpdateType.random: score = random.Next(-999999999, 1000000000); break;
            default: throw new NotImplementedException($"{type} is not implemented in ScoreUpdate");
        }

        Debug.Log($"Got input of type {type}, with value {value}");
        FormatText();
    }

    void FormatText()
    {
        ClampScore();

        string tempText = score.ToString();

        string zero = "0";
        while (zero.Length != (9 - tempText.Length)) { zero += "0"; }

        scoreText = zero + tempText;
    }

    void ClampScore() {
    int tempScore = score;
    score = Mathf.Clamp(score, 0, 999999999);
    if (score < 0){Debug.LogError($"Random attempted to set score to {tempScore}, clamp set it to {score}.");}
    }

    // This is a method that would handle ints instead of an object array, if I used the builtin listener.
    // However, I think that would just be really wacky and require weird workarounds, so forget that.
    public void SampleScoreUpdate(int value)
    {
        // This is a local score variable so that the score is never broken by accidental use.
        int outputFromInt1 = 10; UpdateType type1;
        int outputFromInt2 = 10; UpdateType type2; int sampleValue = 0;

        // Determine if value is 0 so we can set score instead of add. This is wrong.
        if (value == 0) { outputFromInt1 = 0; type1 = UpdateType.set; } else { outputFromInt1 += value; type1 = UpdateType.add; }
        if (sampleValue == 0) { outputFromInt2 = 0; type2 = UpdateType.set; } else { outputFromInt2 += sampleValue; type2 = UpdateType.add; }

        // The flaw with the above is that we cannot set a value other than 0 without another if/else.
        // Even then, we cannot set a nonzero value and add the same value.

        // Create some sample inputs.
        string sampleSetInput = "set:14";
        string sampleAddInput = "add:16";
        // Here is another method. This is just wack because strings are weird.
        int outputFromStringSet = ParseStringInput(sampleSetInput, out UpdateType outputSetType);
        int outputFromStringAdd = ParseStringInput(sampleAddInput, out UpdateType outputAddType);

        Debug.Log($"This method call works, got input value of {value}.");
        Debug.Log($"Output from the int method is type {type1} with value {outputFromInt1}");
        Debug.Log($"Output from the int method is type {type2} with value {outputFromInt2}");
        Debug.Log($"Output from the string method with {outputSetType} mode is {outputFromStringSet}");
        Debug.Log($"Output from the string method with {outputAddType} mode is {outputFromStringAdd}");
        throw new NotImplementedException("This is a sample method, it doesn't do anything!");
    }

    // Here is another method. This is just wack because strings are weird. It also requires manual updating.
    int ParseStringInput(string input, out UpdateType type)
    {
        //
        int location = 1 + input.IndexOf(':');
        int output = int.Parse(input.Substring(location));

        // Get type
        type = UpdateType.set;
        string switchString = input.Substring(0, location - 1);
        switch (switchString)
        {
            case "set": type = UpdateType.set; break;
            case "add": type = UpdateType.add; break;
            default: throw new NotImplementedException($"Type {switchString} is not implemented!");
        }
        return output;
    }
}
