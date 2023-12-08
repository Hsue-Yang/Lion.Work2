var _formElement;

function SystemWorkFlowNodeDetailForm_onLoad(formElement) {
    _formElement = formElement;

    SwitchFunObject($('#NodeType')[0]);

    $('select[name=NodeType]', _formElement).combobox({
        width: 200,
        isRemoveButton: false,
        onChange: NodeType_onChange
    });

    $('select[name=BackWFNodeID]', _formElement).combobox({
        width: 200,
        isRemoveButton: false
    });

    $('select[name=FunSysID]', _formElement).combobox({
        width: 200,
        isRemoveButton: false,
        onChange: FunSysID_onChange
    });

    $('select[name=AssgAPISysID]', _formElement).combobox({
        width: 200,
        isRemoveButton: false,
        onChange: AssgAPISysID_onChange
    });

    $('select[name=FunControllerID]', _formElement).combobox({
        width: 200,
        isRemoveButton: false,
        onChange: FunControllerID_onChange
    });

    $('select[name=AssgAPIControllerID]', _formElement).combobox({
        width: 200,
        isRemoveButton: false,
        onChange: AssgAPIControllerID_onChange
    });

    $('select[name=FunActionName], select[name=AssgAPIActionName]', _formElement).combobox({
        width: 200,
        isRemoveButton: false
    });

    $('select[name=APIControllerID]', _formElement).combobox({
        width: 200,
        isRemoveButton: false,
        onChange: APIControllerID_onChange
    });

    $('select[name=APIActionName]', _formElement).combobox({
        width: 200,
        isRemoveButton: false
    });
    return true;
}

function NodeType_onChange(event) {
    $('#FunSysID', _formElement).combobox('SetSelected', '');
    $('#FunControllerID > option', _formElement).remove();
    $('#FunActionName > option', _formElement).remove();
    $('#APIControllerID > option', _formElement).remove();
    $('#APIActionName > option', _formElement).remove();
    $('#FunControllerID', _formElement).combobox('SetSelected', '');
    $('#FunActionName', _formElement).combobox('SetSelected', '');
    $('#APIControllerID', _formElement).combobox('SetSelected', '');
    $('#APIActionName', _formElement).combobox('SetSelected', '');
    
    SwitchFunObject(event.select[0]);
}

