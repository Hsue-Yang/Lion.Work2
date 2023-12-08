var _formElement;

function SystemWorkFlowForm_onLoad(formElement) {
    _formElement = formElement;

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: SysID_onChange
    });

    $('table.tblsearch #WFFlowGroupID', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#WFFlowGroupID').val(keys[2]);
    $('#WFFlowID').val(keys[3]);
    $('#WFFlowVer').val(keys[4]);
    
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyNode_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#WFFlowGroupID').val(keys[2]);
    $('#WFFlowID').val(keys[3]);
    $('#WFFlowVer').val(keys[4]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyChart_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#WFFlowGroupID').val(keys[2]);
    $('#WFFlowID').val(keys[3]);
    $('#WFFlowVer').val(keys[4]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

//----Button----//
function SearchButton_onClick(srcElement) {
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
        $.blockUI({ message: '' });

        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#WFFlowGroupID').val('');
    $('#WFFlowID').val('');
    $('#WFFlowVer').val('');
}

function SysID_onChange(event) {
    $('#WFFlowGroupID > option', _formElement).remove();
    $('#WFFlowGroupID', _formElement).combobox('SetSelected', '');
    
    if (event.select.val() == null)
        return false;

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
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemWorkFlowGroupIDList);
            _ShowJsErrMessageBox();
        }
    });
}