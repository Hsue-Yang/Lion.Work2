var _formElement;

function UserRoleFunDetailForm_onLoad(formElement) {
    _formElement = formElement;

    $('#SysID', _formElement).combobox({
        width: 200,
        isRemoveButton: true,
        onChange: function(event) {
            $('tr', $('table#UserRoleFunDetailTable')).show();
            if ($(event.select).val() !== '') {
                $('tr[id != ' + $(event.select).val() + ']', $('table#UserRoleFunDetailTable')).hide();
            }
        }
    });

    $('.ui-combobox>span').css({ 'margin-top': 0 });

    if ($('#RoleGroupID').val() === '') {
        RoleOptionSwitch(false);
    } else {
        RoleOptionSwitch(true);
    }

    $('#CollapseButton').attr('isClose', true).val(JsMsg_Button_Unfold);
    SysSystemRoleFold();

    return true;
}

function UserFunction_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'UserFunction');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function UserPurview_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'UserPurview');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function RoleGroupID_onChange(srcElement) {
    if ($(srcElement).val() === '') {
        ResetFormOption();
        RoleOptionSwitch(false);
        return false;
    }

    RoleOptionSwitch(true);

    $.ajax({
        url: '/Sys/GetSystemRoleGroupCollectList',
        type: 'POST',
        data: { roleGroupID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                ResetFormOption();
                $('#RemarkReadOnlyText').val(result[0].Text);
                for (var i = 0; i < result.length; i++) {
                    $('input[name="HasRole"][value="' + result[i].Value + '"]').prop('checked', true);
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemRoleGroupCollectList);
            _ShowJsErrMessageBox();
        }
    });
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function GenerateUserMenu(isDevEnv) {
    $.blockUI({ message: '' });

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/Sys/GenerateUserMenu?isDevEnv=" + isDevEnv + "&userID=" + $('#UserID').val(), true);
    xhr.responseType = "blob";
    xhr.onload = function() {
        if (this.status === 200) {
            if (isDevEnv) {
                var blob = this.response;
                if (blob.size > 100) {
                    var reader = new FileReader();
                    reader.readAsDataURL(blob);
                    reader.onload = function (e) {
                        var a = document.createElement('a');
                        a.download = $('#UserID').val() + ".zip";
                        a.href = e.target.result;
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                    };
                    _alert('dialog_Confirm');
                    return;
                }
            } else {
                _alert('dialog_Confirm');
                return;
            }
        } 
        $.unblockUI();
        _AddJsErrMessage(window.JsMsg_GenerateUserMenu_Error);
        _ShowJsErrMessageBox();
    };
    xhr.send();
}

function GenMenuButton_onClick(srcElement) {
    GenerateUserMenu(false);
}

function GenMenuDevEnvButton_onClick(srcElement) {
    GenerateUserMenu(true);
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function OKButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

//----Private Function----//
function ResetFormOption() {
    $('#RemarkReadOnlyText').val('');
    
    $('input[name="HasRole"]').each(function () {
        this.checked = false;
    });
}

function RoleOptionSwitch(isBlock) {
    if (isBlock) {
        $('div[id="SystemRoleDiv"]').block({
            message: null,
            overlayCSS: {
                backgroundColor: '#fff',
                cursor:          'default'
            }
        });
    }
    else {
        $('div[id="SystemRoleDiv"]').unblock();
    }
}

function CollapseButton_onClick(srcElement) {
    if ($('#CollapseButton').attr('isClose') === 'true') {
        $('#CollapseButton').attr('isClose', false).val(JsMsg_Button_Fold);
        $('tr', $('table#UserRoleFunDetailTable')).show();
    } else {
        $('#CollapseButton').attr('isClose', true).val(JsMsg_Button_Unfold);
        SysSystemRoleFold();
    }
}

function SysSystemRoleFold() {
    var table = $('table#UserRoleFunDetailTable');
    for (var index = 0; index < unAuthSysList.length; index++) {
        var sysID = unAuthSysList[index];
        $('tr[id = ' + sysID + ']', table).hide();
    }
}