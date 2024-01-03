using UnityEngine;

public class UnitDrag : MonoBehaviour
{
    public Camera cam;

    [SerializeField] RectTransform boxVisual;

    Rect selectionBox;

    Vector2 startPosition;
    Vector2 endPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        // "redraws" the box with zero everything, so has it disappear
        DrawVisual();
    }

    // Update is called once per frame
    void Update()
    {
        // player drag select by left clicking first to set a start position
        // then holding left click to draw the box to complete the box
        // and finally release left click to reset it, box was done in previous step

        // when clicked
        if (Input.GetMouseButtonDown(0)){
            startPosition = Input.mousePosition;
            selectionBox = new Rect();
        }

        // when dragging
        if (Input.GetMouseButton(0)){
            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        // when release click
        if (Input.GetMouseButtonUp(0)){
            SelectUnits();
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            // "redraws" the box with zero everything
            DrawVisual();
        }
    }

    void DrawVisual(){
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd)/2;
        // locates the center of the new drawn box
        // notice how the center is a smaller box centered around the larger drawn box
        boxVisual.position = boxCenter;

        // boxSize represents the drawn box based on the x and y of the start and end positions
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection(){
        // a Rect uses a xMin and xMax, and a yMin and yMax
        if (Input.mousePosition.x < startPosition.x){
            // dragging left, mouse is on left side of screen
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else{
            // dragging right
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < startPosition.y){
            // dragging down
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else{
            // dragging up
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    void SelectUnits(){
        // loop through all units
        foreach(var unit in UnitSelections.Instance.unitList){
            // if unit is within the bounds of the selection rect
            // WorldToScreenPoint checks if a point is within something
            if (selectionBox.Contains(cam.WorldToScreenPoint(unit.transform.position))){
                // if any unit is within the selection add them to selection
                UnitSelections.Instance.DragSelect(unit);
            }
        }
    }
}
