using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameController : MonoBehaviour
{
    [Tooltip("Tile basico a ser spawnado")]
    public Transform tile;

    [Tooltip("obstáculo do tipo cubo")]
    public Transform obstacleCube;

    [Tooltip("obstáculo do tipo cilindrico")]
    public Transform obstacleCylinder;

    [Tooltip("obstáculo do tipo esfera")]
    public Transform obstacleSphere;

    [Tooltip("ponto inicial")]
    public Vector3 initialPoint = new Vector3(0, 0, -5);

    [Tooltip("quantidade de spawn inicial")]
    [Range (1,20)]
    public int numSpawIni = 1;

    [Tooltip("quantidade de tiles iniciais sem obstáculo")]
    [Range(1,4)]
    public int numTileWithoutObstacle = 4;

    private Vector3 proxTilePos;
    private Quaternion proxTileRot;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize("2586165");

        proxTilePos = initialPoint;
        proxTileRot = Quaternion.identity;

        for (int i = 0; i < numSpawIni; i++)
        {
            SpawProxTile(i >= numTileWithoutObstacle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// metodo para criar os proximos obstáculos
    /// </summary>
    /// <param name="spawObstacles">colocar ou nao obstáculo naquele tile basic</param>
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
            var spawPoint2 = obstaclesPoints[Random.Range(0, obstaclesPoints.Count)];

            //guarda posição desse ponto
            var obstacleSpawPostion = spawPoint.transform.position;
            var obstacleSpawPostion2 = spawPoint2.transform.position;
            obstacleSpawPostion.y = 1;

            Transform obstacle = getRandomObstacle();
            Transform obstacle2 = getRandomObstacle();

            var newObstacle = Instantiate(obstacle, obstacleSpawPostion, Quaternion.identity);
            var newObstacle2 = Instantiate(obstacle2, obstacleSpawPostion2, Quaternion.identity);

            //Faz ele ser filho do tile basico
            newObstacle.SetParent(spawPoint.transform);
            if (!obstacleSpawPostion.Equals(obstacleSpawPostion2))
            {
                newObstacle2.SetParent(spawPoint2.transform);
            }
            
        }
    }

    /// <summary>
    /// Cria um obstáculo randomico
    /// </summary>
    /// <returns></returns>
    private Transform getRandomObstacle()
    {
        //Cria um novo obstaculo
        switch (Random.Range(0, 3))
        {
            case 0:
                return obstacleCube;
            case 1:
                return obstacleCylinder;
            case 2:
                return obstacleSphere;
            default:
                return obstacleCube;
        }
    }
}
