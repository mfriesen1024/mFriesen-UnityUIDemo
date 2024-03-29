using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum UpdateType { add, set, random }
enum colorChannel { r, g, b, a }

public class UIManager : MonoBehaviour
{
    [Header("Score things")]
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI scoreObj;
    string scoreText = "";

    [Header("Image things")]
    [SerializeField] GameObject rockImageObj;
    float r = 1, g = 1, b = 1, a = 1;
    Color color;
    bool isHidden;

    [Header("HP things")]
    [SerializeField] Slider hpBar;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] Image hpFill;
    [SerializeField] int maxHP; int hp;
    [SerializeField] Color healthyColour;
    [SerializeField] Color criticalColour;
    UpdateType hpUpdateType = UpdateType.set;

    [Header("Event things")]
    [SerializeField] string eventName = "Untitled Event";
    [SerializeField] Slider eventBar;
    [SerializeField] TextMeshProUGUI eventText;
    [SerializeField] float maxTime = 5; float eventTimer;

    // Image Slider tomfoolery
    colorChannel channel;

    System.Random random = new();

    // Start is called before the first frame update
    void Start()
    {
        color = new(r, g, b, a);
        FormatText();

        // Force an update to max hp on start to prevent user from seeing sloppy text.
        UpdateHP(maxHP);

        eventTimer = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

        if (scoreObj != null) { scoreObj.text = scoreText; }

        UpdateEvent();
    }

    public void ScoreUpdate(object input)
    // This is how I would have done it using listeners.
    // It works, but apparently the ask was to use the inspector as much as possible.
    // (Frankly, I think this was poorly communicated, though I may have just been absent.)
    {
        // Convert to enum and int
        object[] newInput = (object[])input;
        UpdateType type = (UpdateType)newInput[0];
        int value = (int)newInput[1];
        ScoreUpdate(type, value);
    }

    private void ScoreUpdate(UpdateType type, int value)
    {
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

    public void ScoreUpdate(int value)
    // This meets the use inspector ask.
    {
        // This has a noteworthy limitation, only 0 can reset score, and you cannot add 0 score (meaning no change).
        UpdateType type = UpdateType.add;
        if (value == 0) { type = UpdateType.set; }

        ScoreUpdate(type, value);
    }
    public void SetRandomScore()
    // This is to allow the inspector to call a method to random update. I'd rather use something a little more organized, but unity quirks I suppose.
    {
        // Update with 0 value because random will simply override it.
        ScoreUpdate(UpdateType.random, 0);
    }

    public void ShowHideRock()
    {
        if (isHidden) { isHidden = false; } else { isHidden = true; }
        rockImageObj.SetActive(!isHidden);
    }

    public void SetChannel(int channelID) // Call this from inspector to set a channel
    { channel = (colorChannel)channelID; }
    public void SliderUpdate(object[] input)
    // This is what I would have used.
    // This is a bit more organized in my eyes because I don't need 4-5 methods, despite having to parse an object array.
    {
        colorChannel channel = (colorChannel)input[1]; float value = (float)input[0];

        Debug.Log($"{channel} {value}");

        SliderUpdate(channel, value);
    }
    public void SliderUpdate(float value)
    // This is the overload we call from inspector.
    {
        SliderUpdate(channel, value);
    }
    private void SliderUpdate(colorChannel channel, float value)
    {
        switch (channel)
        {
            case colorChannel.r: color.r = value; break;
            case colorChannel.g: color.g = value; break;
            case colorChannel.b: color.b = value; break;
            case colorChannel.a: color.a = value; break;
        }

        Image image = rockImageObj.GetComponent<Image>();

        Debug.Log(image.gameObject.name);

        image.color = color;

        Color c = image.color;

        Debug.Log($"{c.r}, {c.g}, {c.b}, {c.a}");
    }

    public void SetHPUpdateType(int value) { hpUpdateType = (UpdateType)value; }
    public void UpdateHP(int value)
    {
        switch (hpUpdateType)
        {
            case UpdateType.add: hp += value; break;
            case UpdateType.set: hp = value; break;
            default: throw new InvalidOperationException($"Update type {hpUpdateType} is either not valid or not implemented for UpdateHP.");
        }

        hp = Mathf.Clamp(hp, 0, maxHP);

        string text = $"{hp}/{maxHP} HP";
        float localHP = hp, localMaxHP = maxHP;
        hpText.text = text;
        float fillValue = localHP / localMaxHP;
        hpBar.value = fillValue;

        hpFill.color = Color.Lerp(criticalColour, healthyColour, fillValue);
        

        if (hp == 0) { OnDeath(); }
    }
    public void ResetEventTimer() { eventTimer = maxTime; }
    private void UpdateEvent()
    {
        eventTimer -= Time.deltaTime;
        eventTimer = Mathf.Clamp(eventTimer, 0, maxTime);

        
        int minutes = 0, seconds = 0;
        // for some reason, a while loop broke unity. I'm too lazy to fix it.
        for (int i = (int)eventTimer; i > 60; i -= 60)
        {
            minutes++;
            seconds = i - 60;
        }

        string printable = $"{eventName}: {minutes}:{seconds}";

        eventText.text = printable;

        float fillValue = eventTimer / maxTime;

        eventBar.value = fillValue;
    }
    public void SetEventName(string name) { eventName = name; }

    public void ExitGame() { SceneManager.LoadScene(1); }

    void FormatText()
    {
        ClampScore();

        string tempText = score.ToString();

        // I don't get why the example provided in class added leading 0s, but it did, so I did that myself. I mistakenly thought it was the ask.
        string zero = string.Empty; // use this as a very lazy way of undoing the leading 0 thing.
        //string zero = "0";
        //if (tempText.Length < 9) { while (zero.Length != (9 - tempText.Length)) { zero += "0"; } }
        //else { zero = string.Empty; }

        scoreText = zero + tempText;
    }

    void ClampScore()
    {
        int tempScore = score;
        score = Mathf.Clamp(score, 0, 999999999);
        if (tempScore < 0) { Debug.Log($"Attempted to set score to {tempScore}, clamp set it to {score}."); }
    }

    void OnDeath()
    {
        Debug.Log("Death is not implemented, but this isn't an error either.");
    }
}
