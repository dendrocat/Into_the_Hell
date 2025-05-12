using System.Collections.Generic;
using UnityEngine;

// <summary>
/// UI мини-карты, отображающей комнаты уровня в виде сетки.
/// </summary>
public class MiniMapUi : MonoBehaviour {
    private List<GameObject> rooms = new();
    private RectTransform window;
    static MiniMapUi Instance = null;

    [Tooltip("Размер одной комнаты на мини-карте")]
    [SerializeField] static int roomSize = 50;

    [Tooltip("Отступ вокруг комнаты на мини-карте")]
    [SerializeField] static int roomOffset = 10;

    [Tooltip("Префаб комнаты для мини-карты")]
    [SerializeField] GameObject mapRoom;

    void Awake() {
        if(Instance){
            Destroy(gameObject);
            return;
        }
        window = GetComponent<RectTransform>();
        Instance = this;
        this.window.sizeDelta = Vector2.zero;
    }

    /// <summary>
    /// Настраивает мини-карту по данным уровня.
    /// </summary>
    /// <param name="level"><see cref="Level">Уровень</see> с информацией о комнатах.</param>
    public void SetUpLevel(Level level){
        window.sizeDelta = new Vector2(level.rooms.w, level.rooms.h) 
                * (roomSize+roomOffset*2);
        DestroyUI();

        for(int i=0;i<level.rooms.w;++i)
        for(int j=0;j<level.rooms.h;++j){
            if(level.rooms[i, j] != null){
                var room = Instantiate(mapRoom, transform);
                rooms.Add(room);
                if(level.rooms[i, j]._isStartRoom){
                    room.GetComponent<RectTransform>().sizeDelta = Vector2.one * (roomSize - roomOffset);
                }else room.GetComponent<RectTransform>().sizeDelta = Vector2.one * roomSize;
                room.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(i+0.5f, j+0.5f) * (roomSize+2*roomOffset);
            }
        }
    }

    /// <summary>
    /// Уничтожает все объекты комнат на мини-карте.
    /// </summary>
    public void DestroyUI(){
        foreach(var room in rooms)Destroy(room);
        rooms.Clear();
    }

    void OnDestroy() { 
        DestroyUI();
        if (Instance == this) Instance = null; 
    }
}
