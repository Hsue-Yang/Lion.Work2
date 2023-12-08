var _formElement;

function SystemEventGroupForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemEventGroupTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('table.tblsearch #QuerySysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: QuerySysID_onChange
    });

    $('table.tblsearch #QueryEventGroupID', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });

    return true;
}

function QuerySysID_onChange(event) {
    if (event.select.val() === '') {
        $('#QueryEventGroupID > option', _formElement).remove();
        $('#QueryEventGroupID', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEventGroupByIdList',
        type: 'POST',
        data: { sysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryEventGroupID > option', _formElement).remove();
                $('#QueryEventGroupID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#QueryEventGroupID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_UnGetSysSystemEventGroupByIdList);
            _ShowJsErrMessageBox();
        }
    });
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#EventGroupID').val(keys[2]);

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

        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#EventGroupID').val('');
}