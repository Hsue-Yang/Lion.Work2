var _formElement;

function RoleUserDetailForm_onLoad(formElement) {
    _formElement = formElement;
    _RegisterAutoCompleteTextBoxEvent($('#RoleUserInfo'));
}

function OkayButton_onClick(srcElement) {
    var memo = $('#UserMemo').val().replace(/\ +|[\r\n]/g, '');
    $('#UserMemo').val(memo);

    if (UserMemoValidation() && _FormValidation()) {
        if ($('#RoleUserOperate').val() === 'Cover') {
            $.blockUI({ message: $('#dialog_Confirm') });
        } else {
            ConfirmButton_onClick();
        }
    }

    _ShowJsErrMessageBox();
    return false;
}

function UserMemoValidation(srcElement) {
    var result = true;

    if ($('#AddType:checked').val() === 'Comma') {
        if ($('#UserMemo').val() === '') {
            _AddJsErrMessage(JsMsg_UserID_Required);
            result = false;
        }
    } else {
        if ($('#RoleUserInfoListBox span').size() === 0) {
            _AddJsErrMessage(JsMsg_UserID_Required);
            return false;
        }
    }

    return result;
}

function CancelButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function ConfirmButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeAdd);
    _btnUnblockUI(this, false);
    _formElement.submit();
}

function AbolishmentButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function AddType_onClick(srcElement) {
    var roleUserInfoTD = $($('#RoleUserInfo').closest('td'), $(srcElement).closest('tr').next('tr'));
    var userMemoTD = $($('#UserMemo').closest('td'), $(srcElement).closest('tr').next('tr'));
    var addType = $('#AddType:checked').val();

    if (addType === 'Comma') {
        userMemoTD.show();
        roleUserInfoTD.hide();
    } else {
        userMemoTD.hide();
        roleUserInfoTD.show();
    }
}

function _RegisterAutoCompleteTextBoxEvent(srcElement) {
    RoleUserInfo_onKeyPress(srcElement);
}

function RoleUserInfo_onKeyPress(srcElement) {
    $(srcElement).autocomplete({
        source: function (request, response) {
            $.post('/Sys/GetRoleUserAutoUserInfo', { condition: request.term }, function (data) {
                if (data == null || data.length === 0) {
                    response(null);
                    return;
                }

                if (data.length === 1) {
                    $(srcElement).autocomplete('option', 'select').call(this, null, {
                        item: {
                            label: (data[0].UserID + ' ' + data[0].UserNM),
                            value: (data[0].UserID + '|' + data[0].UserNM)
                        },
                        sender: $(srcElement)
                    });
                    $(srcElement).autocomplete('close');
                } else if (data.length > 1) {
                    response($.map(data, function (item) {
                        return {
                            label: (item.UserID + ' ' + item.UserNM),
                            value: (item.UserID + '|' + item.UserNM)
                        }
                    }));
                }
            });
        },
        select: function (event, ui) {
            if (ui.item) {
                var itemVal = ui.item.value.split('|');
                if ($('#RoleUserInfoListBox input[name^=RoleUserInfoList][value=' + itemVal[0] + ']').length === 0) {
                    var para = {
                        id: 'RoleUserInfo',
                        text: itemVal[0] + ' ' + itemVal[1],
                        isRevmove: true
                    };

                    var userID = $(document.createElement('input')).attr({
                        'type': 'hidden',
                        'id': 'RoleUserInfoList[0].UserID',
                        'name': 'RoleUserInfoList[0].UserID',
                        value: itemVal[0]
                    });

                    var userNM = $(document.createElement('input')).attr({
                        'type': 'hidden',
                        'id': 'RoleUserInfoList[0].UserNM',
                        'name': 'RoleUserInfoList[0].UserNM',
                        value: itemVal[1]
                    });

                    para.elementList = [userID, userNM];
                    var tagStyle = Html.Tagstyle(para);
                    $('#RoleUserInfoListBox').append(tagStyle);
                    $('#RoleUserInfoListBox span.tagstyle').ToGroupElementReName();
                }

                srcElement.value = '';
                ui.item.value = '';
            }
        }
    });
}

function RoleUserInfoRemove_onClick(srcElement) {
    $(srcElement).closest('span.tagstyle').remove();
    $('#RoleUserInfoListBox span.tagstyle').ToGroupElementReName();
}