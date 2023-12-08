var _formElement;

function SystemAPIForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemAPITable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top - $('.tblsearch').height();
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 4 });
    }

    $('.BaseContainer').css('z-index', 0);

    var sysWidth = $('table.tblsearch #QuerySysID', _formElement).width();
    $('table.tblsearch #QuerySysID', _formElement).combobox({
        width: sysWidth,
        isRemoveButton: true,
        onChange: QuerySysID_onChange
    });

    var apiGroupWidth = $('table.tblsearch #QuerySysID', _formElement).width();
    $('table.tblsearch #QueryAPIGroupID', _formElement).combobox({
        width: apiGroupWidth,
        isRemoveButton: true
    });

    return true;
}

function QuerySysID_onChange(event) {
    if (event.select.val() === '') {
        $('#QueryAPIGroupID > option', _formElement).remove();
        $('#QueryAPIGroupID', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemAPIGroupList',
        type: 'POST',
        data: { sysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#QueryAPIGroupID > option', _formElement).remove();
            $('#QueryAPIGroupID', _formElement).combobox('SetSelected', '');
            if (result !== null) {
                $('#QueryAPIGroupID > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QueryAPIGroupID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetFunMenuList);
            _ShowJsErrMessageBox();
        }
    });
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#APIGroupID').val(keys[2]);
    $('#APIFunID').val(keys[3]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyAPIPara_onClick(srcElement, keys) {
    var para = 'SysID=' + keys[1] + '&'
        + 'APIGroupID=' + keys[2] + '&'
        + 'APIFunID=' + keys[3];

    var objfeatures = { width: 450, height: 500 };

    _openWin('newwindow', '/Sys/SystemAPIPara', para, objfeatures);
    return false;
}

function LinkFunKeyAuthorize_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#APIGroupID').val(keys[2]);
    $('#APIFunID').val(keys[3]);

    $('#ExecAction').val(_ActionTypeQuery);
    return true;
}

function LinkFunKeyAPIClient_onClick(srcElement, keys) {
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#APIGroupID').val(keys[2]);
    $('#APIFunID').val(keys[3]);

    $('#ExecAction').val(_ActionTypeQuery);
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
    $('#APIGroupID').val('');
    $('#APIFunID').val('');
}