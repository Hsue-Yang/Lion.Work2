var _formElement;

function SystemFunForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function QuerySysID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#QuerySubSysID > option', _formElement).remove();
        $('#QueryFunControllerID > option', _formElement).remove();
        return false;
    }

    $.ajax({
        url: '/Sys/GetSystemSubsysIDList',
        type: 'POST',
        data: { sysID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QuerySubSysID > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QuerySubSysID', _formElement).append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemSubsysIDList);
            _ShowJsErrMessageBox();
        }
    });
    
    $.ajax({
        url: '/Sys/GetSystemFunControllerIDList',
        type: 'POST',
        data: { sysID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryFunControllerID > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QueryFunControllerID', _formElement).append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
            _ShowJsErrMessageBox();
        }
    });
    QueryFunControllerID_onChange();
}

function QueryFunControllerID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#QueryFunName > option', _formElement).remove();
        return false;
    }

    $.ajax({
        url: '/Sys/GetSystemFunActionNameList',
        type: 'POST',
        data: { SysID: $('#QuerySysID').val(), FunControllerID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryFunName > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QueryFunName', _formElement).append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunNameList);
            _ShowJsErrMessageBox();
        }
    });
}

function QueryFunMenuSysID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#QueryFunMenu > option', _formElement).remove();
        return false;
    }

    $.ajax({
        url: '/Sys/GetSystemFunMenuList',
        type: 'POST',
        data: { sysID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryFunMenu > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QueryFunMenu', _formElement).append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunMenuList);
            _ShowJsErrMessageBox();
        }
    });
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();
    
    $('#SysID').val(keys[1]);
    $('#FunControllerID').val(keys[2]);
    $('#FunActionName').val(keys[3]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyCopy_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#FunControllerID').val(keys[2]);
    $('#FunActionName').val(keys[3]);

    $('#ExecAction').val(_ActionTypeCopy);
    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    if (Result) {
        $.blockUI({ message: '' });
        Clean_HiddenValue();

        $('#SysID').val($('#QuerySysID').val());
        $('#FunControllerID').val($('#QueryFunControllerID').val());

        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
}

function SaveButton_onClick(srcElement) {
    var Result = _FormValidation();

    var dataCount = 10;
    if ($("[name*='PickList']:checked").length > dataCount) {
        Result = false;
        _AddJsErrMessage(String.format(JsMsg_ExceedDataCountLimit, dataCount));
        _ShowJsErrMessageBox();
    }

    if (Result) {
        $.blockUI({ message: '' });

        $('#ExecAction').val(_ActionTypeUpdate);
        return true;
    }
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#FunActionName').val('');
    $('#FunMenu').val('');
}