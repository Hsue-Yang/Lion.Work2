var _formElement;

function SystemRecordForm_onLoad(formElement) {
    _formElement = formElement;
    if ($(formElement).find('#SystemRecordTable')[0] != null) {
        _TableHover('SystemRecordTable', formElement);
    }

    SwitchTR();

    $('table.tblsearch #RecordType', _formElement).combobox({
        width: $('#RecordType', _formElement).width(),
        isRemoveButton: false,
        onChange: RecordType_onChange
    });

    $('#TimeBegin, #TimeEnd', $('table.tblsearch', _formElement)).combobox({
        isRemoveButton: false
    });

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: SysID_onChange
    });

    $('table.tblsearch #FunControllerID', _formElement).combobox({
        isRemoveButton: false,
        onChange: FunControllerID_onChange
    });
    
    $('#FunActionName, #ConditionID, #LineID, #RoleGroupID', $('table.tblsearch', _formElement)).combobox({
        width: 200,
        isRemoveButton: false
    });

    return true;
}

function LinkFunKeyUserApplyList_onClick(srcElement, keys) {
    var para = 'ErpWFNo=' + keys[1]
               + '&UserID=' + $('#UserID').val()
               + '&RecordType=' + $('#RecordType').val();
    _openWin('LinkFunKeyErpWFNoDetail', '/Sys/SystemRecordUserApplyList', para, { width: 800, height: 600 });
    return false;
}

function RoleConditionDetail_onClick(srcElement, keys) {
    var para = 'RulesJsonString=' + keys[1]
               + '&Roles=' + encodeURI(keys[2])
               + '&RecordType=' + $('#RecordType').val();
    _openWin('RoleConditionDetail', '/Sys/SystemRecordSysRoleConditionDetail', para, { width: 800, height: 600 });
    return false;
}

function UserPurviewDetail_onClick(srcElement, keys) {
    var para = 'PurviewCollectList=' + encodeURI(keys[1])
        + '&SysID=' + encodeURI(keys[2])
        + '&UserID=' + encodeURI(keys[3])
        + '&PurviewID=' + encodeURI(keys[4]);
    _openWin('UserPurviewDetail', '/Sys/SystemRecordUserPurviewDetail', para, { width: 800, height: 600 });
    return false;
}

