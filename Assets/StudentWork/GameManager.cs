using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private PlayerController m_playerController;
	private PlayerController m_playerRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        m_playerRef = Instantiate(m_playerController);

		m_playerRef.Init();
    }

  
}
