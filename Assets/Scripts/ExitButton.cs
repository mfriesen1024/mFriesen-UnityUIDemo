using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool isHovered;
    Color defaultColour;

    TextMeshProUGUI tmpText;

    void Start()
    {
        UnityEngine.UI.Button btn;
        btn = GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(OnClick);
        isHovered = false;

        tmpText = GetComponentInChildren<TextMeshProUGUI>();
        defaultColour = Color.HSVToRGB(0, 0, 0.1f);
    }

    // Update is called once per frame

    void Update()
    {
        // Nicer Quit
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
    }//*/

    public void OnPointerEnter(PointerEventData data)
    {
        isHovered = true;
        tmpText.color = Color.red;
    }

    public void OnPointerExit(PointerEventData data)
    {
        isHovered = false;
        tmpText.color = defaultColour;
    }

    void OnClick()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null) { canvas.SetActive(false); }

        SceneManager.LoadScene(1);
    }
}
