var _formElement;

function SystemEDIFlowForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#EDIFlowID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyJob_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#QuerySysID').val(keys[1]);
    $('#QueryEDIFlowID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyCon_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#QuerySysID').val(keys[1]);
    $('#QueryEDIFlowID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyFlowLog_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#QuerySysID').val(keys[1]);
    $('#QueryEDIFlowID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}
//排序
function MoveUp_onClick(srcElement) {
    var After;
    var Current = $('input[name="IsMoved"]:checked').parent().parent();
    var prev = Current.prev();
    if (Current.index() > 1) {
        Current.insertBefore(prev);
        After = prev.find("input[name$='AfterSortOrder']").val();
        if (After != null) {
            prev.find("input[name$='AfterSortOrder']").val(Current.find("input[name$='AfterSortOrder']").val())
            Current.find("input[name$='AfterSortOrder']").val(After);
        }
    }
}

function MoveDown_onClick(srcElement) {
    var After;
    var Current = $('input[name="IsMoved"]:checked').parent().parent();
    var next = Current.next();
    if (next) {
        Current.insertAfter(next);
        After = next.find("input[name$='AfterSortOrder']").val();
        if (After != null) {
            next.find("input[name$='AfterSortOrder']").val(Current.find("input[name$='AfterSortOrder']").val())
            Current.find("input[name$='AfterSortOrder']").val(After);
        }
    }
}

//----Button----//
function SearchButton_onClick(srcElement) { //按下搜尋，驗證combobox有無值
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    var Result = _FormValidation(); //form驗證
    if (Result) {
        $.blockUI({ message: '' }); //按下按鈕跳轉前，畫面會鎖住
        Clean_HiddenValue();

        $('#SysID').val($('#QuerySysID').val());
        $('#ExecAction').val(_ActionTypeAdd); //jQuery套件執行Action (有底線為套件)
        return true;
    }
}

function OutputButton_onClick(srcElement) {
    $('#SysID').val($('#QuerySysID').val());
        $.blockUI({ message: '' });

        _alert('dialog_Confirm');
}
function SaveButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeUpdate);
    _formElement.submit();

}
//----Private Function----//
function Clean_HiddenValue() { //清空 hidden 內值
    $('#SysID').val('');
    $('#EDIFlowID').val('');
}

function ConfirmOKButton_onClick(srcElement) {
    var Result = _FormValidation();

    $('#ExecAction').val(_ActionTypeCopy);
    if (Result) {
        _formElement.submit();
    }

    _btnUnblockUI(this, false);
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}