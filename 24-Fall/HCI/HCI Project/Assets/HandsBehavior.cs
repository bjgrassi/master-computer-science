using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsBehavior : MonoBehaviour
{
    private KeyCode[] keyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0,
    };
    private KeyCode[] keyCodesPad = {
        KeyCode.Keypad1,
        KeyCode.Keypad2,
        KeyCode.Keypad3,
        KeyCode.Keypad4,
        KeyCode.Keypad5,
        KeyCode.Keypad6,
        KeyCode.Keypad7,
        KeyCode.Keypad8,
        KeyCode.Keypad9,
        KeyCode.Keypad0,
    };
    public List<GameObject> hands;

    // Start is called before the first frame update
    void Start()
    {
        hideAllHands();
    }

    void Update()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]) || Input.GetKeyDown(keyCodesPad[i]))
            {
                ShowHandModel(i);
            }
        }
    }


    // Logic to display the 3D model for the number
    void ShowHandModel(int number)
    {
        hideAllHands();

        var realNumber = number + 1;
        Debug.Log("pressed " + realNumber);

        hands[number].SetActive(true);
    }

    void hideAllHands()
    {
        foreach (var hand in hands)
        {
            hand.SetActive(false);
        }
    }
}
