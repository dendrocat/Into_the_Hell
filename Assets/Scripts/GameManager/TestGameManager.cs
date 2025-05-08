using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    void Start()
    {
        StaticLevelManager.Instance.Generate(Locations.FrozenCaves);
    }
}
