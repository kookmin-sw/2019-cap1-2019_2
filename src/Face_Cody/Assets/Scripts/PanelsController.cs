using System.Collections.Generic;
using UnityEngine;

public class PanelsController : MonoBehaviour
{
    private List<GameObject> panels;
    private int activePanel;
    private GameObject imageViewerController;
    private GameObject synthesisController;

    // Start is called before the first frame update
    void Start()
    {
        imageViewerController = GameObject.Find("ImageViewerController");
        synthesisController = GameObject.Find("SynthesisController");
        InitializePanelsList();
        InitializeCurrentPanel();
    }

    void InitializePanelsList()
    {
        panels = new List<GameObject>(new GameObject[] {GameObject.Find("SynthesisPanel"),
                                                        GameObject.Find("WaitPanel"),
                                                        GameObject.Find("SavePanel")});
    }

    void InitializeCurrentPanel()
    {
        activePanel = 0;
        panels[0].SetActive(true);
        for(int i=1; i<panels.Count; i++)
        {
            panels[i].SetActive(false);
        }
        imageViewerController.GetComponent<ImageViewerController>().UpdateImageViewer(activePanel);
    }

    public void ChangeActivePanel(int index)
    {
        panels[activePanel].SetActive(false);
        activePanel = index;
        panels[activePanel].SetActive(true);
        imageViewerController.GetComponent<ImageViewerController>().UpdateImageViewer(activePanel);
        synthesisController.GetComponent<SynthesisController>().UpdateNextButton(activePanel);
    }

    public int GetActivePanel()
    {
        return activePanel;
    }
}
