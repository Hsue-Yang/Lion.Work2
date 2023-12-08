var _formElement;

function SignatureDetailForm_onLoad(formElement) {
    _formElement = formElement;
    SigType_onChange($('#SigType')[0]);

    $('#ValidAPISysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: ValidAPISysID_onChange
    });

    $('#ValidAPIControllerID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: ValidAPIControllerID_onChange
    });

    $('#SigType, #SigStep, #ValidAPIActionName', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });
    
    return true;
}

function SigType_onChange(srcElement) {
    $('#SigSeqBox').addClass('hide');
    $('#APIFunBox').addClass('hide');
    $('#RoleBox').addClass('hide');
    switch (srcElement.value) {
        case 'C':
            $('#SigSeqBox').removeClass('hide');
            break;
        case 'A':
            $('#APIFunBox').removeClass('hide');
            break;
        case 'R':
            $('#RoleBox').removeClass('hide');
            break;
    }
    return false;
}

function APISysID_onChange(srcElement) {
    $('#APIControllerID > option', _formElement).remove();
    $('#APIActionName > option', _formElement).remove();

    if ($(srcElement).val() === '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIGroupList',
        type: 'POST',
        data: { SysID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#APIControllerID', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIGroupList);
            _ShowJsErrMessageBox();
        }
    });
}

function APIControllerID_onChange(srcElement) {
    $('#APIActionName > option', _formElement).remove();

    if ($(srcElement).val() == '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIFunList',
        type: 'POST',
        data: { SysID: $('#APISysID').val(), APIGroup: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#APIActionName', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIFunList);
            _ShowJsErrMessageBox();
        }
    });
}

function ValidAPISysID_onChange(event) {
    $('#ValidAPIControllerID > option', _formElement).remove();
    $('#ValidAPIActionName > option', _formElement).remove();
    $('#ValidAPIControllerID', _formElement).combobox('SetSelected', '');
    $('#ValidAPIActionName', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIGroupList',
        type: 'POST',
        data: { SysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#ValidAPIControllerID', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIGroupList);
            _ShowJsErrMessageBox();
        }
    });
}

function ValidAPIControllerID_onChange(event) {
    $('#ValidAPIActionName > option', _formElement).remove();
    $('#ValidAPIActionName', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIFunList',
        type: 'POST',
        data: { SysID: $('#ValidAPISysID').val(), APIGroup: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#ValidAPIActionName', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIFunList);
            _ShowJsErrMessageBox();
        }
    });
}

//----Button----//

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (result) {
        if (ValidObjectChecked()) {
            $.blockUI({ message: '' });
            _formElement.submit();
        }
        else {
            _ShowJsErrMessageBox();
        }
    }
}

function UpdateButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (result) {
        if (ValidObjectChecked()) {
            $.blockUI({ message: '' });
            _formElement.submit();
        } else {
            _ShowJsErrMessageBox();
        }
    }
}

function DeleteButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    _alert('dialog_Confirm');
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function ConfirmOKButton_onClick(srcElement) {
    var result = _FormValidation();

    $('#ExecAction').val(_ActionTypeDelete);
    if (result) {
        if (ValidObjectChecked()) {
            _formElement.submit();
        } else {
            _ShowJsErrMessageBox();
        }
    }

    _btnUnblockUI(this, false);
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function ValidObjectChecked() {
    return true;
}
