using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionScene : GenericWindow
{

    public CharacterInfoPanel characterInfoPanel;
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
