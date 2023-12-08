var _formElement;

function SystemRoleFunListForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function QuerySysID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#RoleID > option', _formElement).remove();
        $('#QueryFunGroup > option', _formElement).remove();
        
        return false;
    }
    $.ajax({
        url: '/Sys/GetSystemRoleIDList',
        type: 'POST',
        data: { sysID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#RoleID > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#RoleID', _formElement).append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemRoleIDList);
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
            _AddJsErrMessage(JsMsg_GetSysSystemRoleIDList);
            _ShowJsErrMessageBox();
        }
    });
}

function SelectButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
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

function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#FunControllerID').val('');
    $('#FunActionName').val('');
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemRole';
}