var _formElement;

function PushNotificationDetailForm_onLoad(formElement) {
    _formElement = formElement;
    _RegisterAutoCompleteTextBoxEvent($('#PushMsgUserInfo'));
    ImmediatelyPush_onChange();

    $('#ImmediatelyPush', _formElement).combobox({
        isRemoveButton: true,
        onChange: ImmediatelyPush_onChange
    });
}

function CancelButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function OkayButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function ImmediatelyPush_onChange(srcElement) {
    if ($('#ImmediatelyPush').val() === 'N') {
        $('#PushDate').closest('tr').show();
    } else {
        $('#PushDate').closest('tr').hide();
    }
}

function _RegisterAutoCompleteTextBoxEvent(srcElement) {
    PushMsgUserInfo_onKeyPress(srcElement);
}

function PushTime_onBlur(srcElement) {
    if (srcElement.value.length === 4) {
        srcElement.value = srcElement.value.substr(0, 2) + ':' + srcElement.value.substr(2, 2);
    }
}

function PushMsgUserInfo_onKeyPress(srcElement) {
    $(srcElement).autocomplete({
        source: function(request, response) {
            $.post('/Pub/GetPushMsgUserInfo', { condition: request.term }, function (data) {
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
                    response($.map(data, function(item) {
                        return {
                            label: (item.UserID + ' ' + item.UserNM),
                            value: (item.UserID + '|' + item.UserNM)
                        }
                    }));
                }
            });
        },
        select: function(event, ui) {
            if (ui.item) {
                var itemVal = ui.item.value.split('|');
                if ($('#PushMsgUserInfoListBox input[name^=PushMsgUserInfoList][value=' + itemVal[0] + ']').length === 0) {
                    var para = {
                        id: 'PushMsgUserInfo',
                        text: itemVal[0] + ' ' + itemVal[1],
                        isRevmove: true
                    };

                    var userID = $(document.createElement('input')).attr({
                        'type': 'hidden',
                        'id': 'PushMsgUserInfoList[0].UserID',
                        'name': 'PushMsgUserInfoList[0].UserID',
                        value: itemVal[0]
                    });

                    var userNM = $(document.createElement('input')).attr({
                        'type': 'hidden',
                        'id': 'PushMsgUserInfoList[0].UserNM',
                        'name': 'PushMsgUserInfoList[0].UserNM',
                        value: itemVal[1]
                    });

                    para.elementList = [userID, userNM];
                    var tagStyle = Html.Tagstyle(para);
                    $('#PushMsgUserInfoListBox').append(tagStyle);
                    $('#PushMsgUserInfoListBox span.tagstyle').ToGroupElementReName();
                }

                srcElement.value = '';
                ui.item.value = '';
            }
        }
    });
}

function PushMsgUserInfoRemove_onClick(srcElement) {
    $(srcElement).closest('span.tagstyle').remove();
    $('#PushMsgUserInfoListBox span.tagstyle').ToGroupElementReName();
}