var _formElement;

function LineBotAccountSettingForm_onLoad(formElement) {
    _formElement = formElement;

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: SysID_onChange
    });

    $('table.tblsearch #LineID', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });
}

function SysID_onChange(event) {
    $.ajax({
        url: '/Sys/GetLineBotIDList',
        type: 'POST',
        data: {
            sysID: event.select.val()
        },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#LineID > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#LineID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
                $('#LineID', _formElement).combobox('SetSelected', '');
            }
        },
        error: function () {
            _AddJsErrMessage(JsMsg_GetLineBotIDList_Failure);
            _ShowJsErrMessageBox();
            return false;
        }
    });
    return true;
}

function AddButton_onClick(srcElement) {
    $('#LineID').val('');
    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}

function DetailLinkFunKey_onClick(srcElement, key) {
    $.blockUI({ message: '' });
    $('#SysID').val(key[1]);
    $('#LineID').val(key[2]);
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function ListLinkFunKey_onClick(srcElement, key) {
    $.blockUI({ message: '' });
    $('#SysID').val(key[1]);
    $('#LineID').val(key[2]);
    return true;
}