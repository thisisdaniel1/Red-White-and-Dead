using UnityEngine;
using UnityEngine.UI;

public class SquadIconUI : MonoBehaviour
{
    public GameObject squadIconTemplate;
    public bool needsUpdate = false;

    private static SquadIconUI _instance;
    public static SquadIconUI Instance { get { return _instance; } }

    void Awake(){
        // if an instance of this already exists and it isn't this one
        if (_instance != null && _instance != this){
            Destroy(this.gameObject);
        }
        else{
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (needsUpdate){
            RefreshSquadIcons();
            needsUpdate = false;
        }
    }

    // Method is called whenever a unit is added/created
    public void RequestUpdate(){
        needsUpdate = true;
    }

    void RefreshSquadIcons(){
        ClearSquadIcons();
        AddSquadIcons();
    }

    void ClearSquadIcons(){
        foreach (Transform child in transform){
            Destroy(child.gameObject);
        }
    }

    public void AddSquadIcons(){
        int startingXPosition = -210;
        int xOffset = 30;

        for (int i = 0; i < UnitSelections.Instance.unitList.Count; i++){
            Debug.Log("creating icon" + i);

            Unit unit = UnitSelections.Instance.unitList[i].GetComponent<Unit>();
            var icon = Instantiate(squadIconTemplate, transform);
            icon.GetComponent<Image>().sprite = unit.iconSprite;
            //icon.GetComponent<Image>().sprite = unit.number;

            int newXPosition = startingXPosition + (xOffset * i);

            icon.transform.localPosition = new Vector3(newXPosition, icon.transform.localPosition.y, icon.transform.localPosition.z);
        }
    }
}
