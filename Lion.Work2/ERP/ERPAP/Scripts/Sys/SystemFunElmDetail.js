var _formElement;

function SystemFunElmDetailForm_onLoad(formElement) {
    _formElement = formElement;
    if (typeof systemInfo !== 'undefined') {
        $('table.tblvertical #FunControllerID', _formElement).combobox({
            width: 180,
            isRemoveButton: true,
            onChange: FunControllerID_onChange
        });

        $('table.tblvertical #FunActionName', _formElement).combobox({
            width: 180,
            isRemoveButton: true
        });
    }
}

function FunControllerID_onChange(event) {
    $('#FunActionName > option', _formElement).remove();
    $('#FunActionName', _formElement).combobox('SetSelected', '');
    $('#FunActionName').html('<option value=""></option>');

    if (event.select.val()) {
        var sysID = $('#SysID').val();
        $(systemInfo).filter(function (idx, el) {
            return el.Sys.Value === sysID;
        }).map(function (idx, el) {
            $(el.FunActionList).filter(function (idxFun, elFun) {
                return elFun.GroupID === sysID + '|' + event.select.val();
            }).map(function (idxFun, elFun) {
                $('#FunActionName').append('<option value="' + elFun.Value + '">' + elFun.Text + '</option>');
            });
        });
    }

    return true;
}

function SaveButton_onClick(srcElement) {
    var result = _FormValidation() && _FunElmDisplayDefaultTypeValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (result) {
        $.blockUI({ message: '' });
        return true;
    }

    _ShowJsErrMessageBox();
    return false;
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation() && _FunElmDisplayDefaultTypeValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (result) {
        $.blockUI({ message: '' });
        return true;
    }

    _ShowJsErrMessageBox();
    return false;
}

function CancelButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function _FunElmDisplayDefaultTypeValidation(srcElement) {
    if ($('#FunElmDisplayDefaultType:checked').val() === undefined) {
        _AddJsErrMessage(JsMsg_FunElmDisplayDefaultType_Required);
        return false;
    }

    return true;
}

function DeleteButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeDelete);
    return true;
}