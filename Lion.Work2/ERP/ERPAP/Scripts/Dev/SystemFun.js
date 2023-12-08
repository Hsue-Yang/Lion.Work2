var _formElement;

function SystemFunForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemFunTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch,table[id=Pager]').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }
    
    return true;
}

function QuerySysID_onChange(srcElement) {
    if ($(srcElement).val() === '') {
        $('#QueryFunControllerID > option').remove();
        return false;
    }
    
    $.ajax({
        url: '/Dev/GetSystemFunControllerIDList',
        type: 'POST',
        data: { sysID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryFunControllerID > option').remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QueryFunControllerID').append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
            _ShowJsErrMessageBox();
        }
    });
}

function QueryFunMenuSysID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#QueryFunMenu > option').remove();
        return false;
    }

    $.ajax({
        url: '/Dev/GetSystemFunMenuList',
        type: 'POST',
        data: { sysID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryFunMenu > option').remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QueryFunMenu').append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunMenuList);
            _ShowJsErrMessageBox();
        }
    });
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#FunControllerID').val(keys[2]);
    $('#FunActionName').val(keys[3]);
    $('#DevPhase').val(keys[4]);
    $('#IsFun').val(keys[5]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#FunControllerID').val('');
    $('#FunActionName').val('');
    $('#DevPhase').val('');
    $('#IsFun').val('');
}