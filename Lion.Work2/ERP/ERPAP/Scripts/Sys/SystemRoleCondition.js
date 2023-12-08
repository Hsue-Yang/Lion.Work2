var _formElement;

function SystemRoleConditionForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#SystemRoleConditionTable')[0] != null) {
        _TableHover('SystemRoleConditionTable', formElement);
    }

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: SysID_onChange
    });

    $('#ConditionID, #IncludeRoleID', $('table.tblsearch', _formElement)).combobox({
        width: 180,
        isRemoveButton: true
    });

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, key) {
    $.blockUI({ message: '' });
    if ($('#ConditionID option').size() === 1) {
        $('#ConditionID', _formElement).append('<option value="' + key[1] + '"></option>');   
    }
    $('#ConditionID').val(key[1]);
    $('#SysID').val(key[2]);
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function AddButton_onClick(srcElement) {
    $('#ConditionID').val('');
    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}

function SelectButton_onClick(parameters) {
    _formElement.submit();
    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

function SysID_onChange(event) {
    $.ajax({
        url: '/Sys/GetSystemRoleIDList',
        type: 'POST',
        data: {
            sysID: event.select.val()
        },
        dataType: 'json',
        async: false,
        success: function(result) {
            if (result != null) {
                $('#IncludeRoleID > option', _formElement).remove();
                $('#IncludeRoleID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#IncludeRoleID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error:function() {
            _AddJsErrMessage(JsMsg_GetSystemRoleIDList_Failure);
            _ShowJsErrMessageBox();
            return false;
        }
    });

    $.ajax({
        url: '/Sys/GetSystemConditionIDList',
        type: 'POST',
        data: {
            sysID: event.select.val()
        },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#ConditionID > option', _formElement).remove();
                $('#ConditionID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#ConditionID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function () {
            _AddJsErrMessage(JsMsg_GetSystemConditionIDList_Failure);
            _ShowJsErrMessageBox();
            return false;
        }
    });
    return true;
}