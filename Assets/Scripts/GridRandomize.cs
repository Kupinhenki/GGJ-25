using UnityEngine;

public class GridRandomize : MonoBehaviour
{
    private int random;
    public GameObject grid;
    public GameObject grid2;
    public GameObject grid3;

    void Awake()
    {
        grid.SetActive(false);
        grid2.SetActive(false);
        grid3.SetActive(false);

        random = Random.Range(1, 4);

        switch (random)
        {
            case 1: grid.SetActive(true); break;
                case 2: grid2.SetActive(true); break;
                case 3: grid3.SetActive(true); 
                break;
        }
    }
}
