using System.Collections.Generic;
using UnityEngine;

//六边形单元组成的迷宫，Prim算法生成
//迷宫的房间坐标是左下角是0，0，X正方向是右，Z正方向是上，奇数列比偶数列更靠上错开
public class PrimHexagonGen : MazeGenerator
{
    const int ROOM_SIGN_IDX = (int)HexWallType.WallNum;

    private List<RoomCoordinate> mVisitedRooms = new List<RoomCoordinate>();  //未访问房间集合

    void Start()
    {
        if (mRowCount <= 0 || mColCount <= 0)
            return;

        mMazePrefab = Resources.Load<GameObject>("HexagonCell");

        mMazeData = new int[mRowCount, mColCount, (int)HexWallType.WallNum + 1];

        GenMazeData();

        GenMazeScene();
    }

    public override void GenMazeData()
    {
        int startRow = Random.Range(0, mRowCount);
        int startCol = Random.Range(0, mColCount);

        mVisitedRooms.Add(new RoomCoordinate(startRow, startCol));

        List<HexWallType> check = new List<HexWallType>((int)HexWallType.WallNum);   //单元格可打通的墙列表

        while (mVisitedRooms.Count > 0)
        {
            int i = Random.Range(0, mVisitedRooms.Count);
            int r = mVisitedRooms[i].row;
            int c = mVisitedRooms[i].col;

            mMazeData[r, c, ROOM_SIGN_IDX] = 1; //这个房间被访问

            //这里删除掉标记被访问过的房间
            int lastIdx = mVisitedRooms.Count - 1;
            mVisitedRooms[i] = mVisitedRooms[lastIdx];
            mVisitedRooms.RemoveAt(lastIdx);

            //遍历周围的六个房间，如果有已访问的房间，则加入下一步随机挑选要打通列表，否则如果是没有访问的房间则添加到历史记录

            if (r > 0) //正下房间
            {
                if (mMazeData[r - 1, c, ROOM_SIGN_IDX] == 1)
                    check.Add(HexWallType.Down);
                else if (mMazeData[r - 1, c, ROOM_SIGN_IDX] == 0)
                {
                    mVisitedRooms.Add(new RoomCoordinate(r - 1, c));
                    mMazeData[r - 1, c, ROOM_SIGN_IDX] = 2;
                }
            }

            if (r < mRowCount - 1) //正上
            {
                if (mMazeData[r + 1, c, ROOM_SIGN_IDX] == 1)
                    check.Add(HexWallType.Up);
                else if (mMazeData[r + 1, c, ROOM_SIGN_IDX] == 0)
                {
                    mVisitedRooms.Add(new RoomCoordinate(r + 1, c));
                    mMazeData[r + 1, c, ROOM_SIGN_IDX] = 2;
                }
            }

            if (c % 2 == 0) //偶数列
            {
                if (c > 0) //左上房间
                {
                    if (mMazeData[r, c - 1, ROOM_SIGN_IDX] == 1)
                        check.Add(HexWallType.LeftUp);
                    else if (mMazeData[r, c - 1, ROOM_SIGN_IDX] == 0)
                    {
                        mVisitedRooms.Add(new RoomCoordinate(r, c - 1));
                        mMazeData[r, c - 1, ROOM_SIGN_IDX] = 2;
                    }
                }

                if (c < mColCount - 1) //右上房间
                {
                    if (mMazeData[r, c + 1, ROOM_SIGN_IDX] == 1)
                        check.Add(HexWallType.RightUp);
                    else if (mMazeData[r, c + 1, ROOM_SIGN_IDX] == 0)
                    {
                        mVisitedRooms.Add(new RoomCoordinate(r, c + 1));
                        mMazeData[r, c + 1, ROOM_SIGN_IDX] = 2;
                    }
                }

                if (c > 0 && r > 0) //左下
                {
                    if (mMazeData[r - 1, c - 1, ROOM_SIGN_IDX] == 1)
                        check.Add(HexWallType.LeftDown);
                    else if (mMazeData[r - 1, c - 1, ROOM_SIGN_IDX] == 0)
                    {
                        mVisitedRooms.Add(new RoomCoordinate(r - 1, c - 1));
                        mMazeData[r - 1, c - 1, ROOM_SIGN_IDX] = 2;
                    }
                }

                if (c < mColCount - 1 && r > 0) //右下
                {
                    if (mMazeData[r - 1, c + 1, ROOM_SIGN_IDX] == 1)
                        check.Add(HexWallType.RightDown);
                    else if (mMazeData[r - 1, c + 1, ROOM_SIGN_IDX] == 0)
                    {
                        mVisitedRooms.Add(new RoomCoordinate(r - 1, c + 1));
                        mMazeData[r - 1, c + 1, ROOM_SIGN_IDX] = 2;
                    }
                }

            }
            else //奇数列
            {
                if (c > 0 && r < mRowCount - 1) //左上房间
                {
                    if (mMazeData[r + 1, c - 1, ROOM_SIGN_IDX] == 1)
                        check.Add(HexWallType.LeftUp);
                    else if (mMazeData[r + 1, c - 1, ROOM_SIGN_IDX] == 0)
                    {
                        mVisitedRooms.Add(new RoomCoordinate(r + 1, c - 1));
                        mMazeData[r + 1, c - 1, ROOM_SIGN_IDX] = 2;
                    }
                }

                if (c < mColCount - 1 && r < mRowCount - 1) //右上房间
                {
                    if (mMazeData[r + 1, c + 1, ROOM_SIGN_IDX] == 1)
                        check.Add(HexWallType.RightUp);
                    else if (mMazeData[r + 1, c + 1, ROOM_SIGN_IDX] == 0)
                    {
                        mVisitedRooms.Add(new RoomCoordinate(r + 1, c + 1));
                        mMazeData[r + 1, c + 1, ROOM_SIGN_IDX] = 2;
                    }
                }

                if (c > 0) //左下
                {
                    if (mMazeData[r, c - 1, ROOM_SIGN_IDX] == 1)
                        check.Add(HexWallType.LeftDown);
                    else if (mMazeData[r, c - 1, ROOM_SIGN_IDX] == 0)
                    {
                        mVisitedRooms.Add(new RoomCoordinate(r, c - 1));
                        mMazeData[r, c - 1, ROOM_SIGN_IDX] = 2;
                    }
                }

                if (c < mColCount - 1) //右下
                {
                    if (mMazeData[r, c + 1, ROOM_SIGN_IDX] == 1)
                        check.Add(HexWallType.RightDown);
                    else if (mMazeData[r, c + 1, ROOM_SIGN_IDX] == 0)
                    {
                        mVisitedRooms.Add(new RoomCoordinate(r, c + 1));
                        mMazeData[r, c + 1, ROOM_SIGN_IDX] = 2;
                    }
                }
            }

            //随机挑出一个墙打通其两边的房间
            if (check.Count > 0)
            {
                HexWallType move_dir = check[Random.Range(0, check.Count)];
                if (c % 2 == 0) //偶数列
                {
                    if (move_dir == HexWallType.LeftUp)
                    {
                        mMazeData[r, c, (int)HexWallType.LeftUp] = 1;
                        mMazeData[r, c - 1, (int)HexWallType.RightDown] = 1;
                    }
                    else if (move_dir == HexWallType.RightUp)
                    {
                        mMazeData[r, c, (int)HexWallType.RightUp] = 1;
                        mMazeData[r, c + 1, (int)HexWallType.LeftDown] = 1;
                    }
                    else if (move_dir == HexWallType.LeftDown)
                    {
                        mMazeData[r, c, (int)HexWallType.LeftDown] = 1;
                        mMazeData[r - 1, c - 1, (int)HexWallType.RightUp] = 1;
                    }
                    else if (move_dir == HexWallType.RightDown)
                    {
                        mMazeData[r, c, (int)HexWallType.RightDown] = 1;
                        mMazeData[r - 1, c + 1, (int)HexWallType.LeftUp] = 1;
                    }
                }
                else //奇数列
                {
                    if (move_dir == HexWallType.LeftUp)
                    {
                        mMazeData[r, c, (int)HexWallType.LeftUp] = 1;
                        mMazeData[r + 1, c - 1, (int)HexWallType.RightDown] = 1;
                    }
                    else if (move_dir == HexWallType.RightUp)
                    {
                        mMazeData[r, c, (int)HexWallType.RightUp] = 1;
                        mMazeData[r + 1, c + 1, (int)HexWallType.LeftDown] = 1;
                    }
                    else if (move_dir == HexWallType.LeftDown)
                    {
                        mMazeData[r, c, (int)HexWallType.LeftDown] = 1;
                        mMazeData[r, c - 1, (int)HexWallType.RightUp] = 1;
                    }
                    else if (move_dir == HexWallType.RightDown)
                    {
                        mMazeData[r, c, (int)HexWallType.RightDown] = 1;
                        mMazeData[r, c + 1, (int)HexWallType.LeftUp] = 1;
                    }
                }

                if (move_dir == HexWallType.Up)
                {
                    mMazeData[r, c, (int)HexWallType.Up] = 1;
                    mMazeData[r + 1, c, (int)HexWallType.Down] = 1;
                }
                else if (move_dir == HexWallType.Down)
                {
                    mMazeData[r, c, (int)HexWallType.Down] = 1;
                    mMazeData[r - 1, c, (int)HexWallType.Up] = 1;
                }
            }

            check.Clear();
        }
    }

