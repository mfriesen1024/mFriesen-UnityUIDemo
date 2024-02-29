using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiderButton : MonoBehaviour
{
    [SerializeField]GameObject toHide;
    bool isHidden ;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(Activate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Activate(){
    if (isHidden) {isHidden = false;} else {isHidden = true;}
    toHide.SetActive(!isHidden);
    }
}
