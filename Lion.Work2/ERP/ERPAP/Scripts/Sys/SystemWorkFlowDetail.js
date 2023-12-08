var _formElement;

function SystemWorkFlowDetailForm_onLoad(formElement) {
    _formElement = formElement;

    $('select[name=WFFlowGroupID], select[name=FlowType]', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

    $('select[name=MsgSysID]', _formElement).combobox({
        width: 200,
        isRemoveButton: true,
        onChange: MsgSysID_onChange
    });

    $('select[name=MsgControllerID]', _formElement).combobox({
        width: 200,
        isRemoveButton: true,
        onChange: MsgControllerID_onChange
    });

    $('select[name=MsgActionName]', _formElement).combobox({
        width: 200,
        isRemoveButton: true
    });

    return true;
}

function FlowManUserID_onBlur(srcElement) {
    srcElement.value = srcElement.value.toUpperCase();
}


function MsgSysID_onChange(event) {
    $('#MsgControllerID > option', _formElement).remove();
    $('#MsgActionName > option', _formElement).remove();
    $('#MsgControllerID', _formElement).combobox('SetSelected', '');
    $('#MsgActionName', _formElement).combobox('SetSelected', '');

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
                    $('#MsgControllerID', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIGroups);
            _ShowJsErrMessageBox();
        }
    });
    return true;
}

function MsgControllerID_onChange(event) {
    $('#MsgActionName > option', _formElement).remove();
    $('#MsgActionName', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIFunList',
        type: 'POST',
        data: { SysID: $('#MsgSysID').val(), APIGroup: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#MsgActionName', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIFuns);
            _ShowJsErrMessageBox();
        }
    });
    return true;
}

//----Button----//
function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (result) {
        if (ValidObjectChecked() && ValidDate()) {
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
        if (ValidObjectChecked() && ValidDate()) {
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
    var msgSysId = $('#MsgSysID').val();
    var msgControllerId = $('#MsgControllerID').val();
    var msgActionName = $('#MsgActionName').val();
    if ((msgSysId === '' || msgSysId == undefined) == false) {
        if (msgControllerId === '' || msgControllerId == undefined ||
            msgActionName === '' || msgActionName == undefined) {
            _AddJsErrMessage(JsMsg_SystemMsgFunctionIsNotFullyFilledOrNotEmpty);
            return false;
        }
    }

    if ($('[name="HasRole"]:checked').length <= 0) {
        _AddJsErrMessage(JsMsg_SystemRoleIsEmpty);
        return false;
    }

    return true;
}

function ValidDate() {
    if ($('#DisableDate').val() !== '' && $('#EnableDate').val() !== '' && $('#DisableDate').val() <= $('#EnableDate').val()) {
        _AddJsErrMessage(JsMsg_DateIsError);
        return false;
    }

    return true;
}

function Help03Button_onClick(srcElement) {
    var vMapFields = new Array(3);
    vMapFields[1] = 'FlowManUserID';
    _hISearch(vMapFields, 'newwindow', _enumPUBAP + '/Help/Ishlp03', 'Name=' + encodeURI($.trim($('#FlowManUserID').val())));
}
