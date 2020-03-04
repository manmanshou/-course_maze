using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prim算法，迷宫的房间坐标是左下角是0，0，X正方向是右，Z正方向是上
/// </summary>
public class PrimRectGen : MazeGenerator
{
    public const int ROOM_SIGN_IDX = (int)RectWallType.WallNum;    //房间标记位索引0-3为墙

    private List<RoomCoordinate> mWaitProcRooms = new List<RoomCoordinate>();  //待处理房间集合

    void Start()
    {
        if (mRowCount <= 0 || mColCount <= 0)
            return;

        mMazeData = new int[mRowCount, mColCount, (int)RectWallType.WallNum + 1];

        mMazePrefab = Resources.Load<GameObject>("RectCell");

        GenMazeData();

        GenMazeScene();
    }

    /// <summary>
    /// 生成迷宫，标记位0表示未访问、1表示已访问，2表示已添加到待处理列表
    /// </summary>
    public override void GenMazeData()
    {
        int startRow = Random.Range(0, mRowCount);
        int startCol = Random.Range(0, mColCount);

        mWaitProcRooms.Add(new RoomCoordinate(startRow, startCol));

        List<RectWallType> check = new List<RectWallType>((int)RectWallType.WallNum);

        while (mWaitProcRooms.Count > 0)
        {
            int i = Random.Range(0, mWaitProcRooms.Count);
            int r = mWaitProcRooms[i].row;
            int c = mWaitProcRooms[i].col;

            mMazeData[r, c, ROOM_SIGN_IDX] = 1; //这个房间被访问

            int lastIdx = mWaitProcRooms.Count - 1;
            mWaitProcRooms[i] = mWaitProcRooms[lastIdx];
            mWaitProcRooms.RemoveAt(lastIdx);

            //左边
            if (c > 0)
            {
                if (mMazeData[r, c - 1, ROOM_SIGN_IDX] == 1)
                    check.Add(RectWallType.Left);
                else if (mMazeData[r, c - 1, ROOM_SIGN_IDX] == 0)
                {
                    mWaitProcRooms.Add(new RoomCoordinate(r, c - 1));
                    mMazeData[r, c - 1, ROOM_SIGN_IDX] = 2;
                }
            }

            //上边
            if (r < mRowCount - 1)
            {
                if (mMazeData[r + 1, c, ROOM_SIGN_IDX] == 1)
                    check.Add(RectWallType.Up);
                else if (mMazeData[r + 1, c, ROOM_SIGN_IDX] == 0)
                {
                    mWaitProcRooms.Add(new RoomCoordinate(r + 1, c));
                    mMazeData[r + 1, c, ROOM_SIGN_IDX] = 2;
                }
            }

            //右边
            if (c < mColCount - 1)
            {
                if (mMazeData[r, c + 1, ROOM_SIGN_IDX] == 1)
                    check.Add(RectWallType.Right);
                else if (mMazeData[r, c + 1, ROOM_SIGN_IDX] == 0)
                {
                    mWaitProcRooms.Add(new RoomCoordinate(r, c + 1));
                    mMazeData[r, c + 1, ROOM_SIGN_IDX] = 2;
                }
            }

            //下边
            if (r > 0)
            {
                if (mMazeData[r - 1, c, ROOM_SIGN_IDX] == 1)
                    check.Add(RectWallType.Down);
                else if (mMazeData[r - 1, c, ROOM_SIGN_IDX] == 0)
                {
                    mWaitProcRooms.Add(new RoomCoordinate(r - 1, c));
                    mMazeData[r - 1, c, ROOM_SIGN_IDX] = 2;
                }
            }

            if (check.Count > 0)
            {
                RectWallType move_dir = check[Random.Range(0, check.Count)];
                if (move_dir == RectWallType.Left)
                {
                    mMazeData[r, c, (int)RectWallType.Left] = 1;
                    mMazeData[r, c - 1, (int)RectWallType.Right] = 1;
                }
                else if (move_dir == RectWallType.Right)
                {
                    mMazeData[r, c, (int)RectWallType.Right] = 1;
                    mMazeData[r, c + 1, (int)RectWallType.Left] = 1;
                }
                else if (move_dir == RectWallType.Up)
                {
                    mMazeData[r, c, (int)RectWallType.Up] = 1;
                    mMazeData[r + 1, c, (int)RectWallType.Down] = 1;
                }
                else if (move_dir == RectWallType.Down)
                {
                    mMazeData[r, c, (int)RectWallType.Down] = 1;
                    mMazeData[r - 1, c, (int)RectWallType.Up] = 1;
                }
            }

            check.Clear();
        }
    }

}