function FunSysID_onChange(event) {
    $('#FunControllerID > option', _formElement).remove();
    $('#FunActionName > option', _formElement).remove();
    $('#APIControllerID > option', _formElement).remove();
    $('#APIActionName > option', _formElement).remove();
    $('#FunControllerID', _formElement).combobox('SetSelected', '');
    $('#FunActionName', _formElement).combobox('SetSelected', '');
    $('#APIControllerID', _formElement).combobox('SetSelected', '');
    $('#APIActionName', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '')
        return false;
        
    if ($('#NodeType').val() === 'P' || $('#NodeType').val() === 'S' || $('#NodeType').val() === 'E') {
        $.ajax({
            url: '/Sys/GetSystemFunControllerIDList',
            type: 'POST',
            data: { SysID: event.select.val() },
            dataType: 'json',
            async: false,
            success: function (result) {
                if (result != null) {
                    for (var i = 0; i < result.length; i++) {
                        $('#FunControllerID', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
                _ShowJsErrMessageBox();
            }
        });
    }

    if ($('#NodeType').val() === 'D') {
        $.ajax({
            url: '/Sys/GetSysSystemAPIGroupList',
            type: 'POST',
            data: { SysID: event.select.val() },
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
    return true;
}

function FunControllerID_onChange(event) {
    $('#FunActionName > option', _formElement).remove();
    $('#FunActionName', _formElement).combobox('SetSelected', '');
    
    if (event.select.val() === '')
        return false;
  
    $.ajax({
        url: '/Sys/GetSystemFunNameList',
        type: 'POST',
        data: { SysID: $('#FunSysID').val(), FunControllerID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#FunActionName', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunNameList);
            _ShowJsErrMessageBox();
        }
    });
    return true;
}

function APIControllerID_onChange(event) {
    $('#APIActionName > option', _formElement).remove();
    $('#APIActionName', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIFunList',
        type: 'POST',
        data: { SysID: $('#FunSysID').val(), APIGroup: event.select.val() },
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
    return true;
}

function AssgAPISysID_onChange(event) {
    $('#AssgAPIControllerID > option', _formElement).remove();
    $('#AssgAPIActionName > option', _formElement).remove();
    $('#AssgAPIControllerID', _formElement).combobox('SetSelected', '');
    $('#AssgAPIActionName', _formElement).combobox('SetSelected', '');

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
                    $('#AssgAPIControllerID', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIGroupList);
            _ShowJsErrMessageBox();
        }
    });
    return true;
}

function AssgAPIControllerID_onChange(event) {
    $('#AssgAPIActionName > option', _formElement).remove();
    $('#AssgAPIActionName', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIFunList',
        type: 'POST',
        data: { SysID: $('#AssgAPISysID').val(), APIGroup: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#AssgAPIActionName', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIFunList);
            _ShowJsErrMessageBox();
        }
    });
    return true;
}

//----Tab----//
function SystemWorkFlowNext_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemWorkFlowNext');
    $('#ExecAction').val(_ActionTypeUpdate);
    
    _submit($(srcElement));

    return false;
}

function SystemWorkFlowSignature_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemWorkFlowSignature');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function SystemWorkFlowDocument_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemWorkFlowDocument');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

//----Button----//
function AddButton_onClick(srcElement) {
    var result = _FormValidation();

    if (result) {
        result = ValidSystemFunction();
    }

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

    if (result) {
        result = ValidSystemFunction();
    }

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

function CloseButton_onClick(srcElement) {
    _windowClose();
}

function ConfirmOKButton_onClick(srcElement) {
    var result = _FormValidation();

    $('#ExecAction').val(_ActionTypeDelete);
    if (result) {
        if (ValidObjectChecked()) {
            $.blockUI({ message: '' });
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

//----Private Function----//
function SwitchFunObject(srcElement) {
    if (srcElement.value === 'D') {
        $('#spanBackWFNodeID').hide();
        
        $('#spanFunSysID').show();        
        $('#spanFunControllerID').hide();
        $('#spanFunActionName').hide();
        $('#spanAPIGroup').show();
        $('#spanAPIFun').show();
        $('#spanAPIMemo').show();

        $('#spanIsFirst').hide();
        $('#spanIsFinally').hide();

        $('#spanHasRole').show();
    }
    else if (srcElement.value === 'P' || srcElement.value === 'S') {
        $('#spanFunSysID').show();        
        $('#spanFunControllerID').show();
        $('#spanFunActionName').show();
        $('#spanAPIGroup').hide();
        $('#spanAPIFun').hide();
        $('#spanAPIMemo').hide();

        $('#spanHasRole').show();
        
        if (srcElement.value === 'P') {
            $('#spanBackWFNodeID').show();

            $('#spanIsFirst').show();
            $('#spanIsFinally').show();
        } else {
            $('#spanBackWFNodeID').hide();

            $('#spanIsFirst').hide();
            $('#spanIsFinally').hide();
        }
    }
    else {
        $('#spanBackWFNodeID').hide();

        $('#spanFunSysID').show();
        $('#spanFunControllerID').show();
        $('#spanFunActionName').show();
        $('#spanAPIGroup').hide();
        $('#spanAPIFun').hide();
        $('#spanAPIMemo').hide();
        
        $('#spanIsFirst').hide();
        $('#spanIsFinally').hide();

        $('#spanHasRole').hide();
    }
}

function ValidSystemFunction() {
    var result = true;
    var funSysId = $('#FunSysID').val();
    var funControllerId = $('#FunControllerID').val();
    var funActionName = $('#FunActionName').val();
    var apiGroup = $('#APIControllerID').val();
    var apiFun = $('#APIActionName').val();

    if ($('#NodeType').val() === 'P') {
        if (funSysId === '' || funSysId == undefined ||
            funControllerId === '' || funControllerId == undefined ||
            funActionName === '' || funActionName == undefined) {
            result = false;
            _AddJsErrMessage(JsMsg_SystemFunctionIsEmpty);
            _ShowJsErrMessageBox();
        }
    }

    if ($('#NodeType').val() === 'D') {
        if (funSysId === '' || funSysId == undefined ||
            apiGroup === '' || apiGroup == undefined ||
            apiFun === '' || apiFun == undefined) {
            result = false;
            _AddJsErrMessage(JsMsg_SystemFunctionIsEmpty);
            _ShowJsErrMessageBox();
        }
    }

    if ($('#IsAssgNextNode:checked').length > 0 &&
    ($('#AssgAPISysID').val() > '' || $('#AssgAPIControllerID').val() > '' || $('#AssgAPIActionName').val() > '') === false) {
        result = false;
        _AddJsErrMessage(JsMsg_SystemAssgAPIFunctionIsEmpty);
        _ShowJsErrMessageBox();
    }

    return result;
}

function ValidObjectChecked() {
    return true;
}