    public override void GenMazeScene()
    {
        for (int r = 0; r < mRowCount; r++)
        {
            for (int c = 0; c < mColCount; c++)
            {
                GameObject o = Instantiate<GameObject>(mMazePrefab);

                if (c % 2 == 0)
                    o.transform.position = new Vector3(c / 2 * 9, 0, r * 6 * 0.866f);
                else
                    o.transform.position = new Vector3(c / 2 * 9 + 4.5f, 0, r * 6 * 0.866f + 3 * 0.866f);

                if (mMazeData[r, c, (int)HexWallType.LeftUp] == 1)
                {
                    o.transform.Find("LeftUp").gameObject.SetActive(false);
                }

                if (mMazeData[r, c, (int)HexWallType.RightUp] == 1)
                {
                    o.transform.Find("RightUp").gameObject.SetActive(false);
                }

                if (mMazeData[r, c, (int)HexWallType.LeftDown] == 1)
                {
                    o.transform.Find("LeftDown").gameObject.SetActive(false);
                }

                if (mMazeData[r, c, (int)HexWallType.RightDown] == 1)
                {
                    o.transform.Find("RightDown").gameObject.SetActive(false);
                }

                if (mMazeData[r, c, (int)HexWallType.Up] == 1)
                {
                    o.transform.Find("Up").gameObject.SetActive(false);
                }

                if (mMazeData[r, c, (int)HexWallType.Down] == 1)
                {
                    o.transform.Find("Down").gameObject.SetActive(false);
                }
            }
        }
    }
}
