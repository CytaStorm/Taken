using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUIUpdater : MonoBehaviour
{
    public TMP_Text statText;
    private Flags flags;

    private void Start()
    {
        flags = Flags.Instance;
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateStats()
    {
        int strength = flags.Strength == null ? 0 : flags.Strength.Value;
        int magic = flags.Magic == null ? 0 : flags.Magic.Value;
        int intelligence = flags.Intelligence == null ? 0 : flags.Intelligence.Value;
        int faith = flags.Faith == null ? 0 : flags.Faith.Value;

        // Create and stylize stat text
        string textData = "";
        textData += $"STR: <b>{strength}</b>\n";
        textData += $"MAG: <b>{magic}</b>\n";
        textData += $"INT: <b>{intelligence}</b>\n";
        textData += $"FAI: <b>{faith}</b>";

        // Assign stat text
        statText.text = textData;
    }
}
