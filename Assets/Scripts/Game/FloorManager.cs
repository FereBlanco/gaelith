using NUnit.Framework;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] GameObject tileModel;
    [SerializeField] GameObject limitModel;
    [SerializeField] int rows = 8;
    [SerializeField] int cols = 8;

    private void Awake() {
        Assert.IsNotNull(tileModel, "ERROR: tileModel is null");
        Assert.IsNotNull(tileModel, "ERROR: limitModel is null");

        for (int i = 0; i <= rows; i++)
        {
            for (int j = 0; j <= cols; j++)
            {
                if (i == 0 || i == rows || j == 0 || j == cols)
                {
                    GameObject newTile = Instantiate(limitModel, transform);
                    newTile.name = $"Limit_{i.ToString("00")}_{j.ToString("00")}";
                    newTile.transform.Translate(i, 0f, j);
                }
                else
                {
                    GameObject newTile = Instantiate(tileModel, transform);
                    newTile.name = $"Tile_{i.ToString("00")}_{j.ToString("00")}";
                    newTile.transform.Translate(i, 0f, j);
                    newTile.GetComponent<FloorTileStyle>().SetMaterial((i+j) % 2);
                }
            }
        }

    }    
}
