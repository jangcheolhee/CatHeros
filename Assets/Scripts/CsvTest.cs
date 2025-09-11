using UnityEngine;

public class CsvTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var table = new CharacterTable();
        table.Load("Character");
    }

    // Update is called once per frame
  
}
