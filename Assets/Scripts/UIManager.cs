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

    void ClampScore() { score = Mathf.Clamp(score, 0, 999999999); }

    // This is a method that would handle ints instead of an object array, if I used the builtin listener.
    // However, I think that would just be really wacky and require weird workarounds, so forget that.
    public void SampleScoreUpdate(int value)
    {
        // This is a local score variable so that the score is never broken by accidental use.
        int outputFromInt = 0;

        // Determine if value is 0 so we can set score instead of add. This is wrong.
        if (value == 0) { outputFromInt = 0; } else { outputFromInt += value; }

        // The flaw with the above is that we cannot set a value other than 0 without another if/else.
        // Even then, we cannot set a nonzero value and add the same value.

        // Create some sample inputs.
        string sampleSetInput = "set:14";
        string sampleAddInput = "add:16";
        // Here is another method. This is just wack because strings are weird.
        int outputFromStringSet = ParseStringInput(sampleSetInput, out UpdateType outputSetType);
        int outputFromStringAdd = ParseStringInput(sampleAddInput, out UpdateType outputAddType);

        Debug.Log($"This method call works, got input value of {value}.");
        Debug.Log($"Output from the int method is {outputFromInt}");
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
