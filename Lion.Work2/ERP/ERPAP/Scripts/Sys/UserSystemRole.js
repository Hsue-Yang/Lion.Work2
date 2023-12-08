var _formElement;

function UserSystemRoleForm_onLoad(formElement) {
    _formElement = formElement;

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

    return true;
}

//----Tab----//
function UserBasicInfoDetail_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'UserBasicInfoDetail');
    $('#ExecAction').val(_ActionTypeSelect);

    _submit($(srcElement));

    return false;
}

//----Button----//
function CloseButton_onClick(srcElement) {
    _windowClose();
}