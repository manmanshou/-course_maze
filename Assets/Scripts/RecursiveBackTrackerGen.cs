using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 深度优先生成算法
/// </summary>
public class RecursiveBackTrackerGen : MazeGenerator
{
    const int ROOM_SIGN_IDX = (int)RectWallType.WallNum;

    private Stack<RoomCoordinate> mVisitedRooms = new Stack<RoomCoordinate>(); //已访问的房间集合

    void Start()
    {
        if (mRowCount <= 0 || mColCount <= 0)
            return;

        mMazePrefab = Resources.Load<GameObject>("RectCell");

        mMazeData = new int[mRowCount, mColCount, (int)RectWallType.WallNum + 1];

        GenMazeData();

        GenMazeScene();
    }

    public override void GenMazeData()
    {
        int r = Random.Range(0, mRowCount);
        int c = Random.Range(0, mColCount);

        mVisitedRooms.Push(new RoomCoordinate(r, c));

        List<RectWallType> check = new List<RectWallType>((int)RectWallType.WallNum);

        while (mVisitedRooms.Count > 0)
        {
            mMazeData[r, c, ROOM_SIGN_IDX] = 1;

            check.Clear();

            if (c > 0 && mMazeData[r, c - 1, ROOM_SIGN_IDX] == 0)
            {
                check.Add(RectWallType.Left);
            }

            if (r > 0 && mMazeData[r - 1, c, ROOM_SIGN_IDX] == 0)
            {
                check.Add(RectWallType.Down);
            }

            if (c < mColCount - 1 && mMazeData[r, c + 1, ROOM_SIGN_IDX] == 0)
            {
                check.Add(RectWallType.Right);
            }

            if (r < mRowCount - 1 && mMazeData[r + 1, c, ROOM_SIGN_IDX] == 0)
            {
                check.Add(RectWallType.Up);
            }

            //检查是否有一个有效的未访问房间可以继续探索
            if (check.Count > 0) 
            {
                mVisitedRooms.Push(new RoomCoordinate(r, c));

                RectWallType move_dir = check[Random.Range(0, check.Count)];
                if (move_dir == RectWallType.Left)
                {
                    mMazeData[r, c, (int)RectWallType.Left] = 1;
                    c--;
                    mMazeData[r, c, (int)RectWallType.Right] = 1;
                }
                else if (move_dir == RectWallType.Up)
                {
                    mMazeData[r, c, (int)RectWallType.Up] = 1;
                    r++;
                    mMazeData[r, c, (int)RectWallType.Down] = 1;
                }
                else if (move_dir == RectWallType.Right)
                {
                    mMazeData[r, c, (int)RectWallType.Right] = 1;
                    c++;
                    mMazeData[r, c, (int)RectWallType.Left] = 1;
                }
                else if (move_dir == RectWallType.Down)
                {
                    mMazeData[r, c, (int)RectWallType.Down] = 1;
                    r--;
                    mMazeData[r, c, (int)RectWallType.Up] = 1;
                }
            }
            else
            {
                var room = mVisitedRooms.Pop();
                r = room.row;
                c = room.col;
            }
        }
    }
}
