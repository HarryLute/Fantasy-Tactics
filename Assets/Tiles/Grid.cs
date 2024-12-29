using UnityEngine;

public class Grid : MonoBehaviour
{
     public int height;
     public int length;

    bool[,] grid;

    public void Init(int length, int height)
    {
        grid = new bool[length, height];
        this.length = length;
        this.height = height;
    }
}
