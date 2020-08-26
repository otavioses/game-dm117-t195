using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameController : MonoBehaviour
{
    public Transform tile;

    public Transform obstacle;

    public Vector3 initialPoint = new Vector3(0, 0, -5);

    [Range (1,20)]
    public int numSpawIni;

    [Range(1,4)]
    public int numTileWithoutObstacle = 4;

    private Vector3 proxTilePos;
    private Quaternion proxTileRot;

    // Start is called before the first frame update
    void Start()
    {
        //Advertisement.Initialize("2586169");
        proxTilePos = initialPoint;
        proxTileRot = Quaternion.identity;

        for(int i = 0; i < numSpawIni; i++)
        {
            SpawProxTile(i >= numTileWithoutObstacle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawProxTile(bool spawObstacles = true)
    {
        var newTile = Instantiate(tile, proxTilePos, proxTileRot);

        var proxTile = newTile.Find("SpawPoint");

        proxTilePos = proxTile.position;
        proxTileRot = proxTile.rotation;

        if (!spawObstacles)
        {
            return;
        }

        var obstaclesPoints = new List<GameObject>();

        foreach(Transform child in newTile)
        {
            if (child.CompareTag("ObsSpaw"))
            {
                obstaclesPoints.Add(child.gameObject);
            }
        }

        if (obstaclesPoints.Count > 0)
        {
            //Pega ponto aleatorio
            var spawPoint = obstaclesPoints[Random.Range(0, obstaclesPoints.Count)];

            //guarda posição desse ponto
            var obstacleSpawPostion = spawPoint.transform.position;

            //Cria um novo obstaculo
            var newObstacle = Instantiate(obstacle, obstacleSpawPostion, Quaternion.identity);

            //Faz ele ser filho do tile basico
            newObstacle.SetParent(spawPoint.transform);
        }
    }
}
