using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StartView : MonoBehaviour
{
    public event EventHandler PlayClicked;
    public GameObject Button;

    public void Play()
        => OnPlayClicked(EventArgs.Empty);

    private void OnPlayClicked(EventArgs eventArgs)
    {
        var handler = PlayClicked;
        handler?.Invoke(this, eventArgs);
    }
}

