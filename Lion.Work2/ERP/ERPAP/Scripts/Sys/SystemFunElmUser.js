var _formElement, _addSetUserDialog, _elmUserDivNM, _disPlayTypeDiv;

function SystemFunElmUserForm_onLoad(formElement) {
    _formElement = formElement;
    _disPlayTypeDiv = ['DISPLAYUserList', 'READ_ONLYUserList', 'MASKINGUserList', 'HIDEUserList'];
    CreateSetUserDialog();
    InitAutocompleteUserID();
    $('#MultiUserMemo').attr('placeholder', 'ex: 000001,000002');
}

function IsSingleUser_onClick(srcElement) {
    var isSingleuser = $('input[name=IsSingleUser]:checked').val();

    if (isSingleuser === 'Y') {
        $('#MultiUserMemo').closest('td').hide();
        $('#ElmUserID').closest('td').show();
        $('#DialogConfirm').hide();
    } else {
        $('#MultiUserMemo').closest('td').show();
        $('#ElmUserID').closest('td').hide();
        $('#DialogConfirm').show();
    }
}

function CreateSetUserDialog(srcElement) {
    _addSetUserDialog = $('#SetUserDialog');
    _addSetUserDialog.dialog({
        title: JsMsg_SetElmUser,
        autoOpen: false,
        modal: true,
        resizable: false,
        height: 400,
        width: 600,
        open: function(event, ui) {
            $('#errmsg').html('');
            $('#MultiUserMemo').val('');
        },
        buttons: {
            click: {
                'class': 'btn',
                text: JsMsg_Confirm,
                id: 'DialogConfirm',
                click: function() {
                    if (MultiUserMemoValidation()) {
                        if ($('input[name=IsSingleUser]:checked').val() === 'N') {
                            var userArr = $('#MultiUserMemo').val().split(',');
                            var userRepeatList = $.grep(userArr, function(item) {
                                return $('input[id^=FunElmUserDictionary][type=hidden][value=' + item + ']', _formElement).length > 0;
                            });

                            if (userRepeatList.length > 0) {
                                $('#errmsg').html(userRepeatList.join(',') + ' ' + JsMsg_UserIDRepaet_Failure);
                                return false;
                            }
                            $('#DialogConfirm').attr("disabled", true).addClass("ui-state-disabled");

                            $.ajax({
                                url: '/Sys/GetElmRawCMUserList',
                                type: 'POST',
                                data: {
                                    userIDList: userArr
                                },
                                dataType: 'json',
                                async: true,
                                success: function (result) {
                                    $('#DialogConfirm').removeAttr("disabled", true).removeClass("ui-state-disabled");

                                    if (result != null) {
                                        var divObject = $('div[name=' + _elmUserDivNM + ']');
                                        for (var i = 0; i < result.length; i++) {
                                            var para = {
                                                id: 'ElmUserInfo',
                                                text: result[i].UserIDNM,
                                                isRevmove: true
                                            };

                                            var userID = $(document.createElement('input')).attr({
                                                'type': 'hidden',
                                                'id': 'FunElmUserDictionary[' + _disPlayTypeDiv.indexOf(_elmUserDivNM) + '].value[999].UserID',
                                                'name': 'FunElmUserDictionary[' + _disPlayTypeDiv.indexOf(_elmUserDivNM) + '].value[999].UserID',
                                                value: result[i].UserID
                                            });

                                            para.elementList = [userID];
                                            var tagStyle = Html.Tagstyle(para);
                                            $(divObject).append(tagStyle);
                                        }
                                        FunElmUserDictionaryReName();
                                        _addSetUserDialog.dialog('close');
                                    } else {
                                        $('#errmsg').html(JsMsg_NotHaveUserIDOrRepeat_Failure);
                                    }
                                },
                                error: function () {
                                    $('#DialogConfirm').removeAttr("disabled", true).removeClass("ui-state-disabled");
                                    $('#errmsg').html(JsMsg_GetFunElmUserInfo_Failure);
                                }
                            });
                        } else {
                            _addSetUserDialog.dialog('close');
                        }
                    }
                    return false;
                }
            }
        }
    });
}

function MultiUserMemoValidation(srcElement) {
    if ($('#MultiUserMemo').val().match(/^([0-9a-zA-Z]{6},)*([0-9a-zA-Z]{6})$/) === null) {
        $('#errmsg').html(JsMsg_UserMemoFormate_Error);
        return false;
    }

    return true;
}

