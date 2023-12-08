var _formElement;

function SystemLoginEventSettingForm_onLoad(formElement) {
    _formElement = formElement;

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: true,
        onChange: SysID_onChange
    });

    $('table.tblsearch #LoginEventID', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });
}

function MoveUp_onClick(srcElement) {
    var after;
    var current = $('input[name="IsMoved"]:checked').parent().parent();
    var prev = current.prev();
    if (prev[0] != undefined) {
        current.insertBefore(prev);
        after = prev.find('input[name$="AfterSortOrder"]').val();
        if (after != null) {
            prev.find('input[name$="AfterSortOrder"]').val(current.find('input[name$="AfterSortOrder"]').val());
            current.find('input[name$="AfterSortOrder"]').val(after);
        }
    }
}

function SaveButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeUpdate);
    _formElement.submit();
}

function MoveDown_onClick(srcElement) {
    var after;
    var current = $('input[name="IsMoved"]:checked').parent().parent();
    var next = current.next();
    if (next[0] != undefined) {
        current.insertAfter(next);
        after = next.find('input[name$="AfterSortOrder"]').val();
        if (after != null) {
            next.find('input[name$="AfterSortOrder"]').val(current.find('input[name$="AfterSortOrder"]').val());
            current.find('input[name$="AfterSortOrder"]').val(after);
        }
    }
}

function SysID_onChange(event) {
    $.ajax({
        url: '/Sys/GetSysLoginEventIDList',
        type: 'POST',
        data: {
            sysID: event.select.val()
        },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#LoginEventID > option', _formElement).remove();
                $('#LoginEventID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#LoginEventID', _formElement).append('<option value="' + result[i].Key + '">' + result[i].Value + '</option>');
                }
            }
        },
        error: function () {
            _AddJsErrMessage(JsMsg_GetSysLoginEventIDList_Failure);
            _ShowJsErrMessageBox();
            return false;
        }
    });
    return true;
}

function AddButton_onClick(srcElement) {
    if ($('#SysID').val() !== '') {
        $('#LoginEventID').val('');
        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    } else {
        _AddJsErrMessage(JsMsg_SysID_Required);
        _ShowJsErrMessageBox();
        return false;
    }
}

function DetailLinkFunKey_onClick(srcElement, key) {
    $.blockUI({ message: '' });
    $('#ExecAction').val(_ActionTypeUpdate);
    $('#SysID').val(key[1]);

    var loginEvent = $('#LoginEventID');

    if ($('option[value="' + key[2] + '"]', loginEvent).length === 0) {
        loginEvent.append('<option value="' + key[2] + '"></option>');
    }

    $('#LoginEventID').val(key[2]);
    return true;
}