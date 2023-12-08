var _formElement;

function UserRoleFunDetailForm_onLoad(formElement) {
    _formElement = formElement;

    if ($('#RoleGroupID').val() == '') {
        RoleOptionSwitch(false);
    }
    else {
        RoleOptionSwitch(true);
    }

    return true;
}

function RoleGroupID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
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
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemRoleGroupCollectList);
            _ShowJsErrMessageBox();
        }
    });
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
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
                cursor: 'default'
            }
        });
    }
    else {
        $('div[id="SystemRoleDiv"]').unblock();
    }
}