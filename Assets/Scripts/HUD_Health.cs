
using UnityEngine;

public class HUD_Health : MonoBehaviour
{
    private Entity Player { get; set; }
    [SerializeField]
    private GameObject heart;

    public void Start()
    {
        Player = Entity_Manager.Entity_Manager__PLAYER_ENTITY;

        Player.Damaged += (e) => Set_Health(e);
        Player.Healed  += (e) => Set_Health(e);

        Set_Health(Player);
    }

    private void Set_Health(Entity player)
    {
        if (this == null)
        {
            enabled = false;
            return;
        }
        int hearts = gameObject.transform.childCount;

        if (player.Health < 0 || player.Health == hearts)
            return;

        Debug.Log(player.Health);

        if (player.Health > hearts)
        {
            for(int i=0;i<(player.Health - hearts);i++)
            {
                GameObject added_heart = GameObject.Instantiate(heart);
                added_heart.transform.SetParent(transform, false);
            }
            return;
        }

        for(int i=1;i<=(hearts - player.Health);i++)
        {
            Transform removed_heart = gameObject.transform.GetChild(hearts - i);

            GameObject.Destroy(removed_heart.gameObject);
        }
    }

    public void OnDestroy()
    {
        Debug.Log("DESTROYED");
        Player.Damaged -= Set_Health;
        Player.Healed -= Set_Health;
    }
} 