function SysID_onChange(event) {
    if (event.select.val() === '') {
        $('#FunControllerID > option', _formElement).remove();
        $('#FunActionName > option', _formElement).remove();
        $('#ConditionID > option', _formElement).remove();
        $('#LineID > option', _formElement).remove();
        $('#FunControllerID', _formElement).combobox('SetSelected', '');
        $('#FunActionName', _formElement).combobox('SetSelected', '');
        $('#ConditionID', _formElement).combobox('SetSelected', '');
        $('#LineID', _formElement).combobox('SetSelected', '');
        return false;
    }

    if ($('#RecordType').val() === 'SysRoleCondition') {
        $.ajax({
            url: '/Sys/GetSystemConditionIDList',
            type: 'POST',
            data: {
                sysID: event.select.val()
            },
            dataType: 'json',
            async: false,
            success: function(result) {
                if (result != null) {
                    $('#ConditionID > option', _formElement).remove();
                    $('#ConditionID', _formElement).combobox('SetSelected', '');
                    for (var i = 0; i < result.length; i++) {
                        $('#ConditionID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                    }
                }
            },
            error: function() {
                _AddJsErrMessage(JsMsg_GetSystemConditionIDList_Failure);
                _ShowJsErrMessageBox();
                return false;
            }
        });
    }
    else if ($('#RecordType').val() === 'SysLine' || $('#RecordType').val() === 'SysLineReceiver') {
        $.ajax({
            url: '/Sys/GetLineBotIDList',
            type: 'POST',
            data: {
                sysID: event.select.val()
            },
            dataType: 'json',
            async: false,
            success: function(result) {
                if (result != null) {
                    $('#LineID > option', _formElement).remove();
                    $('#LineID', _formElement).combobox('SetSelected', '');
                    for (var i = 0; i < result.length; i++) {
                        $('#LineID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                    }
                }
            },
            error: function() {
                _AddJsErrMessage(JsMsg_GetSystemConditionIDList_Failure);
                _ShowJsErrMessageBox();
                return false;
            }
        });
    } else {
        $.ajax({
            url: '/Sys/GetFunctionGroupList',
            type: 'POST',
            data: { sysID: event.select.val() },
            dataType: 'json',
            async: false,
            success: function(result) {
                if (result != null) {
                    $('#FunControllerID > option', _formElement).remove();
                    $('#FunControllerID', _formElement).combobox('SetSelected', '');
                    for (var i = 0; i < result.length; i++) {
                        $('#FunControllerID', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                    }
                }
            },
            error: function(jqXHR, textStatus, errorThrown) {
                _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
                _ShowJsErrMessageBox();
            }
        });

        FunControllerID_onChange({ select: $('select[name=FunControllerID]') });
    }
    return false;
}

function FunControllerID_onChange(event) {
    if (event.select.val() === '') {
        $('#FunActionName > option', _formElement).remove();
        $('#FunActionName', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetFunctionNameList',
        type: 'POST',
        data: { SysID: $('#TR_Sys td #SysID').val(), FunControllerID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#FunActionName > option', _formElement).remove();
                $('#FunActionName', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#FunActionName', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunNameList);
            _ShowJsErrMessageBox();
        }
    });
    return false;
}

function RecordType_onChange(srcElement) {
    SwitchTR();
    $('.refreshValue td input.helper').val('');
    $('.refreshValue td select.helper').val('');
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    var userID = keys[1].split(' ')[0];

    if (userID.length === 4) {
        if (userID.substring(0, 1) === 'Z') {
            userID = 'ZZ' + userID;
        } else {
            userID = '00' + userID;
        }
    }

    userID = userID.length === 4 ? '00' + userID : userID;

    var para = 'UserID=' + userID +
               '&ExecAction=' + _ActionTypeSelect;
    var objfeatures = { width: window.screen.availWidth - 100, height: window.screen.availheight, top: 0, left: 0 };

    _openWin('newwindow', '/Sys/UserBasicInfoDetail', para, objfeatures);
    return false;
}

function LinkFunKeyAPIClientDetail_onClick(srcElement, keys) {
    var para = 'APINo=' + keys[1];
    var objfeatures = { width: 450, height: 500 };

    _openWin('newwindow', '/Sys/SystemAPIClientDetail', para, objfeatures);
    return false;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    var recordType = $('#RecordType').val();
    var errorMsg = '';

    if (recordType === 'UserLogin' || recordType === 'UserAccount' || recordType === 'UserAccess' || recordType === 'UserPassword' || recordType === 'UserSystemRole' || recordType === 'UserFun' || recordType === 'UserPurview') {
        if ($('#UserID').val() === '' || $('#UserID').val() == undefined) {
            result = false;
            errorMsg = JsMsg_RequireUserID;
        }
    }
    else if (recordType === 'SysRole' || recordType === 'SysFunGroup' || recordType === 'SysFun' || recordType === 'SysRoleFun' || recordType === 'SysRoleCondition' || recordType === 'SysLine' || recordType === 'SysLineReceiver') {
        if ($('[name=SysID]').eq(1).val() === '' || $('[name=SysID]').eq(1).val() == undefined) {
            result = false;
            errorMsg = JsMsg_RequireSysID + '<br />';
        }

        if (recordType === 'SysRoleFun') {
            if (($('#FunControllerID').val() === '' || $('#FunControllerID').val() == undefined)) {
                result = false;
                errorMsg += JsMsg_RequireFunControllerID + '<br />';
            }
            if (($('#FunActionName').val() === '' || $('#FunActionName').val() == undefined)) {
                result = false;
                errorMsg += JsMsg_RequireFunActionName;
            }
        }
        $('#SysID').val($('[name=SysID]').eq(1).val());
    }
    else if (recordType === 'SysRoleGroupCollect') {
        if ($('#RoleGroupID').val() === '' || $('#RoleGroupID').val() == undefined) {
            result = false;
            errorMsg = JsMsg_RequireRoleGroupID;
        }
    }
    
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        _formElement.submit();
    }
    else {
        $.unblockUI();

        _AddJsErrMessage(errorMsg);
        _ShowJsErrMessageBox();
    }
}

function Help03Button_onClick(srcElement) {
    var vMapFields = new Array(1);
    vMapFields[1] = 'UserID';
    _hISearch(vMapFields, 'newwindow', _enumPUBAP + '/Help/Ishlp03', 'Name=' + encodeURI($.trim($('#UserID').val())));
}

//----Private Function----//
function SwitchTR() {
    var recordType = $('#RecordType').val();

    if (recordType === 'UserLogin' ||
        recordType === 'UserAccount' ||
        recordType === 'UserAccess' ||
        recordType === 'UserPassword' ||
        recordType === 'UserSystemRole' ||
        recordType === 'UserFun' ||
        recordType === 'UserPurview') {
        if ($('#TR_User:visible').length === 0) {
            $('#UserID').val('');
        }
        $('#TR_User').show();
        $('#TR_Sys').hide();
        $('#TR_RoleGroup').hide();
    } else if (recordType === 'SysRole' ||
        recordType === 'SysFunGroup' ||
        recordType === 'SysFun' ||
        recordType === 'SysRoleFun') {
        if ($('#TR_Sys:visible').length === 0) {
            $('#SysID').val('');
            $('#FunControllerID').val('');
            $('#FunActionName').val('');
        }
        $('#TR_User').hide();
        $('#TR_Sys').show();
        $('#TR_RoleGroup').hide();
    } else if (recordType === 'SysRoleGroupCollect') {
        if ($('#TR_RoleGroup:visible').length === 0) {
            $('#RoleGroupID').val('');
        }
        $('#TR_User').hide();
        $('#TR_Sys').hide();
        $('#TR_RoleGroup').show();
    } else if (recordType === 'SysRoleCondition') {
        if ($('#TR_Sys:visible').length === 0) {
            $('#SysID').val('');
            $('#ConditionID').val('');
        }
        $('#TR_User').hide();
        $('#TR_Sys').show();
        $('#TR_RoleGroup').hide();
    }
    else if (recordType === 'SysLine') {
        if ($('#TR_Sys:visible').length === 0) {
            $('#SysID').val('');
            $('#LineID').val('');
        }
        $('#TR_User').hide();
        $('#TR_Sys').show();
        $('#TR_RoleGroup').hide();
    }
    else if (recordType === 'SysLineReceiver') {
        if ($('#TR_Sys:visible').length === 0) {
            $('#SysID').val('');
            $('#LineID').val('');
        }
        $('#TR_User').hide();
        $('#TR_Sys').show();
        $('#TR_RoleGroup').hide();
    } else {
        $('#TR_User').hide();
        $('#TR_Sys').hide();
        $('#TR_RoleGroup').hide();
    }

    if (recordType === 'UserSystemRole' ||
        recordType === 'UserFun' ||
        recordType === 'SysRoleFun' ||
        recordType === 'SysRoleGroupCollect' ||
        recordType === 'SysRoleCondition' ||
        recordType === 'UserPurview') {
        $('.QueryDiffData').show();
        $('#IsOnlyDiffData').change(function () {
            if (this.checked) {
                $('#IsOnlyDiffData').val('Y')
            } else {
                $('#IsOnlyDiffData').val('N')
            }
        });
    } else {
        $('#IsOnlyDiffData').val('N');
        $('.QueryDiffData').hide();
    }

    if (recordType === 'SysRoleFun') {
        $('.QueryFunControllerID').show();
        $('.QueryFunActionName').show();
    } else {
        $('.QueryFunControllerID').hide();
        $('.QueryFunActionName').hide();
    }

    if (recordType === 'SysRoleCondition') {
        $('.QueryConditionID').show();
    } else {
        $('.QueryConditionID').hide();
    }

    if (recordType === 'SysLine' || recordType === 'SysLineReceiver') {
        $('.QueryLineID').show();
    } else {
        $('.QueryLineID').hide();
    }

    if (recordType === 'UserSystemRole') {
        $('.SysID').show();
    } else {
        $('.SysID').hide();
    }
}