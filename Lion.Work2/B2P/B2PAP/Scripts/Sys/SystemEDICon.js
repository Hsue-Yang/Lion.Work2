var _formElement;

function SystemEDIConForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function QuerySysID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#QueryEDIFlowID > option', _formElement).remove();
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEDIFlowList',
        type: 'POST',
        data: { SysID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#QueryEDIFlowID > option', _formElement).remove();
            if (result != null) {
                $('#QueryEDIFlowID > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QueryEDIFlowID', _formElement).append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetFunMenuList);
            _ShowJsErrMessageBox();
        }
    });
}

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

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#EDIFlowID').val(keys[2]);
    $('#EDIConID').val(keys[3]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
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
function SaveButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeUpdate);
    _formElement.submit();

}

function AddButton_onClick(srcElement) {
    var Result = _FormValidation(); //form驗證
    if (Result) {
        $.blockUI({ message: '' }); //按下按鈕跳轉前，畫面會鎖住
        Clean_HiddenValue();

        $('#SysID').val($('#QuerySysID').val());
        $('#EDIFlowID').val($('#QueryEDIFlowID').val());

        $('#ExecAction').val(_ActionTypeAdd); //jQuery套件執行Action (有底線為套件)
        return true;
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeQuery);
    _formElement.submit();
}

//----Private Function----//
function Clean_HiddenValue() { //清空 hidden 內值
    $('#SysID').val('');
    $('#EDIFlowID').val('');
    $('#EDIConID').val('');
}
