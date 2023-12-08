var _formElement;

function SystemEDIFlowDetailForm_onLoad(formElement) {
    _formElement = formElement;

    $('#SysIDReadOnlyText').val($('#SysID').find("option:selected").text());
    $('#EDIFlowIDReadOnlyText').val($('#EDIFlowID').val());
    if ($('#SCHFrequency').val() != "FixedTime")
    {
    $('#SCHIntervalTime').hide();
    $('#Label_SCHIntervalTime').hide();
    $('#SCHIntervalTime_td').hide();
    $('#SCHEndTime').hide();
    $('#Label_SCHEndTime').hide();
    $('#SCHEndTime_td').hide();
    }
    return true;
}

function SCHFrequency_onChange(srcElement) {
    if ($('#SCHFrequency').val() == "FixedTime") {
        $('#SCHIntervalTime').show();
        $('#Label_SCHIntervalTime').show();
        $('#SCHIntervalTime_td').show();
        $('#SCHEndTime').show();
        $('#Label_SCHEndTime').show();
        $('#SCHEndTime_td').show();
    } else {
        $('#SCHIntervalTime').hide();
        $('#Label_SCHIntervalTime').hide();
        $('#SCHIntervalTime_td').hide();
        $('#SCHEndTime').hide();
        $('#Label_SCHEndTime').hide();
        $('#SCHEndTime_td').hide();
    }
}

//----Button----//

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if ($('#SCHStartTime').val() > 235959999) { $('#SCHStartTime').val(235959999) }
    if ($('#SCHEndTime').val() > 235959999) { $('#SCHEndTime').val(235959999) }
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if ($('#SCHStartTime').val() > 235959999) { $('#SCHStartTime').val(235959999) }
    if ($('#SCHEndTime').val() > 235959999) { $('#SCHEndTime').val(235959999) }
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function DeleteButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    _alert('dialog_Confirm');
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function ConfirmOKButton_onClick(srcElement) {
    var Result = _FormValidation();

    $('#ExecAction').val(_ActionTypeDelete);
    if (Result) {
        _formElement.submit();
    }

    _btnUnblockUI(this, false);
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}