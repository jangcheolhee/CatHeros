
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject Prefab;
    private string[] characterIds;
    private List<Player> players = new List<Player>();
    private void Start()
    {
        characterIds = new string[] { "10101", "10102", "10103", "10104" };
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {
        for(int i = 0; i < characterIds.Length; i++)
        {
            Vector3 pos = new Vector3(-6 + i * 1.5f, 0, 0); // 파티 줄 세우기
            GameObject obj = Instantiate(Prefab, pos, Quaternion.identity);
            Player player = obj.GetComponentInChildren<Player>();

            
            player.Setup( characterIds[i]);
            players.Add(player);
        }
    }

    private void SpawnEnemy() 
    {
       string[] enemyIds = new string[5]; // 테이블 읽어오기
        
    }



}

