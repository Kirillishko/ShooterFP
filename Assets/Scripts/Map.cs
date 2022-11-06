using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Map : MonoBehaviour
{
    struct WallsAndFloors
    {
        public const char ConcreteWall = '#';
        public const char BrickWall = 'B';
        public const char WoodWall = 'W';
        public const char ConcreteFloorAndCeiling = '¹';
        public const char BrickFloorAndCeiling = 'b';
        public const char WoodFloorAndCeiling = 'w';
        public const char LiftWalls = 'R';
    }

    struct Other
    {
        public const char FakeConcreteWall = '¹';
        public const char FakeBrickWall = 'b';
        public const char FakeWoodWall = 'w';
        public const char Door = 'D';
        public const char ScoutingDrone = 'S';
        public const char ScoutingDroneBroken = 'M';
        public const char CeilingLamp = 'C';
        public const char Lamp = 'L';
        public const char Lift = 'Y';
        public const char Bush = 'B';
        public const char Player = 'P';
        public const char Ammo = 'A';
        public const char Healer = 'H';
        public const char Table = 'T';
    };

    public GameObject ConcreteWall;
    [SerializeField] private GameObject Fuck;
    public GameObject BrickWall;
    public GameObject WoodWall;
    public GameObject ConcreteFloorAndCeiling;
    public GameObject BrickFloorAndCeiling;
    public GameObject WoodFloorAndCeiling;
    public GameObject LiftWalls;

    public GameObject FakeConcreteWall;
    public GameObject FakeBrickWall;
    public GameObject FakeWoodWall;
    public GameObject Door;
    public GameObject ScoutingDrone;
    public GameObject ScoutingDroneBroken;
    public GameObject CeilingLamp;
    public GameObject Lamp;
    public GameObject Lift;
    public GameObject Bush;
    public GameObject Player;
    public GameObject Ammo;
    public GameObject Healer;
    public GameObject Table;

    private string[] _map;
    private string _path;
    private string _pathWallsAndFloors;
    private string _pathOther;

    private string _pathOtherWalls;

    private void Awake()
    {
        _path = Application.dataPath + "/StreamingAssets";
        _pathWallsAndFloors = "/WallsAndFloors.txt";
        _pathOther = "/Other.txt";
    }

    void Start()
    {
        CreateWallsAndFloors();
        CreateOther();
    }

    private void CreateWallsAndFloors()
    {
        _map = File.ReadAllLines(_path + _pathWallsAndFloors);

        for (int i = 0; i < _map.Length; i++)
        {
            for (int j = 0; j < _map[i].Length; j++)
            {
                if (_map[i][j] == '/') continue;
                switch (_map[i][j])
                {
                    case WallsAndFloors.ConcreteWall:               { Instantiate(ConcreteWall,             new Vector3(i, 0, j),       ConcreteWall.transform.rotation);               break;}
                    case WallsAndFloors.BrickWall:                  { Instantiate(BrickWall,                new Vector3(i, 0, j),       BrickWall.transform.rotation);                  break;}
                    case WallsAndFloors.WoodWall:                   { Instantiate(WoodWall,                 new Vector3(i, 0, j),       WoodWall.transform.rotation);                   break;}
                    case WallsAndFloors.ConcreteFloorAndCeiling:    { Instantiate(ConcreteFloorAndCeiling,  new Vector3(i, -1, j),      ConcreteFloorAndCeiling.transform.rotation);    break;}
                    case WallsAndFloors.BrickFloorAndCeiling:       { Instantiate(BrickFloorAndCeiling,     new Vector3(i, -1, j),      BrickFloorAndCeiling.transform.rotation);       break;}
                    case WallsAndFloors.WoodFloorAndCeiling:        { Instantiate(WoodFloorAndCeiling,      new Vector3(i, -1, j),      WoodFloorAndCeiling.transform.rotation);        break;}
                }
            }
        }

        NavMeshBaker navMeshBaker = GetComponent<NavMeshBaker>();
        navMeshBaker.Bake();
    }

    private void CreateOther()
    {
        _map = File.ReadAllLines(_path + _pathOther);

        for (int i = 0; i < _map.Length; i++)
        {
            for (int j = 0; j < _map[i].Length; j++)
            {
                if (_map[i][j] == '/' || _map[i][j] == '`') continue;
                switch (_map[i][j])
                {
                    case Other.FakeConcreteWall:    { Instantiate(FakeConcreteWall,     new Vector3(i, 0, j), FakeConcreteWall.transform.rotation);         break;}
                    case Other.FakeBrickWall:       { Instantiate(FakeBrickWall,        new Vector3(i, 0, j), FakeBrickWall.transform.rotation);        break;}
                    case Other.FakeWoodWall:        { Instantiate(FakeWoodWall,         new Vector3(i, 0, j), FakeWoodWall.transform.rotation);         break;}
                    case Other.Door:
                        {
                            if (_map[i][j - 1] == '`' && _map[i][j + 1] == '`')
                            {
                                GameObject temp = Instantiate(Door, new Vector3(i, -0.5f, j), Door.transform.rotation);
                                temp.transform.Rotate(0f,90f,0f);
                            }
                            else Instantiate(Door, new Vector3(i, -0.5f, j), Door.transform.rotation);
                            break;
                        }
                    case Other.ScoutingDrone:       { Instantiate(ScoutingDrone,        new Vector3(i, 1, j), ScoutingDrone.transform.rotation);        break; }
                    case Other.ScoutingDroneBroken: { Instantiate(ScoutingDroneBroken,  new Vector3(i, 0, j), ScoutingDroneBroken.transform.rotation);  break; }
                    case Other.CeilingLamp:         { Instantiate(CeilingLamp,          new Vector3(i, 2.1f, j), CeilingLamp.transform.rotation);          break; }
                    case Other.Lamp:                { Instantiate(Lamp,                 new Vector3(i, 0, j), Lamp.transform.rotation);                 break; }
                    case Other.Lift:                { Instantiate(Lift,                 new Vector3(i, 0, j), Lift.transform.rotation);                 break; }
                    case Other.Bush:                { Instantiate(Bush,                 new Vector3(i, 0, j), Bush.transform.rotation);                 break; }
                    case Other.Player:              { Instantiate(Player,               new Vector3(i, 0.5f, j), Player.transform.rotation);               break; }
                    case Other.Ammo:                { Instantiate(Ammo,                 new Vector3(i, 0, j), Ammo.transform.rotation);                 break; }
                    case Other.Healer:              { Instantiate(Healer,               new Vector3(i, -0.4f, j), Healer.transform.rotation);               break; }
                    case Other.Table:               { Instantiate(Table,                new Vector3(i, -0.048f, j), Table.transform.rotation);               break; }
                }
            }
        }
    }
}
