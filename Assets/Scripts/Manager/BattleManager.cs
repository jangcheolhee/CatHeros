using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

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
    public Transform enemyBack;
    private int enemyCount;
    public int PlayerCount {  get; private set; }   

    public Dictionary<FormationRow,List<Player>> Players { get; private set; } = new ();

    public int currentWave = 0;
    private int totalWave;
    public Dictionary<FormationRow, List<Enemy>> AliveEnemies { get; private set; } = new Dictionary<FormationRow, List<Enemy>>();
    public List<WaveData> Waves { get; private set; }
    private bool spawningWave = false;

    public BattleUIManager battleUIManager;
    public UIManager uiManager;
    public bool IsAuto { get; private set; }
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
        else
        {
            uiManager.ShowPanel("DefeatPanel", true);
        }

        
        if (spawningWave || enemyCount > 0) return;

        if (enemyCount == 0)
        {
            if (currentWave >= totalWave)
            {
                Debug.Log("전투 승리!");
                uiManager.ShowPanel("VictoryPanel", true);
                return;
            }
            if (PlayerCount == 0)
            {
                Debug.Log("전투 패배...");
                uiManager?.ShowPanel("DefeatPanel", true);
                return;
            }
            StartCoroutine(SpawnWaveWithDelay(currentWave, Waves[currentWave].Spawn_Delay * 0.001f));
        }

    }


    private void SpawnParty()
    {
        PlayerCount = GameManager.Instance.PartySlots.Count;
        foreach (var slot in GameManager.Instance.PartySlots)
        {
            Vector3 pos = GetSlotPosition(slot.row, slot.index);
            GameObject obj = Instantiate(PlayerPrefab, pos, Quaternion.identity);

            Player player = obj.GetComponent<Player>();
            player.Setup(slot.characterId);
            player.battleManager = this;
            if (!Players.ContainsKey(slot.row))
            {
                Players[slot.row] = new List<Player>();
            }
            
            player.OnDeath += () => Players[slot.row].Remove(player);
            player.OnDeath += () => PlayerCount--;
            player.OnDeath += () => Destroy(player.gameObject, 1);

            Players[slot.row].Add(player);
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
            enemyCount= waveq.Enemies.Count;
            foreach (var enemys in waveq.Enemies)
            {
                Transform slot;
                GameObject obj;
                FormationRow row;
                if (enemys.Position == "Front")
                {
                    slot = enemyFront.GetChild(f++);
                    obj = Instantiate(Enemy, slot.position, Quaternion.identity, enemyFront);
                    row = FormationRow.Front;

                }
                else
                {
                    slot = enemyBack.GetChild(b++);
                    obj = Instantiate(Enemy, slot.position, Quaternion.identity, enemyBack);
                    row = FormationRow.Rear;
                }
                if(!AliveEnemies.ContainsKey(row))
                {
                    AliveEnemies[row] = new List<Enemy>();
                }
                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.Setup(enemys.Monster_ID);
                AliveEnemies[row].Add(enemy);
                enemy.battleManager = this;
                enemy.OnDeath += () => AliveEnemies[row].Remove(enemy);
                enemy.OnDeath += () => enemyCount--;
                enemy.OnDeath += () => Destroy(enemy.gameObject, 1);
            }

            currentWave++;
        }

    }
    public void OnAuto()
    {
        IsAuto = !IsAuto;
    }


}

