var _formElement;

function SystemFunAssignForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function UserID_onBlur(srcElement) {
    var objTextbox = $("input[id^='" + srcElement.id.split('.')[0] + ".UserNM']");

    $.ajax({
        url: '/Sys/GetRAWUserList',
        type: 'POST',
        data: { condition: srcElement.value },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null && result.length == 1) {
                objTextbox.val(result[0].Text);
            }
            else {
                srcElement.value = '';
                objTextbox.val('');
                _AddJsErrMessage(JsMsg_CanNotFindInformation);
                _ShowJsErrMessageBox();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetRAWUserListResult);
            _ShowJsErrMessageBox();
        }
    });
}

//----Tab----//
function SystemFunDetail_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemFunDetail');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeQuery);
    _formElement.submit();
}

function AddRowButton_onClick(srcElement) {
    _addTR("IsProcess");
}

function DeleteRowButton_onClick(srcElement) {
    var objTextbox = $("input[name*='.UserID']");
    var objTR = _GetParentElementByTag(srcElement, 'TR');

    if (objTextbox.length == 1) {
        objTextbox.val('');
        $("input[name*='.UserNM']").val('');
    }
    else {
        $(objTR).find("input[id=IsProcess]").prop("checked", true);
    }

    _deleteTR("IsProcess");
}

//----Private Function----//