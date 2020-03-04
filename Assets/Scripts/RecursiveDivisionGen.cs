using UnityEngine;

public class RecursiveDivisionGen : MazeGenerator
{
    void Start()
    {
        if (mRowCount <= 0 || mColCount <= 0)
            return;

        //初始化房间数据，默认值为0表示封闭的墙
        mMazeData = new int[mRowCount, mColCount, (int)RectWallType.WallNum];

        mMazePrefab = Resources.Load<GameObject>("RectCell");

        GenMazeData();

        GenMazeScene();
    }

    /// <summary>
    /// 递归分割函数
    /// </summary>
    /// <param name="r1">起始行号</param>
    /// <param name="r2">结束行号</param>
    /// <param name="c1">起始列号</param>
    /// <param name="c2">结束列号</param>
    private void RecursiveDiv(int r1, int r2, int c1, int c2)
    {
        if (r1 < r2 && c1 < c2)
        {
            int rm = Random.Range(r1, r2 - 1); //取两中间的随机，不包括两端
            int cm = Random.Range(c1, c2 - 1);

            int cd1 = Random.Range(c1, cm + 1);
            int cd2 = Random.Range(cm + 1, c2);
            int rd1 = Random.Range(r1, rm + 1);
            int rd2 = Random.Range(rm + 1, r2);

            int d = Random.Range(0, 4); //随机4个象限

            if (d == 0)
            {
                mMazeData[rd2, cm, (int)RectWallType.Right] = 1;
                mMazeData[rd2, cm + 1, (int)RectWallType.Left] = 1;
                mMazeData[rm, cd1, (int)RectWallType.Up] = 1;
                mMazeData[rm + 1, cd1, (int)RectWallType.Down] = 1;
                mMazeData[rm, cd2, (int)RectWallType.Up] = 1;
                mMazeData[rm + 1, cd2, (int)RectWallType.Down] = 1;
            }
            else if (d == 1)
            {
                mMazeData[rd1, cm, (int)RectWallType.Right] = 1;
                mMazeData[rd1, cm + 1, (int)RectWallType.Left] = 1;
                mMazeData[rm, cd1, (int)RectWallType.Up] = 1;
                mMazeData[rm + 1, cd1, (int)RectWallType.Down] = 1;
                mMazeData[rm, cd2, (int)RectWallType.Up] = 1;
                mMazeData[rm + 1, cd2, (int)RectWallType.Down] = 1;
            }
            else if (d == 2)
            {
                mMazeData[rd1, cm, (int)RectWallType.Right] = 1;
                mMazeData[rd1, cm + 1, (int)RectWallType.Left] = 1;
                mMazeData[rd2, cm, (int)RectWallType.Right] = 1;
                mMazeData[rd2, cm + 1, (int)RectWallType.Left] = 1;
                mMazeData[rm, cd2, (int)RectWallType.Up] = 1;
                mMazeData[rm + 1, cd2, (int)RectWallType.Down] = 1;
            }
            else if (d == 3)
            {
                mMazeData[rd1, cm, (int)RectWallType.Right] = 1;
                mMazeData[rd1, cm + 1, (int)RectWallType.Left] = 1;
                mMazeData[rd2, cm, (int)RectWallType.Right] = 1;
                mMazeData[rd2, cm + 1, (int)RectWallType.Left] = 1;
                mMazeData[rm, cd1, (int)RectWallType.Up] = 1;
                mMazeData[rm + 1, cd1, (int)RectWallType.Down] = 1;
            }

            RecursiveDiv(r1, rm, c1, cm);
            RecursiveDiv(r1, rm, cm + 1, c2);
            RecursiveDiv(rm + 1, r2, cm + 1, c2);
            RecursiveDiv(rm + 1, r2, c1, cm);
        }
        else if (r1 < r2) // c1 == c2
        {
            int rm = Random.Range(r1, r2);

            mMazeData[rm, c1, (int)RectWallType.Up] = 1;
            mMazeData[rm + 1, c1, (int)RectWallType.Down] = 1;

            RecursiveDiv(r1, rm, c1, c1);
            RecursiveDiv(rm + 1, r2, c1, c1);
        }
        else if (c1 < c2) // r1 == r2
        {
            int cm = Random.Range(c1, c2);

            mMazeData[r1, cm, (int)RectWallType.Right] = 1;
            mMazeData[r1, cm + 1, (int)RectWallType.Left] = 1;

            RecursiveDiv(r1, r1, c1, cm);
            RecursiveDiv(r1, r1, cm + 1, c2);
        }
        else //只剩一个房间
        {

        }
    }

    public override void GenMazeData()
    {
        int r1 = 0;
        int r2 = mRowCount - 1;
        int c1 = 0;
        int c2 = mColCount - 1;

        RecursiveDiv(r1, r2, c1, c2);
    }
}