function ElmUserBtn_onClick(srcElement) {
    $('input[name=IsSingleUser][value="Y"]').attr('checked', true);
    $('#ElmUserID').val('');

    IsSingleUser_onClick();

    _elmUserDivNM = $('div', $(srcElement).closest('tr')).attr('name');
    _addSetUserDialog.dialog('open');
    return false;
}

function InitAutocompleteUserID(srcElement) {
    var elmuserID = $('#ElmUserID');
    elmuserID.autocomplete({
        minLength: 4,
        source: function(request, response) {
            var self = this;
            $('#errmsg').html('');

            $.post('/Sys/GetFunElmUserInfo', { condition: request.term }, function(data) {
                if (data == null || data.length === 0) {
                    response(null);
                    return;
                }

                if (data.length === 1) {
                    response($.map(data, function(item) {
                        return {
                            label: (item.UserID + ' ' + item.UserNM),
                            value: (item.UserID + '|' + item.UserNM)
                        }
                    }));

                    elmuserID.autocomplete('option', 'select').call(this, null, {
                        item: {
                            label: (data[0].UserID + ' ' + data[0].UserNM),
                            value: (data[0].UserID + '|' + data[0].UserNM)
                        }
                    });
                    response(null);
                    self.element.val('');
                    $('input[name=IsSingleUser]:checked').focus();

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
                var userIDNM = itemVal[0] + ' ' + itemVal[1];
                var selector = 'input[name^=FunElmUserDictionary][name$=UserID][value=' + itemVal[0] + ']';

                if ($(selector, _formElement).length > 0) {
                    $('#errmsg').html(userIDNM + ' ' + JsMsg_UserIDRepaet_Failure);
                } else {
                    var para = {
                        id: 'ElmUserInfo',
                        text: userIDNM,
                        isRevmove: true
                    };
                    
                    var userID = $(document.createElement('input')).attr({
                        'type': 'hidden',
                        'id': 'FunElmUserDictionary[' + _disPlayTypeDiv.indexOf(_elmUserDivNM) + '].value[999].UserID',
                        'name': 'FunElmUserDictionary[' + _disPlayTypeDiv.indexOf(_elmUserDivNM) + '].value[999].UserID'
                    }).val(itemVal[0]);
                    
                    para.elementList = [userID];
                    var tagStyle = Html.Tagstyle(para);
                    $('div[name=' + _elmUserDivNM + ']').append(tagStyle);
                    FunElmUserDictionaryReName();
                }

                elmuserID.val('');
                ui.item.value = '';
            }
        }
    });
}

function ElmUserInfoRemove_onClick(srcElement) {
    _elmUserDivNM = $(srcElement).closest('div').attr('name');
    $(srcElement).closest('span.tagstyle').remove();
    FunElmUserDictionaryReName();
}

function FunElmUserDictionaryReName() {
    $('input[name^="FunElmUserDictionary"][name$="UserID"][type="hidden"]', $('div[name=' + _elmUserDivNM + ']')).each(function(idx, el) {
        var elIdNm = $(el).attr('name');
        $(el).attr('id', elIdNm.replace(/value\[\d+]/g, 'value[' + idx + ']'));
        $(el).attr('name', elIdNm.replace(/value\[\d+]/g, 'value[' + idx + ']'));
    });
}

function SaveButton_onClick(srcElement) {
    var result = _FormValidation();

    if (result) {
        $.blockUI({ message: '' });

        for (var i = 0; i < _disPlayTypeDiv.length; i++) {
            var length = $('input[name^="FunElmUserDictionary"][name$="UserID"][type="hidden"]',
                $('div[name=' + _disPlayTypeDiv[i] + ']')).length;

            if (length === 0) {
                var userHiddenField = $(document.createElement('input')).attr({
                    'type': 'hidden',
                    'id': 'FunElmUserDictionary[' + _disPlayTypeDiv.indexOf(_disPlayTypeDiv[i]) + '].value[0].UserID',
                    'name': 'FunElmUserDictionary[' + _disPlayTypeDiv.indexOf(_disPlayTypeDiv[i]) + '].value[0].UserID'
                });
                $('div[name=' + _disPlayTypeDiv[i] + ']').append(userHiddenField);
            }
        }

        $('#ExecAction').val(_ActionTypeUpdate);
        return true;
    }

    _ShowJsErrMessageBox();
    return false;
}

function CloseButton_onClick(srcElement) {
    window.close();
}