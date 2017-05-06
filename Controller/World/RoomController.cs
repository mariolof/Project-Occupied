using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

    public static RoomController Instance { get; protected set; }

    public List<Room> roomList { get; protected set; }

    public RoomDetect roomDetect { get; protected set; }


    void OnEnable()
    {
        Instance = this;
    }


    void Start ()
    {
        roomList = new List<Room>();
        roomDetect = new RoomDetect();
    }
	

	void Update ()
    {

    }


    public void SplitRoom(Tile tile)
    {
        List<Tile> startTileList = new List<Tile>();
        List<Tile> tileWithinList = new List<Tile>();

        if(tile.room != null)
        {
            tile.room.tileInRoom.Remove(tile);
            tile.room = null;
        }

        roomDetect.FindStartTile(tile, startTileList); //Debug.Log("startTileList.Count" + startTileList.Count);

        if (startTileList.Count > 1)
        {
            for (int i = 0; i < startTileList.Count; i++)
            {
                Tile startTile = startTileList[i];
                roomDetect.DetectClosedArea(startTile, tileWithinList);

                if (tileWithinList.Count != 0)
                {
                    Debug.Log("startTile is x" + startTile.x + " y" + startTile.y);
                    Debug.Log("new room has " + tileWithinList.Count + " tiles");

                    CreateRoom(tileWithinList);
                    tileWithinList.Clear();
                }
            }
        }
    }


    public void MergerRoom(Tile tile)
    {
        List<Tile> startTileList = new List<Tile>();
        List<Tile> combinedTileList = new List<Tile>();

        roomDetect.FindStartTile(tile, startTileList); //Debug.Log("startTileList.Count" + startTileList.Count);

        if (startTileList.Count > 1)
        {
            bool isOutdoor = false;

            for (int i = 0; i < startTileList.Count; i++)
            {
                Tile startTile = startTileList[i];

                if (startTile.room == null)
                {
                    isOutdoor = true;
                }
                else
                {
                    for (int j = 0; j < startTile.room.tileInRoom.Count; j++)
                    {
                        combinedTileList.Add(startTile.room.tileInRoom[j]);
                    }
                }
            }

            if (isOutdoor == true)
            {
                foreach (Tile t in combinedTileList)
                {
                    t.room.tileInRoom.Clear();
                    t.room = null;
                }
            }
            else
            {
                CreateRoom(combinedTileList);
            }
        }
    }


    public void CreateRoom(List<Tile> tileList)
    {
        if (tileList.Count > 0)
        {
            Room room = new Room();

            for (int i = 0; i < tileList.Count; i++)
            {
                Tile t = tileList[i];

                if (t.room != null)
                {
                    t.room.tileInRoom.Remove(t);
                }

                room.tileInRoom.Add(t);
                t.room = room;
            }

            roomList.Add(room);
        }
    }


    public void AddTileToRoom(Tile t, Room r)
    {
        if(r.tileInRoom.Contains(t) == false)
        {
            r.tileInRoom.Add(t);
            t.room = r;
        }
    }


    public void ClearEmptyRoom()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Room r = roomList[i];

            if (r.tileInRoom.Count == 0)
            {
                roomList.Remove(r); Debug.Log("roomList.Count" + roomList.Count);
            }
        }
    }
}
