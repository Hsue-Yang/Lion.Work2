var _formElement;

function SystemRoleForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function QuerySysID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#QueryRoleID > option', _formElement).remove();
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
                $('#QueryRoleID > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QueryRoleID', _formElement).append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemRoleIDList);
            _ShowJsErrMessageBox();
        }
    });
}

function DetailLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#RoleID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function ListLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#SysNM').val(keys[2]);
    $('#RoleID').val(keys[3]);
    $('#RoleNM').val(keys[4]);

    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function ListLinkRoleFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#QuerySysID').val(keys[1]);
    $('#RoleID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeSelect);
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
        $('#RoleID').val($('#QueryRoleID').val());

        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#SysNM').val('');
    $('#RoleID').val('');
    $('#RoleNM').val('');
}