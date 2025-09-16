using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    
    public GameObject Prefab;
    public GameObject Enemy;

 
    public Transform playerParent; // BattleWorld
    public Transform enemyParent;  // BattleWorld

    private int[] characterIds;
    public List<Player> Players { get; private set; } = new List<Player>();
    public int currentWave = 0;
    private int totalWave ;
    public List<Enemy> AliveEnemies { get; private set; } = new List<Enemy>();
    public List<WaveData> Waves { get; private set; }
    private bool spawningWave = false;

    public BattleUIManager battleUIManager;
    public UIManager uiManager;
    private void Start()
    {
        
        characterIds = new int[] { 10101, 10102, 10103, 10104 };
        var stageData = DataTableManger.StageTable.Get(3801);
        totalWave = stageData.MaxWaveCount;
        Waves = DataTableManger.WaveTable.Get(stageData.StageID);
        
        
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
                uiManager?.ShowPanel("EndPanel", true);
                return;
            }
            StartCoroutine(SpawnWaveWithDelay(currentWave, Waves[currentWave].Spawn_Delay * 0.001f));
        }
    }


    private void SpawnParty()
    {
        
        for (int i = 0; i < 4; i++)
        {
            Transform slot = playerParent.GetChild(i);
            
            GameObject obj = Instantiate(Prefab, slot.position, Quaternion.identity, playerParent.parent);
            Player player = obj.GetComponent<Player>();
            
            player.Setup(characterIds[i]);
            player.battleManager = this;
            player.OnDeath += () => Players.Remove(player);
            Players.Add(player);
        }
    }
    private IEnumerator SpawnWaveWithDelay(int waveIndex, float delay)
    {
        spawningWave = true;
        Debug.Log($"웨이브 {waveIndex + 1} 준비 중...");

        yield return new WaitForSeconds(delay);

        SpawnWave(waveIndex);

        spawningWave = false;
    }
    private void SpawnWave(int wave)
    {
        if (currentWave < totalWave)
        {
            Debug.Log($"웨이브 {currentWave + 1} 시작!");
            battleUIManager.UpdateWaveText(currentWave + 1, totalWave);
            var waveq = Waves[wave];
            int i = 0;
            foreach (var enemys in waveq.Enemies)
            {
                Transform slot = enemyParent.GetChild(i++);
                GameObject obj = Instantiate(Enemy, slot.position, Quaternion.identity, enemyParent);
                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.Setup(enemys.Monster_ID);
                AliveEnemies.Add(enemy);
                enemy.battleManager = this;
                enemy.OnDeath += () => AliveEnemies.Remove(enemy);
                enemy.OnDeath += () => Destroy(enemy.gameObject);
            }

            currentWave++;
        }

    }




}

