using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        None,
        Menu,
        Load,
        Play,
        Pause,
    }

    [SerializeField] GameState CurrentState;
    [SerializeField] int activeRoom;
    [SerializeField] SlimeController PlayerPrefab;

    public LevelSettings[] GameLevels;
    public ushort SelectedLevel;
    public float LevelStartTime;
    //[SerializeField] int MaxRooms;
    //[SerializeField] RoomController[] RoomVars;
    [SerializeField] List<RoomController> SpawnedRooms;
    [SerializeField] List<int> used_index;
    //[SerializeField] RoomController StartRoom;
    //[SerializeField] RoomController EndRoom;

    [Space]
    [Header("Score")]
    public int JumpCount;
    public float TimeCount;
    public int CoinCount;
    public int BounceCount;
    public int RespawnCount;

    public SlimeController PlayerInstance { get; private set; }
    public static GameManager instance { get; private set; }
    
    void Awake()
    {
        
    }

    private void Start()
    {
        instance = this;

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (CurrentState)
            {
                case GameState.Play:
                    SwitchState(GameState.Pause);
                    break;
                case GameState.Pause:
                    SwitchState(GameState.Play);
                    break;
            }
        }
    }


    // GAMEPLAY
    public void SwitchState(GameState new_state)
    {
        Debug.Log($"Switching state from {CurrentState} to {new_state}!");
        switch (new_state)
        {
            case GameState.None:
                //UIManager.instance.SwitchState(UIManager.UIState.LevelMenu);
                CurrentState = new_state;
                break;
            case GameState.Menu:
                Time.timeScale = 1f;
                UIManager.instance.SwitchState(UIManager.UIState.LevelMenu);
                ClearLevel();
                //Write code for deletion all spawned objects
                CurrentState = new_state;
                break;
            case GameState.Load:
                UIManager.instance.SwitchState(UIManager.UIState.Load);
                LoadLevel();
                Respawn();
                CurrentState = new_state;
                LevelStartTime = Time.time;

                SwitchState(GameState.Play);
                break;
            case GameState.Play:
                //PlayerInstance.transform.position = SpawnedRooms[activeRoom].spawn.GetComponentInChildren<TriggerZone>().transform.position;
                UIManager.instance.SwitchState(UIManager.UIState.GameUI);
                Time.timeScale = 1f;
                CurrentState = new_state;
                break;
            case GameState.Pause:
                
                UIManager.instance.SwitchState(UIManager.UIState.GamePause);
                Time.timeScale = 0f;
                CurrentState = new_state;
                break;
        }
        
    }

    public void Respawn()
    {
        if (PlayerInstance == null)
        {
            PlayerInstance = Instantiate(PlayerPrefab, SpawnedRooms[activeRoom].spawn.GetComponentInChildren<TriggerZone>().transform.position, Quaternion.identity);
        }
        else
        {
            PlayerInstance.transform.position = SpawnedRooms[activeRoom].spawn.GetComponentInChildren<TriggerZone>().transform.position;
            //PlayerInstance.ZeroVelocity();
            PlayerInstance.ResetSlime();
        }
    }

    public void SpawnTriggered(SpawnPoint new_sp)
    {
        if (new_sp != SpawnedRooms[activeRoom].spawn)
        {
            SpawnedRooms[activeRoom].CloseExit();
            if (CheckLevelComplete())
            {
                LevelComplete();
                return;
            }
            activeRoom++;
            Debug.Log($"> Spawn Triggered! (Active room: {activeRoom}/{GameLevels[SelectedLevel].RoomCount})");
        }
    }

    public bool CheckLevelComplete()
    {
        if (GameLevels[SelectedLevel].name != "Level Test 01")
        {
            return activeRoom >= GameLevels[SelectedLevel].RoomCount - 1;
        }
        else
        {
            return activeRoom >= GameLevels[SelectedLevel].RoomCount;
        }
        
        
    }

    public void LevelComplete()
    {
        Debug.Log(">>> Level Compelted! <<<");
        Time.timeScale = 0f;
        SwitchState(GameState.Menu);
        //UIManager.instance.SwitchState(UIManager.UIState.LevelComplete);

    }

    public void OnCoinPickUp(Coin coin)
    {
        SpawnedRooms[activeRoom].CoinPickedUp(coin);
    }

    public void ResetLevel()
    {
        foreach (RoomController room in SpawnedRooms)
        {
            room.ResetRoom();
        }
        activeRoom = 0;
        Respawn();
    }



    // LEVEL GENERATION
    public void CheckLevelSettings(LevelSettings level)
    {
        if (level.RoomCount > level.RoomVars.Length)
        {
            Debug.LogWarning($"Room count of {level.LevelName} had to be shortened due to lack of RoomVars");
            level.RoomCount = level.RoomVars.Length + 1;
        }
    }

    void SpawnRoom(LevelSettings level, RoomController new_room = null)
    {
        int new_index = -1;
        //-1 for the room variant with index 0 to spawn
        while (new_room == null)
        {
            new_index = Random.Range(0, level.RoomVars.Length);
            new_room = level.RoomVars[new_index];
            if (used_index.Count > 0)
            {
                foreach (int index in used_index)
                {
                    if (index == new_index)
                    {
                        new_room = null;
                        break;
                    }
                }
            }
        }
        used_index.Add(new_index);
        SpawnedRooms.Add(PlaceRoom(new_room));
    }

    RoomController PlaceRoom(RoomController new_room)
    {
        if (SpawnedRooms.Count > 0)
        {
            Vector3 pos_offset = new Vector3(11, 1);
            return Instantiate(new_room, SpawnedRooms[SpawnedRooms.Count-1].GetExitPos() - new_room.GetSpawnPos() + pos_offset, Quaternion.identity);
            //return Instantiate(new_room, SpawnedRooms[SpawnedRooms.Count-1].GetConnectionPoint() + new Vector3(12, 1), Quaternion.identity);
        }
        else
        {
            return Instantiate(new_room, Vector2.zero, Quaternion.identity);
        }
    }

    public void LoadLevel(LevelSettings levelToLoad = null)
    {
        if (levelToLoad == null)
        {
            levelToLoad = GameLevels[SelectedLevel];
        }
        activeRoom = 0;
        CheckLevelSettings(levelToLoad);
        SpawnRoom(levelToLoad, levelToLoad.StartRoom);
        //PlaceRoom(StartRoom);
        if (levelToLoad.KeepRoomOrder)
        {
            levelToLoad.RoomCount = levelToLoad.RoomVars.Length+1;
            foreach (RoomController room in levelToLoad.RoomVars)
            {
                SpawnRoom(levelToLoad, room);
            }
        }
        else
        {
            int i = 0;
            while (levelToLoad.RoomCount > used_index.Count && i < 30)
            {
                SpawnRoom(levelToLoad);
                i++;
            }
        }
        SpawnRoom(levelToLoad, levelToLoad.EndRoom);
    }

    public bool CheckHasLevelLoaded()
    {
        return SpawnedRooms.Count != 0;
    }

    public void ClearLevel()
    {
        Debug.Log("Clearing Level!");

        if(PlayerInstance)
            Destroy(PlayerInstance.gameObject);

        foreach (RoomController room in SpawnedRooms)
        {
            Destroy(room.gameObject);
        }
        used_index.Clear();
        SpawnedRooms = new List<RoomController>();
    }

    public void ClearLevelEditor()
    {
        Debug.Log("Clearing Level!");
        foreach (RoomController room in SpawnedRooms)
        {
            DestroyImmediate(room.gameObject);
        }
        used_index.Clear();
        SpawnedRooms = new List<RoomController>();
    }
}
