var _formElement;

function DomainGroupForm_onLoad(formElement) {
    _formElement = formElement;

    if ($('#DomainPath').val().indexOf('liontech') > -1) {
        $('#DomainSecondLevelPath').next().hide();
    }

    $('table.tblsearch select[name=DomainPath]', _formElement).combobox(
        {
            width: 200,
            isRemoveButton: false,
            onChange: DomainPath_onChange
        }
    );

    $('table.tblsearch select[name=DomainSecondLevelPath]', _formElement).combobox(
        {
            width: 200,
            isRemoveButton: false,
            onChange: DomainSecondLevelPath_onChange
        }
    );

    $('table.tblsearch select[name=DomainThridLevelPath]', _formElement).combobox(
        {
            width: 200,
            isRemoveButton: false
        }
    );

    return true;
}

function DomainPath_onChange(event) {
    var thridLevelSpan = $('#DomainThridLevelPath').closest('span');
    thridLevelSpan.show();

    if (event.select.val().indexOf('liontech') > -1) {
        $('#DomainSecondLevelPath > option', _formElement).remove();
        $('#DomainThridLevelPath > option', _formElement).remove();
        thridLevelSpan.hide();
        $.blockUI({ message: $('#dialog_Confirm') });
    } else {
        _GetDomainSubLevelList({
            LDAPPath: event.select.val(),
            OnSuccess: function (result) {
                $('#DomainSecondLevelPath > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#DomainSecondLevelPath', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }

                $('#DomainSecondLevelPath').combobox('SetSelected', $('#DomainSecondLevelPath').val());

                _GetDomainSubLevelList({
                    LDAPPath: $('#DomainSecondLevelPath', _formElement).val(),
                    OnSuccess: function(subResult) {
                        $('#DomainThridLevelPath > option', _formElement).remove();
                        for (var i = 0; i < subResult.length; i++) {
                            $('#DomainThridLevelPath', _formElement).append('<option value="' + subResult[i].Value + '">' + subResult[i].Text + '</option>');
                        }

                        $('#DomainThridLevelPath').combobox('SetSelected', $('#DomainThridLevelPath').val());
                    }
                });

            }
        });
    }

    return true;
}

function OKButton_onClick(srcElement) {
    if ($('#dialog_Confirm input[name=PWD]').val() !== '') {
        _GetDomainSubLevelList({
            LDAPPath: $('#DomainPath').val(),
            DomainPWD: $('#dialog_Confirm input[name=PWD]').val(),
            OnSuccess: function(result) {
                $('#DomainSecondLevelPath > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#DomainSecondLevelPath', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
                $('#DomainSecondLevelPath').combobox('SetSelected', $('#DomainSecondLevelPath').val());
                _btnUnblockUI(this, false);
            }
        });
    } else {
        _AddJsErrMessage(JsMsg_LDAPPwd_Required);
        _ShowJsErrMessageBox();
        $('#' + _JsErrMessageBox).css('z-index', 9999);
        return false;
    }

    return true;
}

function CancelButton_onClick(srcElement) {
    $('#DomainPath').val($('#DomainPath option:eq(0)').val());
    _btnUnblockUI(this, false);
}

function _GetDomainSubLevelList(parameters) {
    $.ajax({
        url: '/Sys/GetDomainSubLevelList',
        type: 'POST',
        data: {
            LDAPPath: parameters.LDAPPath,
            DomainPWD: parameters.DomainPWD ? parameters.DomainPWD : ''
        },
        dataType: 'json',
        async: true,
        success: function(result) {
            if (result != null) {
                parameters.OnSuccess(result);
            }
        },
        error: function() {
            _AddJsErrMessage(JsMsg_GetDomainSubLevelList_Failure);
            _ShowJsErrMessageBox();
            $('#' + _JsErrMessageBox).css('z-index', 9999);
        }
    });
}

function DomainSecondLevelPath_onChange(event) {
    if ($('#DomainPath').val().indexOf('liontech') === -1) {
        _GetDomainSubLevelList({
            LDAPPath: event.select.val(),
            OnSuccess: function (result) {
                $('#DomainThridLevelPath > option', _formElement).remove();
                for (var i = 0; i < result.length; i++) {
                    $('#DomainThridLevelPath', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }

                $('#DomainThridLevelPath').combobox('SetSelected', $('#DomainThridLevelPath').val());
            }
        });
    }
    return true;
}

function GroupUserLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    $('#DomainPath').val(keys[1]);
    $('#DomainGroupNM').val(keys[2]);
    return true;
}

function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}