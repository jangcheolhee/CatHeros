using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionScene : GenericWindow
{

    public CharacterInfoPanel characterInfoPanel;
    public override void Open()
    {
        characterInfoPanel.gameObject.SetActive(false);
        base.Open();

    }
    public void OnClickHome()
    {
        manager.Open(Windows.Main);
    }
    public void OnClickStage()
    {
        manager.Open(Windows.Stage);
    }
    public void OnClickPanel(CharacterInfo character)
    {

        characterInfoPanel.SetCharacter(character);

    }

}
