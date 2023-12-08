var _formElement;

function SystemEDIParaForm_onLoad(formElement) {
    _formElement = formElement;

    $('#SysIDReadOnlyText').val($('#QuerySysID').find("option:selected").text());

    if ($(formElement).find('#SystemEDIParaTable')[0] != null) {
        _TableHover('SystemEDIParaTable', formElement);
    }

    $('#EDIParaType', _formElement).combobox({
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

function SaveButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeUpdate);
    _formElement.submit();

}

function LinkFunKeyDelete_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
        $('#QuerySysID').val(keys[1]);
        $('#QueryEDIFlowID').val(keys[2]);
        $('#QueryEDIJobID').val(keys[3]);
        $('#EDIParaID').val(keys[4]);
    _alert('dialog_Confirm');
}

function ConfirmOKButton_onClick(srcElement) {

    $('#ExecAction').val(_ActionTypeDelete);
        _formElement.submit();

    _btnUnblockUI(this, false);
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

function MoveUp_onClick(srcElement) {
    var After;
    var Current = $('input[name="IsMoved"]:checked').parent().parent();
    var prev = Current.prev();
    if (Current.index() > 1) {
        Current.insertBefore(prev);
        After = prev.find("input[name$='AfterSortOrder']").val();
        prev.find("input[name$='AfterSortOrder']").val(Current.find("input[name$='AfterSortOrder']").val())
        Current.find("input[name$='AfterSortOrder']").val(After);
    }
}


function MoveDown_onClick(srcElement) {
    var After;
    var Count = 0;
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

//----Private Function----//
function Clean_HiddenValue() { //清空 hidden 內值
    $('#QuerySysID').val('');
    $('#EDIFlowID').val('');
    $('#EDIJobID').val('');
}
