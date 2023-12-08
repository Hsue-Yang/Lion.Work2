var _formElement;

function RoleUserForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#RoleUserTable', _formElement);
    
    if ($('tbody tr', table).length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch, .tblvertical').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 1 });

        $('.BaseContainer').css('z-index', 0);
    }
    
    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: true,
        onChange: SysID_onChange
    });

    $('table.tblsearch #RoleCategoryID', _formElement).combobox({
        width: 180,
        isRemoveButton: true,
        onChange: RoleCategoryID_onChange
    });

    $('#SysID, #RoleCategoryID, #RoleID', $('table.tblsearch', _formElement)).combobox({
        width: 180,
        isRemoveButton: true
    });

    return true;
}

function SaveButton_onClick(srcElement) {
    var result = true;

    if ($('#UserChangeList:checked', $('#RoleUserTable')).length === 0) {
        _AddJsErrMessage(window.JsMsg_RoleUserList_Required);
        result = false;
    }

    if (result && _SearchConditionValidation() && _FormValidation()) {
        $('#UserChangeList:checked', $('#RoleUserTable')).attr('checked', false);
        $('#ExecAction').val(_ActionTypeUpdate);
        return true;
    }

    _ShowJsErrMessageBox();
    return false;
}

function SelectButton_onClick(srcElement) {
    if (_SearchConditionValidation()) {
        $('#ExecAction').val(_ActionTypeSelect);
        $('#RowDataInfo').val($('#SysID').val() + '|' + $('#RoleID').val());
        $.blockUI({ message: '' });
        _formElement.submit();
    }

    _ShowJsErrMessageBox();
    return false;
}

function _SearchConditionValidation() {
    var result = true;
    if ($('#SysID').val() === '' || $('#RoleID').val() === '') {
        _AddJsErrMessage(window.JsMsg_SysIDRoleID_Required);
        result = false;
    }

    return result;
}

function AddButton_onClick(srcElement) {
    if (_SearchConditionValidation()) {
        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }

    _ShowJsErrMessageBox();
    return false;
}

function SysID_onChange(event) {
    $.ajax({
        url: '/Sys/GetSystemRoleCategoryIDList',
        type: 'POST',
        data: {
            sysID: event.select.val()
        },
        dataType: 'json',
        async: false,
        success: function(result) {
            if (result != null) {
                $('#RoleCategoryID > option', _formElement).remove();
                $('#RoleCategoryID', _formElement).combobox('SetSelected', '');
                
                for (var i = 0; i < result.length; i++) {
                    $('#RoleCategoryID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function() {
            _AddJsErrMessage(window.JsMsg_GetSystemRoleCategoryIDList_Failure);
            _ShowJsErrMessageBox();
        }
    });

    $.ajax({
        url: '/Sys/GetSystemRoleIDList',
        type: 'POST',
        data: {
            sysID: event.select.val(),
            roleCategoryID: $('#RoleCategoryID').val()
        },
        dataType: 'json',
        async: false,
        success: function(result) {
            if (result != null) {
                $('#RoleID > option', _formElement).remove();
                $('#RoleID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#RoleID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function() {
            _AddJsErrMessage(window.JsMsg_GetSysSystemRoleIDList_Failure);
            _ShowJsErrMessageBox();
        }
    });
}

function RoleCategoryID_onChange(event) {
    $.ajax({
        url: '/Sys/GetSystemRoleIDList',
        type: 'POST',
        data: {
            sysID: $('#SysID').val(),
            roleCategoryID: event.select.val()
        },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#RoleID > option', _formElement).remove();
                $('#RoleID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#RoleID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function () {
            _AddJsErrMessage(window.JsMsg_GetSysSystemRoleIDList_Failure);
            _ShowJsErrMessageBox();
        }
    });
}