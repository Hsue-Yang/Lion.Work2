var _formElement;

function AppUserMobileForm_onLoad(formElement) {
    _formElement = formElement;
}

function SaveButton_onClick(srcElement) {
    var isMasterDevice = $('input[name^=IsMasterCheckList]:checked');
    if (isMasterDevice.length > 0) {
        $('#ExecAction').val(_ActionTypeUpdate);
        return true;
    } else {
        _AddJsErrMessage(JsMsg_AppMasterDeviceChoose);
        _ShowJsErrMessageBox();
        return false;
    }
}