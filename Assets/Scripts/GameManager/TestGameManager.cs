using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    void Start()
    {
        AbstractLevelManager.Instance.Generate();
    }
}
