using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 4面墙的类型
/// </summary>
enum RectWallType
{
    Left = 0,
    Up = 1,
    Right = 2,
    Down = 3,
    WallNum = 4,
}

/// <summary>
/// 6面墙类型
/// </summary>
enum HexWallType
{
    Up = 0,         //正上方：不管奇偶c,r+1
    Down = 1,       //正下方：不管奇偶c,r-1
    LeftUp = 2,     //左上方：本列是奇数列c-1,r+1，本列是偶数列c-1,r
    RightUp = 3,    //右上方：本列是奇数列c+1,r+1，本列是偶数列c+1,r
    LeftDown = 4,   //左下方：本列是奇数列c-1,r，本列是偶数列c-1,r-1
    RightDown = 5,  //右下方：本列是奇数列c+1,r，本列是偶数列c+1,r-1
    WallNum = 6,
}

/// <summary>
/// 房间坐标类型
/// </summary>
public class RoomCoordinate
{
    public int row; //行
    public int col; //列

    public RoomCoordinate(int r, int c)
    {
        row = r;
        col = c;
    }
}

/// <summary>
/// 迷宫生成器父类
/// </summary>
public abstract class MazeGenerator : MonoBehaviour
{
    protected int[,,] mMazeData;          //迷宫数据

    protected GameObject mMazePrefab;

    public int mRowCount = 10;          //迷宫行个数

    public int mColCount = 10;          //迷宫列个数
    #region 图片填充
    //public const int ROOM_PIXEL_WIDTH = 10;    //房间像素宽度
    //public const int ROOM_PIXEL_HEIGHT = 10;   //房间像素高度
    //public const int WALL_PIXEL_WIDTH = 2;     //房间墙厚度    //protected RawImage mRawImage;       //绘制迷宫的Image
    //protected Texture2D mMazeTexture;   //迷宫纹理
    //protected Color[] mMazColors;       //迷宫纹理颜色值
    #endregion

    #region 图片填充
    //生成迷宫的纹理并显示出来
    //protected void GenMazeImage()
    //{
    //    int roomRowPixelCount = mColCount * ROOM_PIXEL_WIDTH;

    //    for (int r = 0; r < mRowCount; r++)
    //    {
    //        for (int c = 0; c < mColCount; c++)
    //        {
    //            //绘制房间为白色
    //            for (int i = r * ROOM_PIXEL_HEIGHT + WALL_PIXEL_WIDTH; i < (r + 1) * ROOM_PIXEL_HEIGHT - WALL_PIXEL_WIDTH; i++)
    //            {
    //                for (int j = c * ROOM_PIXEL_WIDTH + WALL_PIXEL_WIDTH; j < (c + 1) * ROOM_PIXEL_WIDTH - WALL_PIXEL_WIDTH; j++)
    //                {
    //                    int idx = j + i * roomRowPixelCount;
    //                    mMazColors[idx] = Color.white;
    //                }
    //            }

    //            //左墙
    //            if (mMazeData[r, c, (int)RectWallType.Left] == 1)
    //            {
    //                for (int i = r * ROOM_PIXEL_HEIGHT + WALL_PIXEL_WIDTH; i < (r + 1) * ROOM_PIXEL_HEIGHT - WALL_PIXEL_WIDTH; i++)
    //                {
    //                    for (int j = c * ROOM_PIXEL_WIDTH; j < c * ROOM_PIXEL_WIDTH + WALL_PIXEL_WIDTH; j++)
    //                    {
    //                        int idx = j + i * roomRowPixelCount;
    //                        mMazColors[idx] = Color.white;
    //                    }
    //                }
    //            }

    //            if (mMazeData[r, c, (int)RectWallType.Up] == 1)
    //            {
    //                for (int i = (r + 1) * ROOM_PIXEL_HEIGHT - WALL_PIXEL_WIDTH; i < (r + 1) * ROOM_PIXEL_HEIGHT; i++)
    //                {
    //                    for (int j = c * ROOM_PIXEL_WIDTH + WALL_PIXEL_WIDTH; j < (c + 1) * ROOM_PIXEL_WIDTH - WALL_PIXEL_WIDTH; j++)
    //                    {
    //                        int idx = j + i * roomRowPixelCount;
    //                        mMazColors[idx] = Color.white;
    //                    }
    //                }
    //            }

    //            //右墙
    //            if (mMazeData[r, c, (int)RectWallType.Right] == 1)
    //            {
    //                for (int i = r * ROOM_PIXEL_HEIGHT + WALL_PIXEL_WIDTH; i < (r + 1) * ROOM_PIXEL_HEIGHT - WALL_PIXEL_WIDTH; i++)
    //                {
    //                    for (int j = (c + 1) * ROOM_PIXEL_WIDTH - WALL_PIXEL_WIDTH; j < (c + 1) * ROOM_PIXEL_WIDTH; j++)
    //                    {
    //                        int idx = j + i * roomRowPixelCount;
    //                        mMazColors[idx] = Color.white;
    //                    }
    //                }
    //            }

    //            if (mMazeData[r, c, (int)RectWallType.Down] == 1)
    //            {
    //                for (int i = r * ROOM_PIXEL_HEIGHT; i < r * ROOM_PIXEL_HEIGHT + WALL_PIXEL_WIDTH; i++)
    //                {
    //                    for (int j = c * ROOM_PIXEL_WIDTH + WALL_PIXEL_WIDTH; j < (c + 1) * ROOM_PIXEL_WIDTH - WALL_PIXEL_WIDTH; j++)
    //                    {
    //                        int idx = j + i * roomRowPixelCount;
    //                        mMazColors[idx] = Color.white;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    mMazeTexture.SetPixels(mMazColors);

    //    mMazeTexture.Apply();
    //}
    #endregion

    public abstract void GenMazeData();

    /// <summary>
    /// 根据数据，生成实例化整个迷宫
    /// </summary>
    public virtual void GenMazeScene()
    {
        for (int r = 0; r < mRowCount; r++)
        {
            for (int c = 0; c < mColCount; c++)
            {
                GameObject o = Instantiate<GameObject>(mMazePrefab);
                o.transform.position = new Vector3(c * 4, 0, r * 4);

                if (mMazeData[r, c, (int)RectWallType.Left] == 1)
                {
                    o.transform.Find("Left").gameObject.SetActive(false);
                }

                if (mMazeData[r, c, (int)RectWallType.Right] == 1)
                {
                    o.transform.Find("Right").gameObject.SetActive(false);
                }

                if (mMazeData[r, c, (int)RectWallType.Up] == 1)
                {
                    o.transform.Find("Up").gameObject.SetActive(false);
                }

                if (mMazeData[r, c, (int)RectWallType.Down] == 1)
                {
                    o.transform.Find("Down").gameObject.SetActive(false);
                }
            }
        }
    }
}
