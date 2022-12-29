using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButtonUI : MonoBehaviour
{
    [SerializeField]
    private Canvas popUpCanvas;
    [SerializeField]
    private PauseMenu pauseMenuPrefab;

    public void ShowPauseMenu()
    {
        

        PauseMenu pauseMenu = Instantiate(pauseMenuPrefab);
        pauseMenu.transform.SetParent(popUpCanvas.transform, false);
        //pauseMenu.transform.localPosition = Vector3.zero;
    }
}