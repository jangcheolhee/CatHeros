using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public float battleDuration = 210f;
    private float remainTime;

    public event Action<float> OnTimeChanged;

    public GameObject PlayerPrefab;
    public GameObject Enemy;

 
    public Transform playerFront;
    public Transform playerBack;// BattleWorld
    public Transform enemyFront;  // BattleWorld
    public Transform enemyBack ;
    
    
    public List<Player> Players { get; private set; } = new List<Player>();

    public int currentWave = 0;
    private int totalWave ;
    public List<Enemy> AliveEnemies { get; private set; } = new List<Enemy>();
    public List<WaveData> Waves { get; private set; }
    private bool spawningWave = false;

    public BattleUIManager battleUIManager;
    public UIManager uiManager;
    public bool IsAuto {  get; private set; }
    private void Start()
    {
        Time.timeScale = 1f;
        

        var stageData = DataTableManger.StageTable.Get(3801);
        totalWave = stageData.MaxWaveCount;
        Waves = DataTableManger.WaveTable.Get(stageData.StageID);
        remainTime = battleDuration;
        SpawnParty();
        
    }
    private void Update()
    {
        if (remainTime > 0)
        {
            remainTime -= Time.deltaTime;
            if (remainTime < 0) remainTime = 0;

            OnTimeChanged?.Invoke(remainTime);
        }
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

        foreach (var slot in GameManager.Instance.PartySlots)
        {
            Vector3 pos = GetSlotPosition(slot.row, slot.index);
            GameObject obj = Instantiate(PlayerPrefab, pos, Quaternion.identity);

            Player player = obj.GetComponent<Player>();
            player.Setup(slot.characterId);
            player.battleManager = this;
            player.OnDeath += () => Players.Remove(player);
            player.OnDeath += () => Destroy(player.gameObject, 1);

            Players.Add(player);
        }
    }
    private Vector3 GetSlotPosition(FormationRow row, int index)
    {
        if (row == FormationRow.Front)
            return playerFront.GetChild(index).position;
        else
            return playerBack.GetChild(index).position;
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
            int f = 0;
            int b = 0;
            foreach (var enemys in waveq.Enemies)
            {
                Transform slot;
                GameObject obj;
                if (enemys.Position == "Front")
                {
                    slot = enemyFront.GetChild(f++);
                    obj = Instantiate(Enemy, slot.position, Quaternion.identity, enemyFront);


                }
                else 
                {
                    slot = enemyBack.GetChild(b++);
                    obj = Instantiate(Enemy, slot.position, Quaternion.identity, enemyBack);
                }
                    
                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.Setup(enemys.Monster_ID);
                AliveEnemies.Add(enemy);
                enemy.battleManager = this;
                enemy.OnDeath += () => AliveEnemies.Remove(enemy);
                enemy.OnDeath += () => Destroy(enemy.gameObject,1);
            }

            currentWave++;
        }

    }
    public void OnAuto()
    {
        IsAuto = !IsAuto;
    }
    

}

