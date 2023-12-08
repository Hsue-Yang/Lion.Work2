var _formElement;

function SystemEDIJobLogForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

//連動
function QuerySysID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#QueryEDIFlowID > option', _formElement).remove();
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEDIFlowList',
        type: 'POST',
        data: { sysID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryEDIFlowID > option').remove();
                for (var i = 1; i < result.length; i++) {
                    $('#QueryEDIFlowID').append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
            _ShowJsErrMessageBox();
        }
    });
    QueryEDIFlowID_onChange();
}
function QueryEDIFlowID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#QueryEDIJobID > option', _formElement).remove();
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEDIJobList',
        type: 'POST',
        data: { SysID: $('#QuerySysID').val(), EDIFlowID: $('#QueryEDIFlowID').val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryEDIJobID > option').remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QueryEDIJobID').append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
            _ShowJsErrMessageBox();
        }
    });
}

function PageSize_onEnter(srcElement) {
    SearchButton_onClick();
    return true;
}

//----Button----//
function SearchButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
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

//----Private Function----//
function Clean_HiddenValue() { //清空 hidden 內值
    $('#SysID').val('');
    $('#EDIFlowID').val('');
}