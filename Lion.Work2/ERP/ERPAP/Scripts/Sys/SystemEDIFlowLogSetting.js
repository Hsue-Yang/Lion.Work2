var _formElement;

function SystemEDIFlowLogSettingForm_onLoad(formElement) {
    _formElement = formElement;

    $('#SysIDReadOnlyText').val($('#QuerySysID').find("option:selected").text());

    if ($(formElement).find('#SystemEDIFlowLogSettingTable')[0] != null) {
        _TableHover('SystemEDIFlowLogSettingTable', formElement);
    }
    //alert($('#SaveType').val());
    //if ($('#SaveType').val() == true) { _windowClose(); }

    $('table.tblsearch #QueryEDIFlowID', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

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

function CloseButton_onClick(srcElement) {
    _windowClose();
}

//----Private Function----//
function Clean_HiddenValue() { //清空 hidden 內值
    $('#SysID').val('');
    $('#EDIFlowID').val('');
}