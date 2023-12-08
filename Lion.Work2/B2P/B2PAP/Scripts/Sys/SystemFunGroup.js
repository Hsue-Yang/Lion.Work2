var _formElement;

function SystemFunGroupForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function QuerySysID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#QueryFunControllerID > option', _formElement).remove();
        return false;
    }

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
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#FunControllerID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
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

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#FunControllerID').val('');
}