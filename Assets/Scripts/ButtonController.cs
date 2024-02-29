using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] Button b;
    GameObject managerObj;

    [SerializeField] int value = 0;
    [SerializeField] UpdateType type = (UpdateType)1;

    // Start is called before the first frame update
    void Start()
    {
        Button b2 = b.GetComponent<Button>();
        b2.onClick.AddListener(OnClick);
        managerObj = GameObject.Find("UI Manager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        object[] sendable = { type, value };
        managerObj.SendMessage("ScoreUpdate", sendable);
    }
}
