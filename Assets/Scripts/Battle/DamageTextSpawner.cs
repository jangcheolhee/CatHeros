using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    public static DamageTextSpawner Instance;

   
    public GameObject damageTextPrefab;
    public Canvas worldCanvas; 

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnDamageText(string damage, Vector3 worldPosition)
    {
        if (damageTextPrefab == null || worldCanvas == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        GameObject obj = Instantiate(damageTextPrefab, worldCanvas.transform);
        obj.transform.position = screenPos;

        DamageText dmgText = obj.GetComponent<DamageText>();
        dmgText.SetText(damage);
    }
}
