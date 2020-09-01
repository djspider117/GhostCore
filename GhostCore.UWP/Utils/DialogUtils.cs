using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace GhostCore.UWP.Utils
{
    public static class DialogUtils
    {
        public static UICommand DefaultAcceptCommand { get; private set; } = new UICommand("Accept");
        public static UICommand DefaultCancelCommand { get; private set; } = new UICommand("Cancel");

        public static async Task<IUICommand> ShowMessageBox(string content, string title)
        {
            var msgDlg = new MessageDialog(content, title);
            return await msgDlg.ShowAsync();
        }

        public static async Task<IUICommand> ShowMessageBox(string content, string title, UICommand acceptCommand, UICommand cancelCommand)
        {
            if (acceptCommand == null)
                throw new ArgumentNullException(nameof(acceptCommand));

            if (cancelCommand == null)
                throw new ArgumentNullException(nameof(cancelCommand));

            var msgDlg = new MessageDialog(content, title);
            msgDlg.Options = MessageDialogOptions.AcceptUserInputAfterDelay;
            msgDlg.Commands.Add(cancelCommand);
            msgDlg.Commands.Add(acceptCommand);

            msgDlg.DefaultCommandIndex = 1;
            msgDlg.CancelCommandIndex = 0;

            return await msgDlg.ShowAsync();
        }
    }
}
