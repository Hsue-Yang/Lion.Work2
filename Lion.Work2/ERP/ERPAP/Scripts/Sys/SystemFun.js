var _formElement;

function SystemFunForm_onLoad(formElement) {
    _formElement = formElement;

    var table = $('#SystemFunTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('table.tblsearch:eq(1),table[id=Pager]').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 2 });
    }

    $('.BaseContainer').css('z-index', 0);

    $(systemInfo).map(function(idx, el) {
        $('#QuerySysID').append('<option value="' + el.Sys.Value + '">' + el.Sys.Text + '</option>');
    });
    $('#QuerySysID').val($('#QuerySysID').attr('originalvalue'));
    QuerySysID_onChange({ select: $('#QuerySysID') });
    $('#QuerySubSysID').val($('#QuerySubSysID').attr('originalvalue'));
    $('#QueryFunControllerID').val($('#QueryFunControllerID').attr('originalvalue'));
    QueryFunControllerID_onChange({ select: $('#QueryFunControllerID') });
    $('#QueryFunActionName').val($('#QueryFunActionName').attr('originalvalue'));


    var sysWidth = $('#QuerySysID', _formElement).width();
    $('table.tblsearch #QuerySysID', _formElement).combobox({
        width: sysWidth,
        isRemoveButton: false,
        onChange: QuerySysID_onChange
    });

    $('table.tblsearch #QuerySubSysID', _formElement).combobox({
        width: sysWidth,
        isRemoveButton: true
    });

    $('table.tblsearch #QueryFunMenuSysID', _formElement).combobox({
        width: sysWidth,
        isRemoveButton: true,
        onChange: QueryFunMenuSysID_onChange
    });

    $('table.tblsearch #QueryFunMenu', _formElement).combobox({
        width: sysWidth,
        isRemoveButton: true
    });

    $('table.tblsearch #QueryFunControllerID', _formElement).combobox({
        width: sysWidth,
        isRemoveButton: true,
        onChange: QueryFunControllerID_onChange
    });
    
    $('table.tblsearch #QueryFunActionName', _formElement).combobox({
        width: sysWidth,
        isRemoveButton: true
    });

    $('#PurviewID', $('table.tblsearch', _formElement)).combobox({
        width: sysWidth,
        isRemoveButton: true
    });

    return true;
}

function IsPick_onClick(srcElement) {
    if (srcElement.checked) {
        $('input[name=PickList]').attr('checked', 'checked');
    } else {
        $('input[name=PickList]').removeAttr('checked');
    }
}

function QuerySysID_onChange(event) {
    var querySysValue = event.select.val();
    $('#QuerySubSysID').html('<option value=""></option>');
    $('#QueryFunControllerID').html('<option value=""></option>');
    $('#QueryFunActionName').html('');
    $('#QuerySubSysID', _formElement).combobox('SetSelected', '');
    $('#QueryFunControllerID', _formElement).combobox('SetSelected', '');
    $('#QueryFunActionName', _formElement).combobox('SetSelected', '');
    if (querySysValue !== '') {
        $(systemInfo).filter(function(idx, el) {
            return el.Sys.Value === querySysValue;
        }).map(function(idx, el) {
            $(el.SubSysList).map(function(idxSub, elSub) { $('#QuerySubSysID').append('<option value="' + elSub.Value + '">' + elSub.Text + '</option>'); });
            $(el.FunControllerList).map(function(idxCon, elCon) { $('#QueryFunControllerID').append('<option value="' + elCon.Value + '">' + elCon.Text + '</option>'); });
        });
    }
}

function QueryFunControllerID_onChange(event) {
    $('#QueryFunActionName').html('<option value=""></option>');
    $('#QueryFunActionName', _formElement).combobox('SetSelected', '');
    
    if (event.select.val()) {
        var sysID = $('#QuerySysID').val();
        $(systemInfo).filter(function(idx, el) {
            return el.Sys.Value === sysID;
        }).map(function(idx, el) {
            $(el.FunActionList).filter(function (idxFun, elFun) {
                return elFun.GroupID === sysID + '|' + event.select.val();
            }).map(function (idxFun, elFun) {
                $('#QueryFunActionName').append('<option value="' + elFun.Value + '">' + elFun.Text + '</option>');
            });
        });
    }
}

function QueryFunMenuSysID_onChange(event) {
    if (event.select.val() === '') {
        $('#QueryFunMenu > option', _formElement).remove();
        $('#QueryFunMenu', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSystemFunMenuList',
        type: 'POST',
        data: { sysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryFunMenu > option', _formElement).remove();
                $('#QueryFunMenu', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#QueryFunMenu', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_UnGetSystemFunMenuList);
            _ShowJsErrMessageBox();
        }
    });
    return false;
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#FunControllerID').val(keys[2]);
    $('#FunActionName').val(keys[3]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyCopy_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#FunControllerID').val(keys[2]);
    $('#FunActionName').val(keys[3]);

    $('#ExecAction').val(_ActionTypeCopy);
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
        $('#PageIndex').val(1);
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    if (result) {
        $.blockUI({ message: '' });
        Clean_HiddenValue();

        $('#SysID').val($('#QuerySysID').val());
        $('#FunControllerID').val($('#QueryFunControllerID').val());

        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
    return false;
}

function SaveButton_onClick(srcElement) {
    var result = _FormValidation();

    var dataCount = 10;
    if ($('[name*="PickList"]:checked').length > dataCount) {
        result = false;
        _AddJsErrMessage(String.format(JsMsg_ExceedDataCountLimit, dataCount));
        _ShowJsErrMessageBox();
    }

    if (result) {
        $.blockUI({ message: '' });

        $('#ExecAction').val(_ActionTypeUpdate);
        return true;
    }
    return false;
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#FunControllerID').val('');
    $('#FunActionName').val('');
}