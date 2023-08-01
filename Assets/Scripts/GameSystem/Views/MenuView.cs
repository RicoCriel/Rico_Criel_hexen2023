using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class MenuView : MonoBehaviour
{
    public event EventHandler PlayClicked;

    public void Play()
        => OnPlayClicked(EventArgs.Empty);

    private void OnPlayClicked(EventArgs eventArgs)
    {
        var handler = PlayClicked;
            handler?.Invoke(this, eventArgs);
    }

}
