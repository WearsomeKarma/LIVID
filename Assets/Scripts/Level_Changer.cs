
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Changer : MonoBehaviour
{
    [SerializeField]
    private string main_Menu_Scene = "MAIN_MenuScene";
    [SerializeField]
    private string main_World_Scene = "MAIN_WorldScene";
    [SerializeField]
    private string final_Dungeon_Scene = "Final_Dungeon";

    public void Main_Menu_Scene()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(main_Menu_Scene);
    }

    public void Main_World_Scene()
    {
        SceneManager.LoadScene(main_World_Scene);
    }

    public void Final_Dungeon_Scene()
    {
        SceneManager.LoadScene(final_Dungeon_Scene);
    }

    public void Exit_Game()
    {
        Application.Quit();
    }
}
