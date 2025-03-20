using System.Collections;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (InputManager.Instance.CurrentInputMap)
            {
                case InputMap.UI:
                    InputManager.Instance.SwitchInputMap(InputMap.Gameplay);
                    break;

                case InputMap.Gameplay:
                    InputManager.Instance.SwitchInputMap(InputMap.UI);
                    break;
            }
        }
    }
}
