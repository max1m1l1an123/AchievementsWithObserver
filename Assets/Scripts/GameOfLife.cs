using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public GameObject TileObject;
    private static readonly int Width = 10;
    private static readonly int Height = 10;
    bool[,] grid = new bool[Width, Height];
    GameObject[,] tiles = new GameObject[Width, Height];

    private float TimeAccu = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Generate tiles
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // Clear the grid
                grid[x, y] = false;
                // Instantiate the tile
                tiles[x, y] = Instantiate(TileObject,
                                        new Vector3(x, 0f, y) * 1.05f,
                                        TileObject.transform.rotation);
                //tiles[x, y].SetActive(false);
                tiles[x, y].GetComponent<MeshRenderer>().material.color = Color.black;
            }
        }

        grid[5, 5] = true;
        tiles[5, 5].GetComponent<MeshRenderer>().material.color = Color.red;
        grid[3, 5] = true;
        tiles[3, 5].GetComponent<MeshRenderer>().material.color = Color.red;
        grid[4, 4] = true;
        tiles[4, 4].GetComponent<MeshRenderer>().material.color = Color.red;
        grid[1, 4] = true;
        tiles[1, 4].GetComponent<MeshRenderer>().material.color = Color.red;
        grid[2, 3] = true;
        tiles[2, 3].GetComponent<MeshRenderer>().material.color = Color.red;
        grid[1, 3] = true;
        tiles[1, 3].GetComponent<MeshRenderer>().material.color = Color.red;
        grid[2, 2] = true;
        tiles[2, 2].GetComponent<MeshRenderer>().material.color = Color.red;



    }

    private int GetLiveNeighbours(int x, int y)
    {
        int liveneighbours = 0;
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (!(i == x & j == y) && i >= 0 && j >= 0 && i < Width && j < Height)
                {
                    // current i,j is not x,y
                    if (grid[i, j] == true)
                    {
                        liveneighbours++;
                    }
                }
            }
        }
        return liveneighbours;
    }

    // Update is called once per frame
    void Update()
    {
        TimeAccu += Time.deltaTime;

        if (TimeAccu > 2.0f)
        {
            // RULES:
            // 1. Fewer than 2 live neighbours --> die
            // 2. 2 or 3 live neighbours -> live on
            // 3. more than 3 live neighbours -> die
            // 4. dead cell with 3 live neighbours -> rebirth 
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int live = GetLiveNeighbours(x, y);
                    if (live < 2)
                    {
                        grid[x, y] = false;
                    }
                    else if (live < 4 && grid[x, y] == true)
                    {
                        // live on... do nothing
                    }
                    else if (live > 3 && grid[x, y] == true)
                    {
                        grid[x, y] = false;
                    }
                    else if (live == 3 && grid[x, y] == false)
                    {
                        grid[x, y] = true;
                    }
                }
            }

            // Update the tiles
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (grid[x, y] == true)
                    {
                        tiles[x, y].GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    else
                    {
                        tiles[x, y].GetComponent<MeshRenderer>().material.color = Color.black;
                    }
                }
            }

            TimeAccu = 0.0f;
        }
    }
}
