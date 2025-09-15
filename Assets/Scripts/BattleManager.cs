
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    
    public GameObject Prefab;
    public GameObject Enemy;

 
    public Transform playerParent; // BattleWorld
    public Transform enemyParent;  // BattleWorld

    private string[] characterIds;
    public List<Player> Players { get; private set; } = new List<Player>();
    public int currentWave = 0;
    private int totalWave = 3;
    public List<Enemy> AliveEnemies { get; private set; } = new List<Enemy>();

    private bool spawningWave = false;

    public BattleUIManager battleUIManager;
    public UIManager uiManager;
    private void Start()
    {
        characterIds = new string[] { "10101", "10102", "10103", "10104" };
        
        SpawnParty();
        
    }
    private void Update()
    {
        if (spawningWave || AliveEnemies.Count > 0) return;

        if (AliveEnemies.Count == 0)
        {
            if (currentWave >= totalWave)
            {
                Debug.Log("전투 승리!");
                uiManager.ShowPanel("EndPanel", true);
                return;
            }
            if (Players.TrueForAll(p => p.IsDead))
            {
                Debug.Log("전투 패배...");
                uiManager?.ShowPanel("End", true);
                return;
            }
            StartCoroutine(SpawnWaveWithDelay(currentWave, 1f));
        }
    }


    private void SpawnParty()
    {
        Debug.Log(123);
        for (int i = 0; i < 4; i++)
        {
            Transform slot = playerParent.GetChild(i);
            Debug.Log(slot);
            GameObject obj = Instantiate(Prefab, slot.position, Quaternion.identity, playerParent.parent);
            Player player = obj.GetComponent<Player>();
            
            player.Setup(characterIds[i]);
            player.battleManager = this;
            Players.Add(player);
        }
    }
    private IEnumerator SpawnWaveWithDelay(int waveIndex, float delay)
    {
        spawningWave = true;
        Debug.Log($"웨이브 {waveIndex + 1} 준비 중...");

        yield return new WaitForSeconds(delay); // ? 1초 기다림

        SpawnWave(waveIndex);

        spawningWave = false;
    }
    private void SpawnWave(int wave)
    {
        if (currentWave < totalWave)
        {
            Debug.Log($"웨이브 {currentWave + 1} 시작!");
            battleUIManager.UpdateWaveText(currentWave + 1, totalWave);

            for (int i = 0; i < 3; i++)
            {
                Transform slot = enemyParent.GetChild(i);
                GameObject obj = Instantiate(Enemy, slot.position, Quaternion.identity, enemyParent);
                Enemy enemy = obj.GetComponent<Enemy>();
                AliveEnemies.Add(enemy);
                enemy.battleManager = this;
                enemy.OnDeath += () => AliveEnemies.Remove(enemy);
                enemy.OnDeath += () => Destroy(enemy.gameObject);
            }

            currentWave++;
        }

    }




}

