var _formElement;

function SystemWorkFlowNodeForm_onLoad(formElement) {
    _formElement = formElement;

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: SysID_onChange
    });

    $('table.tblsearch #WFFlowGroupID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: WFFlowGroupID_onChange
    });

    $('table.tblsearch #WFCombineKey', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: WFCombineKey_onChange
    });

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $('#ExecAction').val(_ActionTypeUpdate);

    var para = 'SysID=' + keys[1] +
               '&WFFlowGroupID=' + keys[2] +
               '&WFFlowID=' + keys[3] +
               '&WFFlowVer=' + keys[4] +
               '&WFNodeID=' + keys[5] + 
               '&ExecAction=' + _ActionTypeUpdate;
    var objfeatures = { width: window.screen.availWidth, height: window.screen.availWidth, top: 0, left: 0 };

    _openWin('newwindow', '/Sys/SystemWorkFlowNodeDetail', para, objfeatures);
}

//----Button----//
function SearchButton_onClick(srcElement) { //按下搜尋，驗證combobox有無值
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    if (result) {
        $('#ExecAction').val(_ActionTypeAdd);

        var para = 'SysID=' + $('#SysID').val() +
                   '&WFFlowGroupID=' + $('#WFFlowGroupID').val() +
                   '&WFFlowID=' + $('#WFFlowID').val() +
                   '&WFFlowVer=' + $('#WFFlowVer').val() + 
                   '&ExecAction=' + _ActionTypeAdd;
        var objfeatures = { width: window.screen.availWidth, height: window.screen.availWidth, top: 0, left: 0 };

        _openWin('newwindow', '/Sys/SystemWorkFlowNodeDetail', para, objfeatures);
    }
}

//----Private Function----//
function SysID_onChange(event) {
    $('#WFFlowGroupID > option', _formElement).remove();
    $('#WFCombineKey > option', _formElement).remove();
    $('#WFFlowGroupID', _formElement).combobox('SetSelected', '');
    $('#WFCombineKey', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '') {
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemWorkFlowGroupIDList',
        type: 'POST',
        data: { SysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#WFFlowGroupID', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
                WFFlowGroupID_onChange($('#WFFlowGroupID')[0]);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemWorkFlowGroupIDList);
            _ShowJsErrMessageBox();
        }
    });

}

function WFFlowGroupID_onChange(event) {
    var groupIDValue;

    if (event.select !== undefined) {
        groupIDValue = $(event.select).val();
    } else {
        groupIDValue = $(event).val();
    }

    $.ajax({
        url: '/Sys/GetSysUserSystemWorkFlowIDList',
        type: 'POST',
        data: { sysID: $('#SysID').val(), groupID: groupIDValue },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#WFCombineKey > option', _formElement).remove();
            $('#WFCombineKey', _formElement).combobox('SetSelected', '');
            if (result != null) {
                for (var i = 1; i < result.length; i++) {
                    $('#WFCombineKey', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysUserSystemWorkFlowIDList);
            _ShowJsErrMessageBox();
        }
    });
    WFCombineKey_onChange(null);
}

function WFCombineKey_onChange(srcElement) {
    var combineKey = $('#WFCombineKey').val();

    $('#WFFlowID').val('');
    $('#WFFlowVer').val('');

    if (combineKey == null || combineKey === '') {
        _btnUnblockUI(this, false);
        return false;
    }

    var array = combineKey.split('|');
    if (array.length !== 2) {
        _btnUnblockUI(this, false);
        return false;
    } else {
        $('#WFFlowID').val(array[0]);
        $('#WFFlowVer').val(array[1]);
        return true;
    }
}