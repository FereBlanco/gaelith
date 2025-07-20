using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

using Scripts.Tools;

public class FloorManager : MonoBehaviour
{
    [SerializeField] GameObject tileModel;
    [SerializeField] GameObject limitModel;
    [SerializeField] int rows = 8;
    [SerializeField] int cols = 8;

    private void Awake()
    {
        Assert.IsNotNull(tileModel, "ERROR: tileModel is null");
        Assert.IsNotNull(tileModel, "ERROR: limitModel is null");

        for (int i = 0; i <= rows; i++)
        {
            for (int j = 0; j <= cols; j++)
            {
                if (i == 0 || i == rows || j == 0 || j == cols)
                {
                    GameObject newTile = Instantiate(limitModel, transform);
                    newTile.name = CustomStringTools.CreateString("Limit_", i.ToString(), "_", j.ToString());
                    newTile.transform.Translate(i, 0f, j);
                }
                else
                {
                    GameObject newTile = Instantiate(tileModel, transform);
                    newTile.name = CustomStringTools.CreateString("Tile_", i.ToString(), "_", j.ToString());
                    newTile.transform.Translate(i, 0f, j);
                    newTile.GetComponent<FloorTileStyle>().SetMaterial((i + j) % 2);
                }
            }
        }
    }
}
