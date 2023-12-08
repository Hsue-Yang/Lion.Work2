var _formElement;

function SystemSubsysListForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#SystemSubsysListTable')[0] != null) {
        _TableHover('SystemSubsysListTable', formElement);
    }

    return true;
}

function SubSysID_onBlur(srcElement) {
    var result = false;
    var SubSysID = $('#SubSysID').val();
    var UpdateButton = $('#UpdateButton').val();
    var AddButton = $('#AddButtonHidden').val();

    if (SubSysID != null && SubSysID != "") {
        $('input[name=Table_SubSysID]').each(function () {
            if (this.value == SubSysID) {
                result = true;
            }
        });
    }

    if (result == true) {
        $('#AddButton.btn').attr('value', UpdateButton);
        $('#AddButton.btn').attr('name', 'UpdateButton');
        $('#AddButton.btn').attr('id', 'UpdateButton');
    } else {
        $('#UpdateButton.btn').attr('value', AddButton);
        $('#UpdateButton.btn').attr('name', 'AddButton');
        $('#UpdateButton.btn').attr('id', 'AddButton');

        $('#SaveButton.btn').attr('value', AddButton);
        $('#SaveButton.btn').attr('name', 'AddButton');
        $('#SaveButton.btn').attr('id', 'AddButton');
    }
}

function DeleteLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    $('#SubSysID').attr('value', keys[1]);
    _alert('dialog_Confirm', JsMsg_DeleteWarn);
}

function UpdateLinkFunKey_onClick(srcElement, keys) {
    $('#SubSysID').attr('value', keys[1]);
    $('#SubSysID').attr('ReadOnly', 'true');
    $('#SubSysID').attr('style', 'background-color : #E7E8ED');

    $('#SysNMZHTW').attr('value', keys[2]);
    $('#SysNMZHCN').attr('value', keys[3]);
    $('#SysNMENUS').attr('value', keys[4]);
    $('#SysNMTHTH').attr('value', keys[5]);
    $('#SysNMJAJP').attr('value', keys[6]);
    $('#SortOrder').attr('value', keys[7]);
    $('#SysNMKOKR').attr('value', keys[9]);

    $('#AddButton').attr('value', keys[8]);
    $('#AddButton').attr('name', 'SaveButton');
    $('#AddButton').attr('id', 'SaveButton');
}

function IPListLinkFunKey_onClick(srcElement, keys) {
    var aryPara = [];
    aryPara[aryPara.length] = 'SysID=' + keys[1];
    aryPara[aryPara.length] = 'SysNM=' + encodeURI(keys[2]);
    aryPara[aryPara.length] = 'SubSysID=' + keys[3];
    aryPara[aryPara.length] = 'ExecAction=' + _ActionTypeUpdate;
    
    _openWin("newwindow", "/Sys/SystemIPList", aryPara.join('&'));

    return false;
}

function ServiceListLinkFunKey_onClick(srcElement, keys) {
    var aryPara = [];
    aryPara[aryPara.length] = 'SysID=' + keys[1];
    aryPara[aryPara.length] = 'SysNM=' + encodeURI(keys[2]);
    aryPara[aryPara.length] = 'SubSysID=' + keys[3];
    aryPara[aryPara.length] = 'ExecAction=' + _ActionTypeUpdate;

    _openWin("newwindow", "/Sys/SystemServiceList", aryPara.join('&'));

    return false;
}

function PageSize_onEnter(srcElement) {
    _formElement.submit();
    return true;
}

//----Button----//

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function SaveButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function UpdateButton_onClick(srcElement) {
    _alert('dialog_ConfirmUpdate');
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemSetting';
}

function ConfirmOKButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeDelete);
    _formElement.submit();
}

function ConfirmOKUpdateButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeUpdate);
    _formElement.submit();
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}