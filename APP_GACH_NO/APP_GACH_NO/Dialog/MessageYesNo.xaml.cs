﻿using APP_GACH_NO.Global;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_GACH_NO.Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageYesNo : PopupPage
    {

        TaskCompletionSource<DialogReturn> _tsk = null;

        public MessageYesNo(string noidung)
        {
            InitializeComponent();
            lblMessage.Text = noidung;
        }

        private async void btnOK_Clicked(object sender, EventArgs e)
        {

            await Navigation.PopPopupAsync(true);
            _tsk.SetResult(DialogReturn.OK);
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync(true);
            _tsk.SetResult(DialogReturn.Cancel);
        }
        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }

    }
}