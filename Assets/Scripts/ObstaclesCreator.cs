using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesCreator : MonoBehaviour
{

    public static ObstaclesCreator instance = null;

    [SerializeField] private GameObject[] ObsataclePrefabs;


    [SerializeField] private int ObstaclesXsize;
    [SerializeField] private int ObstaclesZsize;

    [SerializeField] private GameObject[,] ObjectsPool=new GameObject[41,20];



    //Local Variables
    Vector3 RandomPos=new Vector3(1,0,1);
    int k, l = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        CreateObstacles();
    }

    private void CreateObstacles()
    {
        for(int i = 0; i < ObstaclesXsize; i++)
        {
            for(int j = 0; j < ObstaclesZsize; j++)
            {
                GameObject Obj = Instantiate(ObsataclePrefabs[GetRandomNumber(0, 2)], new Vector3((j*10)+ GetRandomNumber(-2, 4), 0,(i*10)+ GetRandomNumber(-2, 4)) , Quaternion.identity);
                Obj.transform.localScale = new Vector3(3, GetRandomNumber(1, 3), 3);
                Obj.transform.parent = this.transform;
                ObjectsPool[i,j] = Obj;
            }
        }
    }


    public void MoveObsatclesNext()
    {
        Debug.Log("Looped");
        for(int i = k; i < k+1; i++)
        {
            for(int j = 0; j < 20; j++)
            {
                ObjectsPool[i, j].transform.position = ObjectsPool[i, j].transform.position+Vector3.forward*400;
            }
        }
        k++;
    }


    private int GetRandomNumber(int min,int max)
    {
        return Random.Range(min,max);
    }
}
