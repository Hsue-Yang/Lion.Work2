var _formElement;
var _targetSysID;

function SystemEventTargetForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#SystemEventTargetTable')[0] != null) {
        _TableHover('SystemEventTargetTable', formElement);
    }

    $('#TargetSysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: TargetSysID_onChange
    });

    $('#SubSysID', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });

    return true;
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    _targetSysID = keys[1];

    _alert('dialog_Confirm');
}

function TargetSysID_onChange(event) {
    if (event.select.val() === '') {
        $('#SubSysID > option', _formElement).remove();
        $('#SubSysID', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemSubByIdList',
        type: 'POST',
        data: { sysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#SubSysID > option', _formElement).remove();
            $('#SubSysID', _formElement).combobox('SetSelected', '');
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#SubSysID', _formElement).append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_UnGetSystemSubByIdList);
            _ShowJsErrMessageBox();
        }
    });
}

//----Button----//
function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function EventParaButton_onClick(srcElement) {
    var para = "SysID=" + $('#SysID').val() + "&"
        + "EventGroupID=" + $('#EventGroupID').val() + "&"
        + "EventID=" + $('#EventID').val();

    var objfeatures = { width: 400, height: 500 };

    _openWin("newwindow", "/Sys/SystemEventPara", para, objfeatures);
    return false;
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemEvent';
}

function ConfirmOKButton_onClick(srcElement) {
    $('#TargetSysID').val(_targetSysID);
    $('#TargetPath').val('');
    _targetSysID = '';

    $('#ExecAction').val(_ActionTypeDelete);
    _formElement.submit();
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

//----Private Function----//
