//2017/3/29
//by Chao
//控制重生点的范围，重生位置确定，重生敌人种类以及数量。
using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {
    public int maxEnemy;
    public int count=0;
    private float time=0;
    private float spawnColdTime=3f;
    private float spawmRandom;
    public GameObject[] enemy;
    private int variousOfEnemy;
    public EnemySpawnPoint[] spawnPoint;
    private int spCount;
    private Vector3[] spawnPos;
    public GameObject leftLimitAnchor;
    public GameObject rightLimitAnchor;
    public float leftLimit;
    public float rightLimit;
    public void EnemySpawn(Vector3 pos, Quaternion rot,int i)
    {

    }

    void Start () {
        variousOfEnemy = enemy.Length;//获取该重生点下所有子对象的数目
        leftLimit = leftLimitAnchor.transform.position.x;
        rightLimit = rightLimitAnchor.transform.position.x;
        count = 0;

        spCount = transform.childCount;
        spawnPoint = new EnemySpawnPoint[spCount];//初始化数组
        spawnPos = new Vector3[spCount];
        Debug.Log("SP Count"+spCount.ToString());
        for (int i = 0; i < spCount; i++)
        {
            spawnPoint[i] = transform.GetChild(i).GetComponent<EnemySpawnPoint>();
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;
        if (time > spawnColdTime)
        {
            spawmRandom = Random.Range(0, 100f);
            if (spawmRandom < 90f)//每X秒有10%几率重生
            {
                if (count < maxEnemy)
                {
                    int j=0;
                    int canSpawnNum = 0;
                    for (int i = 0; i < spCount; i++)//遍历所有出生点子对象找出能够重生的点，并在这些点中随机选择一个作为出生点
                    {   
                        if (spawnPoint[i].canSpawn)
                        {
                            spawnPos[j++] = spawnPoint[i].transform.position;
                            canSpawnNum++;
                            Debug.Log("SP" + spawnPoint[i].canSpawn.ToString());
                        }
                    }
                    if (canSpawnNum <= 0)//若无重生点能重生则Return
                        return;
                    int k = Random.Range(0, j);

                    int randomEnemy = Random.Range(0, variousOfEnemy);//随机重生该出生点所有种类敌人中的任意一个
                    GameObject newEnemy=Instantiate(enemy[randomEnemy], spawnPos[k] + new Vector3(0, enemy[randomEnemy].GetComponent<Enemy>().centerYPos, 0), Quaternion.identity) as GameObject;
                    newEnemy.GetComponent<Enemy>().spawnPoint=this;
                    Debug.Log("Enemy Spawned!");
                    count++;
                }

            }
            time = 0;
        }
	}
}
