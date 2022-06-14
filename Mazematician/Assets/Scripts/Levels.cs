using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public float holep;
    public int w, h, x, y;
    public bool[,] hwalls, vwalls;
    public Transform Level, Player, Goal;
    public GameObject Floor, Wall;

    void Start()
    {
        foreach (Transform child in Level)
            Destroy(child.gameObject);

        hwalls = new bool[w + 1, h];
        vwalls = new bool[w, h + 1];
        var st = new int[w, h];

        void dfs(int x, int y)
        {
            st[x, y] = 1;
            Instantiate(Floor, new Vector3(x, y), Quaternion.identity, Level);

            var dirs = new[]
            {
                (x - 1, y, hwalls, x, y, Vector3.right, 90, KeyCode.A),
                (x + 1, y, hwalls, x + 1, y, Vector3.right, 90, KeyCode.D),
                (x, y - 1, vwalls, x, y, Vector3.up, 0, KeyCode.S),
                (x, y + 1, vwalls, x, y + 1, Vector3.up, 0, KeyCode.W),
            };
            foreach (var (nx, ny, wall, wx, wy, sh, ang, k) in dirs.OrderBy(d => Random.value))
                if (!(0 <= nx && nx < w && 0 <= ny && ny < h) || (st[nx, ny] == 2 && Random.value > holep))
                {
                    wall[wx, wy] = true;
                    Instantiate(Wall, new Vector3(wx, wy) - sh / 2, Quaternion.Euler(0, 0, ang), Level);
                }
                else if (st[nx, ny] == 0) dfs(nx, ny);
            st[x, y] = 2;
        }
        dfs(0, 0);

        x = Random.Range(0, w);
        y = Random.Range(0, h);
        Player.position = new Vector3(x, y);
        do Goal.position = new Vector3(Random.Range(0, w), Random.Range(0, h));
        while (Vector3.Distance(Player.position, Goal.position) < (w + h) / 4);
    }

    void Update()
    {
        var dirs = new[]
        {
            (x - 1, y, hwalls, x, y, Vector3.right, 90, KeyCode.A),
            (x + 1, y, hwalls, x + 1, y, Vector3.right, 90, KeyCode.D),
            (x, y - 1, vwalls, x, y, Vector3.up, 0, KeyCode.S),
            (x, y + 1, vwalls, x, y + 1, Vector3.up, 0, KeyCode.W),
        };
        foreach (var (nx, ny, wall, wx, wy, sh, ang, k) in dirs.OrderBy(d => Random.value))
            if (Input.GetKeyDown(k))
                if (wall[wx, wy])
                    Player.position = Vector3.Lerp(Player.position, new Vector3(nx, ny), 0.1f);
                else (x, y) = (nx, ny);

        Player.position = Vector3.Lerp(Player.position, new Vector3(x, y), Time.deltaTime * 12);
        if (Vector3.Distance(Player.position, Goal.position) < 0.12f)
        {
            if (Random.Range(0, 5) < 3) w++;
            else h++;
            Start();
        }
    }
}