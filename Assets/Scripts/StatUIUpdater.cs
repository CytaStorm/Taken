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
    }

    // Update is called once per frame
    void Update()
    {
        int strength = flags.Strength.Value;
        int magic = flags.Magic.Value;
        int intelligence = flags.Intelligence.Value;
        int faith = flags.Faith.Value;

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
