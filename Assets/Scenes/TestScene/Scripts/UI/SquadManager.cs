using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadManager : MonoBehaviour
{
    public static SquadManager instance; // Global instance of SquadManager

    public List<SquadData> squads = new List<SquadData>();

    // Selected squad
    public SquadData selectedSquad;

    void Awake()
    {
        instance = this;
    }

    // Method to select a squad
    public void SelectSquad(SquadData squad)
    {
        selectedSquad = squad;
        // Update UI or perform other actions as needed
    }

    // Method to issue squad-wide orders
    public void IssueOrders(Vector3 destination)
    {
        foreach (var unit in selectedSquad.units)
        {
            //unit.MoveTo(destination);
            // Implement other squad-wide orders here
        }
    }
}
