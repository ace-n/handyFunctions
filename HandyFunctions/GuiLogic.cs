using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HandyFunctions
{
    class GuiLogic
    {
        public static void SetupGuiControlNetwork(Control control, Delegate controlValidator, Delegate doOnValidInputReceived, Delegate doOnInvalidInputReceived)
        {
            string type = control.GetType().ToString().ToLowerInvariant();

            // VERIFY THESE TYPE VALUES!!!

            if (type == "textbox")
            {
                control.TextChanged += new System.EventHandler(controlValidator);
                // this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            }
        }
    }
}
